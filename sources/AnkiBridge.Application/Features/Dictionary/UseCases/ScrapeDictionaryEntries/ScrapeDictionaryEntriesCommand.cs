using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.UseCases.ScrapeDictionaryEntries;

public sealed record ScrapeDictionaryEntriesCommand(string Headword) : IRequest<Result>;
