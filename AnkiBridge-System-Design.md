# AnkiBridge — Tài liệu thiết kế hệ thống

> Self-hosted app hỗ trợ tạo flashcard Anki để học active vocabulary bằng cách scrape Cambridge Dictionary, quản lý learning entries, và export sang Anki qua AnkiConnect.

---

## 1. Tổng quan

### 1.1 Mục tiêu
Người dùng nhập từ/cụm từ → hệ thống scrape Cambridge Dictionary → người dùng chọn/chỉnh sửa dữ liệu để tạo **Learning Entry** (snapshot cá nhân) → tạo **Anki Note** (gắn deck + note type) → export bất đồng bộ sang Anki qua AnkiConnect.

Toàn bộ hệ thống chạy **self-hosted, single-user, không có login**, triển khai bằng `git clone` + `docker compose up -d`, không cần cài thêm gì khác ngoài Docker và Anki (kèm AnkiConnect) trên máy người dùng.

### 1.2 Tech stack
| Layer | Công nghệ |
|---|---|
| Runtime | .NET 10 |
| UI | Blazor Server (interactive), Fluent UI Blazor |
| Kiến trúc | Monolith, Domain-Driven Design, Event-Driven (qua Outbox) |
| Application | MediatR (CQRS) |
| Data access | EF Core (Code First + Migrations) |
| Database | PostgreSQL |
| Object storage | Azure Blob Storage (Azurite khi self-hosted) |
| Scraping | AngleSharp |
| Dev orchestration | .NET Aspire (AppHost) |
| Distribution | Docker Compose (hand-tuned, build sẵn qua CI) |
| Tích hợp ngoài | AnkiConnect, Pixabay API, Pexels API, Google Translate (unofficial), Google Translate TTS (unofficial) |

### 1.3 Nguyên tắc thiết kế xuyên suốt
- **DictionaryEntry = cache**, không phải aggregate người dùng sở hữu → không audit, không soft-delete.
- **LearningEntry = snapshot**, độc lập vòng đời với DictionaryEntry gốc → không có FK cứng tới DictionaryEntry.
- **Note = export job**, không lưu bản sao dữ liệu đã export → export lại luôn đọc dữ liệu **hiện tại** của LearningEntry.
- Mọi enum phân loại (Source, Accent, Status...) lưu dạng `string` trong DB (không dùng `int`) để tránh lỗi âm thầm khi enum C# bị chèn/đổi thứ tự giá trị theo thời gian.

---

## 2. Kiến trúc tổng thể — Bounded Contexts

```
┌─────────────────┐     ┌──────────────────┐     ┌───────────────────┐
│   Dictionary     │     │     Learning      │     │     Flashcard      │
│  (cache scrape)  │────▶│  (owned aggregate)│────▶│   (export sang     │
│                  │     │    = snapshot     │     │   Anki qua         │
│                  │     │                   │     │   AnkiConnect)     │
└─────────────────┘     └──────────────────┘     └───────────────────┘
                                                             │
                                                     ┌───────▼────────┐
                                                     │     Outbox      │
                                                     │ (event-driven,  │
                                                     │  async worker)  │
                                                     └─────────────────┘
```

- **Dictionary**: quản lý việc scrape Cambridge, lưu cache kết quả để tránh scrape lặp.
- **Learning**: aggregate chính người dùng thao tác — tạo/sửa/xóa Learning Entry.
- **Flashcard**: quản lý cache Deck/NoteType lấy từ Anki, và các job export (Note).
- **Outbox**: đảm bảo mọi side-effect bất đồng bộ (tải media, gọi AnkiConnect) được xử lý đáng tin cậy (at-least-once).

---

## 3. Domain Model

### 3.1 Dictionary Context — cache, KHÔNG audit/soft-delete

**DictionaryEntry** (aggregate root)
- `Headword`, `PartOfSpeech`, `Source` (luôn `"Cambridge"` — không hỗ trợ user tự tạo thủ công)
- Unique theo `(Headword, PartOfSpeech)` — **cache-first**: search trùng từ thì trả cache có sẵn, không scrape lại; nếu cố tình trigger scrape cho từ đã tồn tại → báo lỗi, không tạo bản ghi mới.

Children (cascade delete theo DictionaryEntry):
- `DictionaryDefinition` (Text, OrderIndex) → `DictionaryExample` (Text)
- `DictionaryTranslation` (Text, Source ∈ {Cambridge, GoogleTranslate})
- `DictionaryImage` (Url, Source ∈ {Pixabay, Pexels}, **+OrderIndex** — 3 ảnh candidate lấy lúc scrape)
- `Pronunciation` (Ipa, Accent ∈ {Us, Uk}, AudioUrl, AudioSource=Cambridge)

> Lưu ý: `DictionaryImage`/`Pronunciation` được điền **ngay lúc scrape** (cùng lúc với Definition/Translation), không phải lúc user chọn — khớp với "tìm thấy thì lưu dictionary entries vào DB".

### 3.2 Learning Context — aggregate người dùng sở hữu, soft-delete

**LearningEntry** (aggregate root)

| Field | Bắt buộc | Ghi chú |
|---|---|---|
| `Headword`, `PartOfSpeech` | ✔ | |
| `Cloze` | ✔ | Sinh tự động: che nguyên âm bằng `_`, ẩn ngẫu nhiên thêm 1 số phụ âm |
| `Definition` | ✔ | |
| `Translation`, `TranslationSource` | ✔ | Source ∈ {User, Cambridge, GoogleTranslate} |
| `Accent`, `Ipa` | ✔ | Bắt buộc trong **mọi** trường hợp, kể cả nhập tay hoàn toàn |
| `AudioSource`, `AudioPath` | optional | Source ∈ {User, Cambridge, GoogleTts} |
| `AudioDownloadStatus` | optional | **[mới]** enum {Pending, Completed, Failed}, null nếu không chọn audio |
| `ImageSource`, `ImagePath` | optional | Source ∈ {User, Pixabay, Pexels} |
| `ImageDownloadStatus` | optional | **[mới]** cùng enum như trên |
| `DictionaryEntryId` | optional, **không FK** | Chỉ để trace nguồn gốc (null = tạo tay, có giá trị = từng scrape). Có index thường (không FK) để query ngược. |

Children: `LearningExample` (Text, **+OrderIndex** — giữ đúng thứ tự 3 ví dụ).

> **Vì sao thêm `AudioDownloadStatus`/`ImageDownloadStatus`:** việc tải audio/image về Azurite là **bất đồng bộ** (xác nhận #4). Giữa lúc save và lúc tải xong, `AudioPath`/`ImagePath` vẫn còn null — nếu không có status, UI không phân biệt được "chưa chọn media" và "đang tải". Đây là field tôi đề xuất thêm, bạn duyệt lại giúp.

**Đã xác nhận luồng tìm media khi tạo tay (dictionary source = user):** ngay cả khi không scrape, form Learning Entry vẫn cho phép tìm audio (Google TTS) / ảnh (Pixabay → fallback Pexels) độc lập ngay trong form. Kết quả tìm kiếm này chỉ hiển thị tạm (không lưu DB) cho tới khi user chọn 1 candidate — lúc đó mới thuộc vào luồng tải-về-Azurite lúc save.

### 3.3 Flashcard Context — cache + export job

**Deck** / **NoteType** — cache local của Anki, sync qua AnkiConnect mỗi lần mở dialog tạo note (upsert theo `ExternalId`). `ExternalId` cần **unique index** để tránh trùng khi upsert nhiều lần.

**Note** (aggregate — export job, KHÔNG lưu snapshot field value)

| Field | Ghi chú |
|---|---|
| `LearningEntryId`, `NoteTypeId`, `DeckId` | Unique theo bộ 3 — 1 learning entry chỉ có 1 note trên mỗi cặp (deck, note type) |
| `Status` | enum **string** (sửa từ `int`): `PendingExport → Exporting → Exported / Failed` |
| `ExternalId` | Anki note ID (có sau khi export thành công lần đầu) |
| `ExportedAt` | |

**Luồng export lại:** nút "Export lại" → nếu `ExternalId` đã có (từng export thành công) → gọi `updateNoteFields`; nếu chưa từng export/đang Failed → gọi `addNote`. Không tạo Note mới, chỉ update lại record cũ + set `Status = Exporting`. Dữ liệu field luôn đọc **trực tiếp từ LearningEntry hiện tại** tại thời điểm export — không có snapshot lịch sử, và Anki là nguồn sự thật cuối cùng sau khi export.

**CardTemplate / CardType — Phase 2 (không thuộc MVP)**

Bạn xác nhận 2 bảng này hiện thừa nhưng muốn giữ để mở rộng **preview card độc lập trong app** (không liên quan gì tới NoteType/Note thật của Anki). Thiết kế đề xuất:
- `CardTemplate` = 1 bộ preview cục bộ (Name, Description, Css dùng chung).
- `CardType` = từng mặt preview trong bộ đó (FrontHtml/BackHtml, dùng placeholder trùng tên field chuẩn ở mục 6, ví dụ `{{Headword}}`, `{{Cloze}}`).
- Không có FK nào tới `Note`/`NoteType` — đây là tính năng hiển thị-trước hoàn toàn tách biệt khỏi export thật.
- **Đề xuất:** bỏ 2 bảng này khỏi migration đầu tiên (MVP), thêm lại bằng 1 migration riêng khi thật sự build tính năng preview, để schema v1 gọn.

### 3.4 Outbox — infra

`OutboxMessages(Id, CreationDate, Payload jsonb, PayloadType, ProcessedDate, ProcessedCount, Error, NextRetryAt)`

**[mới]** thêm 2 cột theo yêu cầu #9:
- `Error` (text, nullable) — lỗi lần xử lý gần nhất, phục vụ debug.
- `NextRetryAt` (timestamptz, nullable, default = now) — cho phép backoff đơn giản: worker query `WHERE ProcessedDate IS NULL AND NextRetryAt <= now() ORDER BY CreationDate`, tránh vòng lặp retry dồn dập khi AnkiConnect/Cambridge tạm lỗi. Có thể tăng dần `NextRetryAt` theo `ProcessedCount` (exponential backoff) trong processor.

---

## 4. Bảng enum tổng hợp

| Enum | Giá trị | Dùng ở |
|---|---|---|
| `DictionarySource` | Cambridge | `DictionaryEntry.Source` (cố định) |
| `TranslationSource` | User, Cambridge, GoogleTranslate | `DictionaryTranslation.Source`, `LearningEntry.TranslationSource` |
| `AudioSource` | User, Cambridge, GoogleTts | `Pronunciation.AudioSource`, `LearningEntry.AudioSource` |
| `ImageSource` | User, Pixabay, Pexels | `DictionaryImage.Source`, `LearningEntry.ImageSource` |
| `Accent` | Us, Uk | `Pronunciation.Accent`, `LearningEntry.Accent` |
| `MediaDownloadStatus` **[mới]** | Pending, Completed, Failed | `LearningEntry.AudioDownloadStatus`/`ImageDownloadStatus` |
| `NoteStatus` | PendingExport, Exporting, Exported, Failed | `Note.Status` |
| `PartOfSpeech` | Noun, Verb, Adjective, Adverb, Collocation, Idiom, Phrase, Pronoun, Determiner, Preposition, Conjunction, AuxiliaryVerb, ModalVerb, PhrasalVerb, Number, OrdinalNumber, Prefix, Suffix, Exclamation, Other | `DictionaryEntry.PartOfSpeech`, `LearningEntry.PartOfSpeech` |

Tất cả enum lưu DB dạng **string** (kể cả `NoteStatus`, sửa từ `int` ban đầu — lý do đã nêu ở mục 1.3).

---

## 5. Luồng nghiệp vụ chính

### 5.1 Search & Scrape
1. User nhập từ/cụm từ → bấm "Tìm kiếm".
2. Handler kiểm tra `DictionaryEntry` theo `(Headword, PartOfSpeech tương ứng nếu có)`:
   - **Có sẵn** → trả về cache ngay, không scrape.
   - **Chưa có** → scrape Cambridge bằng AngleSharp:
     - Không tìm thấy → báo lỗi "không tìm thấy".
     - Tìm thấy → tạo `DictionaryEntry` + children (Definition/Example/Translation/Pronunciation), đồng thời gọi Pixabay (fallback Pexels) lấy 3 ảnh candidate → lưu `DictionaryImage`. Báo tìm kiếm thành công.
3. Nếu cùng lúc tìm thấy nhiều entries (nhiều part of speech/nghĩa) → hiển thị danh sách cho user chọn 1.

### 5.2 Chọn & Lưu Learning Entry
1. User chọn 1 `DictionaryEntry` (hoặc bắt đầu form trống nếu nhập tay) → form tự điền toàn bộ field theo entry đã chọn (quan hệ cha-con: đổi entry → toàn bộ field con đổi theo).
2. User chỉnh sửa tùy ý, chọn 1 audio candidate + 1 image candidate (hoặc tự upload / tự tìm Pixabay/Pexels/Google TTS ngay trong form).
3. Bấm Lưu:
   - Ghi `LearningEntry` (+3 `LearningExample`) ngay lập tức, với `AudioDownloadStatus`/`ImageDownloadStatus = Pending` nếu có chọn media từ nguồn ngoài (Cambridge/GoogleTts/Pixabay/Pexels); nếu user tự upload file thì tải lên Azurite luôn, đồng bộ, `Status = Completed` ngay.
   - Publish domain event qua Outbox (`LearningEntryMediaDownloadRequested`) kèm URL nguồn.
   - Background worker: tải file → upload Azurite → update `AudioPath`/`ImagePath` + `Status = Completed` (hoặc `Failed` nếu lỗi, cho phép user bấm "thử tải lại" trên UI).
4. Sau khi tạo, user sửa/xóa (soft-delete) bình thường.

### 5.3 Tạo Anki Note
1. User chọn 1 hoặc nhiều Learning Entry → bấm "Tạo Anki Note".
2. Dialog gọi AnkiConnect lấy danh sách Deck + NoteType hiện có → upsert vào cache local (`Deck`/`NoteType`).
3. User chọn 1 Deck + 1 NoteType → tạo `Note` (`Status = PendingExport`). Chặn trùng theo unique `(LearningEntryId, NoteTypeId, DeckId)`.

### 5.4 Export sang Anki
1. User chọn các Note ở trạng thái `PendingExport`/`Failed` → bấm "Export".
2. `Status = Exporting`, publish event qua Outbox.
3. Background worker: đọc field hiện tại từ `LearningEntry` → build payload theo quy ước field chuẩn (mục 6) → đọc bytes audio/image từ Azurite → gọi AnkiConnect `storeMediaFile` rồi `addNote` (lần đầu) hoặc `updateNoteFields` (export lại) → cập nhật `Status = Exported` + `ExternalId` + `ExportedAt`, hoặc `Status = Failed` (+ lý do lỗi) nếu thất bại — **không tự động retry**, user tự bấm "Export lại".

---

## 6. Quy ước tích hợp Anki

### 6.1 Field convention cố định (hướng a)
AnkiBridge quy ước **cứng** 1 bộ tên field, tự tạo sẵn Note Type mẫu qua `createModel` (kiểm tra tồn tại qua `modelNames` trước, idempotent) lúc khởi động lần đầu:

```
Headword, PartOfSpeech, Cloze, Definition, Translation,
Example1, Example2, Example3, IPA, Audio, Image
```

- `Audio` field nhận giá trị dạng `[sound:<filename>]`, `Image` dạng `<img src="<filename>">` — filename lấy từ kết quả `storeMediaFile`.
- User có thể dùng note type do AnkiBridge tự tạo, hoặc tự đặt tên field trùng convention trên note type riêng của mình để tương thích. Note type có field không khớp convention (vd Anki "Basic" mặc định: Front/Back) sẽ không tự map được.
- Thiết kế HTML/CSS mặc định cho `qfmt`/`afmt` của note type mẫu để lại làm chi tiết implementation, không chốt trong tài liệu này.

### 6.2 Yêu cầu setup AnkiConnect (quan trọng, cần ghi trong README)
- Mặc định AnkiConnect chỉ bind `127.0.0.1` — cần đổi `webBindAddress` thành `0.0.0.0` trong config addon để container gọi được từ `host.docker.internal`.
- Cần thêm origin của AnkiBridge vào `webCorsOriginList` (hoặc `*` nếu chấp nhận rủi ro, chỉ dùng mạng nội bộ tin cậy).
- User **phải tự cài Anki + AnkiConnect addon** trên máy host — phần "không cần cài gì thêm" chỉ áp dụng cho AnkiBridge.

---

## 7. Migration — thay đổi so với bản đầu

| # | Thay đổi | Bảng |
|---|---|---|
| 1 | Thêm index thường (không FK) trên `DictionaryEntryId` | LearningEntry |
| 2 | Thêm unique index trên `ExternalId` | Deck, NoteType |
| 3 | Thêm cột `AudioDownloadStatus`, `ImageDownloadStatus` (varchar, nullable) | LearningEntry |
| 4 | Thêm cột `OrderIndex` (int) | LearningExample, DictionaryImage |
| 5 | Đổi `Status` từ `int` sang `varchar` (`HasConversion<string>()`) | Note |
| 6 | Thêm cột `Error` (text, nullable), `NextRetryAt` (timestamptz, nullable) | OutboxMessages |
| 7 | Bỏ khỏi migration MVP, để dành Phase 2 | CardTemplate, CardType |

---

## 8. Triển khai self-hosted (Docker)

### 8.1 Chiến lược
- **Dev-loop**: dùng .NET Aspire AppHost bình thường (`dotnet run` trên AppHost) — Aspire tự quản lý container Postgres/Azurite lúc dev, không cần docker-compose.
- **Distribution**: KHÔNG bắt end-user chạy Aspire CLI. Thay vào đó:
  1. CI (GitHub Actions) build image AnkiBridge, push lên GitHub Container Registry (GHCR) mỗi lần release.
  2. Commit sẵn `docker-compose.yml` + `.env.example` (đã trỏ `ghcr.io/<user>/ankibridge:<tag>`) vào repo.
  3. End-user chỉ `git clone` → copy `.env.example` → `docker compose up -d`. Không cần .NET SDK, không cần Aspire CLI.
- File `docker-compose.yml` được **maintain thủ công** (có thể khởi tạo từ `aspire publish` rồi tinh chỉnh thêm) thay vì phụ thuộc hoàn toàn vào auto-generator của `Aspire.Hosting.Docker` — vì tính năng publisher này còn đang phát triển nhanh, và cần kiểm soát chặt phần `depends_on`/healthcheck cho 1 artifact phân phối công khai.

### 8.2 Danh sách services

| Service | Vai trò |
|---|---|
| `postgres` | Database, có `healthcheck` (`pg_isready`), volume persist |
| `azurite` | Blob storage emulator, volume persist |
| `migrator` | One-shot container chạy `dotnet ef database update` (hoặc `context.Database.Migrate()`), chạy xong thì exit, `depends_on: postgres (service_healthy)` |
| `ankibridge` | App chính, `depends_on: migrator (service_completed_successfully)`, `postgres (service_healthy)`, `azurite` |

### 8.3 `docker-compose.yml` mẫu (tham khảo, cần test lại khi implement)

```yaml
services:
  postgres:
    image: postgres:17-alpine
    environment:
      POSTGRES_USER: ankibridge
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
      POSTGRES_DB: ankibridge
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ankibridge"]
      interval: 5s
      timeout: 5s
      retries: 10
    restart: unless-stopped

  azurite:
    image: mcr.microsoft.com/azure-storage/azurite:latest
    command: "azurite-blob --blobHost 0.0.0.0"
    volumes:
      - azurite-data:/data
    restart: unless-stopped

  migrator:
    image: ghcr.io/<user>/ankibridge-migrator:${TAG:-latest}
    environment:
      ConnectionStrings__Default: "Host=postgres;Database=ankibridge;Username=ankibridge;Password=${POSTGRES_PASSWORD}"
    depends_on:
      postgres:
        condition: service_healthy
    restart: "no"

  ankibridge:
    image: ghcr.io/<user>/ankibridge:${TAG:-latest}
    ports:
      - "8080:8080"
    environment:
      ConnectionStrings__Default: "Host=postgres;Database=ankibridge;Username=ankibridge;Password=${POSTGRES_PASSWORD}"
      Azurite__ConnectionString: "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=...;BlobEndpoint=http://azurite:10000/devstoreaccount1;"
      AnkiConnect__BaseUrl: "http://host.docker.internal:8765"
      Pixabay__ApiKey: ${PIXABAY_API_KEY}
      Pexels__ApiKey: ${PEXELS_API_KEY}
    extra_hosts:
      - "host.docker.internal:host-gateway"
    depends_on:
      migrator:
        condition: service_completed_successfully
      postgres:
        condition: service_healthy
      azurite:
        condition: service_started
    restart: unless-stopped

volumes:
  pgdata:
  azurite-data:
```

### 8.4 `.env.example`

```
POSTGRES_PASSWORD=changeme
PIXABAY_API_KEY=
PEXELS_API_KEY=
TAG=latest
```

### 8.5 Lưu ý bảo mật
- App **không có login** — chỉ nên chạy trong mạng nội bộ tin cậy (LAN/VPN cá nhân). Nếu muốn expose ra internet, khuyến nghị đặt sau reverse proxy (Caddy/Nginx) có basic auth hoặc VPN, việc này nằm ngoài phạm vi `docker-compose.yml` gốc.
- Google Translate / Google Translate TTS dùng endpoint không chính thức — không có SLA, có thể bị chặn/đổi bất kỳ lúc nào; nên thiết kế theo interface để dễ thay bằng API chính thức sau này nếu cần.

---

## 9. Ghi chú triển khai (không chặn thiết kế, quyết định lúc code)

Các mục dưới đây đã được xác nhận là **để mở có chủ đích** — không cần chốt trước khi bắt đầu implement, chỉ là tham số/chi tiết sẽ tinh chỉnh trong lúc code:

1. Mapping nhãn part-of-speech thật của Cambridge (vd `"noun"`, `"phrasal verb"`...) sang giá trị enum `PartOfSpeech` ở mục 4 — cần bảng mapping cụ thể lúc viết scraper (AngleSharp).
2. Thiết kế HTML/CSS mặc định (`qfmt`/`afmt`) cho Note Type tự tạo qua `createModel` (mục 6.1) — sẽ chốt khi implement export.
3. UI flow chi tiết cho CardTemplate/CardType preview (Phase 2, mục 3.3) — chưa cần thiết kế ngay.
4. Vài tham số tinh chỉnh nhỏ khác không ảnh hưởng domain model: tỉ lệ mask phụ âm ngẫu nhiên trong thuật toán sinh Cloze, khoảng backoff cụ thể cho `NextRetryAt`, ngưỡng số lần retry tối đa trước khi Outbox message coi là "dead" — có thể để config/appsettings thay vì hard-code.
