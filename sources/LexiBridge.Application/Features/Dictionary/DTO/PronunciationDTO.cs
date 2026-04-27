using LexiBridge.Domain.Enums;

namespace LexiBridge.Application.Features.Dictionary.DTO;

public sealed record PronunciationDTO(
    Accent Accent,
    string Ipa,
    string AudioUrl
);