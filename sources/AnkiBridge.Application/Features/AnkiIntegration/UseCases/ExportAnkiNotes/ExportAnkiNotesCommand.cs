using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.ExportAnkiNotes;

public sealed record ExportAnkiNotesCommand(
    IReadOnlyList<Guid> Ids
) : IRequest<Result>;