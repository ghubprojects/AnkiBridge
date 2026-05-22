using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.ExportAnkiNote;

public sealed record ExportAnkiNoteCommand(
    Guid Id
) : IRequest<Result>;