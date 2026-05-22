using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SyncAnkiNoteTypes;

public sealed record SyncAnkiNoteTypesCommand : IRequest<Result>;
