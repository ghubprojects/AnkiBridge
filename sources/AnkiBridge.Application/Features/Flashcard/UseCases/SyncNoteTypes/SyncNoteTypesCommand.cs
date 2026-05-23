using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SyncNoteTypes;

public sealed record SyncNoteTypesCommand : IRequest<Result>;
