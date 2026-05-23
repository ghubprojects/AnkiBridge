using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SyncDecks;

public sealed class SyncDecksCommand : IRequest<Result>;
