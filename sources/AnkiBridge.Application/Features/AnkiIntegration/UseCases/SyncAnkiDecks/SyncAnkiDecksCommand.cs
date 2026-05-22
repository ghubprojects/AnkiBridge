using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SyncAnkiDecks;

public sealed class SyncAnkiDecksCommand : IRequest<Result>;
