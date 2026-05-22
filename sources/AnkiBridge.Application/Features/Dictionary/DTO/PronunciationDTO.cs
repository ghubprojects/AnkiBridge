using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Dictionary.DTO;

public sealed record PronunciationDTO(
    Accent Accent,
    string Ipa,
    string AudioUrl
);