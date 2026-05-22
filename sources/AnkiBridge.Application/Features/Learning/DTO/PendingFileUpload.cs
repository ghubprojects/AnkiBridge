namespace AnkiBridge.Application.Features.Learning.DTO;

public sealed record PendingFileUpload(
    string? FileName,
    string? ContentType,
    string StagedPath,
    string DestinationPath,
    FileUploadKind Kind);
