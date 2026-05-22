using AnkiBridge.Application.Features.AnkiIntegration.UseCases.ExportAnkiNote;
using AnkiBridge.Application.Features.AnkiIntegration.UseCases.ExportAnkiNotes;
using AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchAnkiNotes;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Web.Features.Anki.Models;
using AnkiBridge.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AnkiBridge.Web.Features.Anki.Pages;

public partial class AnkiNoteList
{
    private string Headword { get; set; } = string.Empty;
    private string NoteType { get; set; } = string.Empty;
    private string Deck { get; set; } = string.Empty;
    private ExportStatus? Status { get; set; }

    private bool IsExporting { get; set; }

    private async Task HandleTabChangeAsync(FluentTab tab)
    {
        if (tab.Id is null || !Enum.TryParse<AnkiNoteExportStatus>(tab.Id, out var status))
            return;

        Status = status switch
        {
            AnkiNoteExportStatus.All => null,
            AnkiNoteExportStatus.NotStarted => ExportStatus.NotStarted,
            AnkiNoteExportStatus.Processing => ExportStatus.Processing,
            AnkiNoteExportStatus.Success => ExportStatus.Success,
            AnkiNoteExportStatus.Failed => ExportStatus.Failed,
            _ => Status
        };

        await RefreshDataAsync();
    }

    protected override async Task RefreshItemsAsync(GridItemsProviderRequest<AnkiNoteGridItemViewModel> request)
    {
        if (request.Count is null)
            return;

        IsRefreshing = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            var query = new SearchAnkiNotesQuery
            {
                Headword = Headword,
                NoteType = NoteType,
                Deck = Deck,
                Status = Status,
                PageNumber = request.StartIndex / request.Count.Value + 1,
                PageSize = request.Count.Value
            };

            var result = await Dispatcher.Send(query);

            await result.Match(
                async data =>
                {
                    GridItems = data.Items
                        .Select(x => new AnkiNoteGridItemViewModel
                        {
                            Id = x.Id,
                            LearningEntryId = x.LearningEntryId,
                            LearningEntryHeadword = x.LearningEntryHeadword,
                            NoteTypeId = x.NoteTypeId,
                            NoteTypeName = x.NoteTypeName,
                            DeckId = x.DeckId,
                            DeckName = x.DeckName,
                            ExportStatus = x.ExportStatus,
                            CreatedDate = x.CreatedAt
                        })
                        .AsQueryable();

                    await pagination.SetTotalItemCountAsync(data.TotalCount);
                },
                error =>
                {
                    DialogService.ShowError(error.Message);
                    return Task.CompletedTask;
                });
        }
        finally
        {
            IsRefreshing = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task ExportToAnkiAsync()
    {
        IsExporting = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            await Dispatcher
                .Send(new ExportAnkiNotesCommand(SelectedGridItemIds.ToList()))
                .Match(
                    async () =>
                    {
                        ToastService.ShowSuccess(
                            "The notes are being exported to Anki.",
                            10000,
                            "View",
                            EventCallback.Factory.Create<ToastResult>(this, GoToProcessingTab));
                        await RefreshDataAsync();
                    },
                    error => DialogService.ShowErrorAsync(error.Message));
        }
        finally
        {
            IsExporting = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void GoToProcessingTab()
        => Navigation.NavigateTo("/anki/notes/processing");

    public enum AnkiNoteExportStatus
    {
        All,
        NotStarted,
        Processing,
        Success,
        Failed
    }
}
