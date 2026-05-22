using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnkiBridge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AnkiIntegration");

            migrationBuilder.EnsureSchema(
                name: "Flashcard");

            migrationBuilder.EnsureSchema(
                name: "Dictionary");

            migrationBuilder.EnsureSchema(
                name: "Learning");

            migrationBuilder.EnsureSchema(
                name: "TransactionalOutbox");

            migrationBuilder.CreateTable(
                name: "AnkiDeck",
                schema: "AnkiIntegration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExternalId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnkiDeck", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnkiNoteType",
                schema: "AnkiIntegration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExternalId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnkiNoteType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardTemplate",
                schema: "Flashcard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Css = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryEntry",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Headword = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PartOfSpeech = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LearningEntry",
                schema: "Learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Headword = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PartOfSpeech = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Ipa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Accent = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Cloze = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Definition = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AudioPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "TransactionalOutbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    PayloadType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    ProcessedCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardType",
                schema: "Flashcard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FrontHtml = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    BackHtml = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardType_CardTemplate_CardTemplateId",
                        column: x => x.CardTemplateId,
                        principalSchema: "Flashcard",
                        principalTable: "CardTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryDefinition",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryDefinition_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryImage",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryImage_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pronunciation",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ipa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Accent = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AudioUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AudioSource = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pronunciation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pronunciation_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnkiNote",
                schema: "AnkiIntegration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LearningEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeckId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExternalId = table.Column<long>(type: "bigint", nullable: false),
                    ExportedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnkiNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnkiNote_AnkiDeck_DeckId",
                        column: x => x.DeckId,
                        principalSchema: "AnkiIntegration",
                        principalTable: "AnkiDeck",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnkiNote_AnkiNoteType_NoteTypeId",
                        column: x => x.NoteTypeId,
                        principalSchema: "AnkiIntegration",
                        principalTable: "AnkiNoteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnkiNote_LearningEntry_LearningEntryId",
                        column: x => x.LearningEntryId,
                        principalSchema: "Learning",
                        principalTable: "LearningEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningExample",
                schema: "Learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningExample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningExample_LearningEntry_LearningEntryId",
                        column: x => x.LearningEntryId,
                        principalSchema: "Learning",
                        principalTable: "LearningEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryExample",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefinitionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryExample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryExample_EntryDefinition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalSchema: "Dictionary",
                        principalTable: "EntryDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnkiDeck_ExternalId",
                schema: "AnkiIntegration",
                table: "AnkiDeck",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnkiNote_DeckId",
                schema: "AnkiIntegration",
                table: "AnkiNote",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_AnkiNote_LearningEntryId_NoteTypeId_DeckId",
                schema: "AnkiIntegration",
                table: "AnkiNote",
                columns: new[] { "LearningEntryId", "NoteTypeId", "DeckId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnkiNote_NoteTypeId",
                schema: "AnkiIntegration",
                table: "AnkiNote",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnkiNoteType_ExternalId",
                schema: "AnkiIntegration",
                table: "AnkiNoteType",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_CardTemplate_CreatedAt",
                schema: "Flashcard",
                table: "CardTemplate",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CardTemplate_Name",
                schema: "Flashcard",
                table: "CardTemplate",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_CardTemplateId",
                schema: "Flashcard",
                table: "CardType",
                column: "CardTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_CardTemplateId_Name",
                schema: "Flashcard",
                table: "CardType",
                columns: new[] { "CardTemplateId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntry_Headword",
                schema: "Dictionary",
                table: "DictionaryEntry",
                column: "Headword");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntry_Headword_PartOfSpeech",
                schema: "Dictionary",
                table: "DictionaryEntry",
                columns: new[] { "Headword", "PartOfSpeech" });

            migrationBuilder.CreateIndex(
                name: "IX_EntryDefinition_DictionaryEntryId",
                schema: "Dictionary",
                table: "EntryDefinition",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryExample_DefinitionId",
                schema: "Dictionary",
                table: "EntryExample",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryImage_DictionaryEntryId",
                schema: "Dictionary",
                table: "EntryImage",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningEntry_CreatedAt",
                schema: "Learning",
                table: "LearningEntry",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LearningEntry_Headword",
                schema: "Learning",
                table: "LearningEntry",
                column: "Headword");

            migrationBuilder.CreateIndex(
                name: "IX_LearningEntry_Headword_PartOfSpeech",
                schema: "Learning",
                table: "LearningEntry",
                columns: new[] { "Headword", "PartOfSpeech" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningExample_LearningEntryId",
                schema: "Learning",
                table: "LearningExample",
                column: "LearningEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_ProcessedCount",
                schema: "TransactionalOutbox",
                table: "OutboxMessages",
                column: "ProcessedCount");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_ProcessedDate_CreationDate",
                schema: "TransactionalOutbox",
                table: "OutboxMessages",
                columns: new[] { "ProcessedDate", "CreationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Pronunciation_DictionaryEntryId",
                schema: "Dictionary",
                table: "Pronunciation",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pronunciation_Ipa_Accent",
                schema: "Dictionary",
                table: "Pronunciation",
                columns: new[] { "Ipa", "Accent" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnkiNote",
                schema: "AnkiIntegration");

            migrationBuilder.DropTable(
                name: "CardType",
                schema: "Flashcard");

            migrationBuilder.DropTable(
                name: "EntryExample",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "EntryImage",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "LearningExample",
                schema: "Learning");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "TransactionalOutbox");

            migrationBuilder.DropTable(
                name: "Pronunciation",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "AnkiDeck",
                schema: "AnkiIntegration");

            migrationBuilder.DropTable(
                name: "AnkiNoteType",
                schema: "AnkiIntegration");

            migrationBuilder.DropTable(
                name: "CardTemplate",
                schema: "Flashcard");

            migrationBuilder.DropTable(
                name: "EntryDefinition",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "LearningEntry",
                schema: "Learning");

            migrationBuilder.DropTable(
                name: "DictionaryEntry",
                schema: "Dictionary");
        }
    }
}
