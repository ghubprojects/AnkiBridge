namespace AnkiBridge.Application.Features.Dictionary.DTO;

public sealed record DefinitionDTO(
    string Text,
    IReadOnlyList<string> Examples
);