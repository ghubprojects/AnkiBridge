using AnkiBridge.Application.Features.Learning.UseCases.SearchLearningEntries;
using AnkiBridge.Shared.Extensions;
using AnkiBridge.Web.Features.Anki.Components;
using AnkiBridge.Web.Features.Learning.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AnkiBridge.Web.Features.Learning.Pages;

public partial class LearningEntryList
{
    private string Keyword { get; set; } = string.Empty;

    protected override async Task RefreshItemsAsync(GridItemsProviderRequest<LearningEntryGridItemViewModel> request)
    {
        if (request.Count is null)
            return;

        IsRefreshing = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            await Dispatcher
                .Send(new SearchLearningEntriesQuery
                {
                    Keyword = Keyword,
                    PageNumber = request.StartIndex / request.Count.Value + 1,
                    PageSize = request.Count.Value
                })
                .Match(
                    async data =>
                    {
                        GridItems = data.Items
                            .Select(x => new LearningEntryGridItemViewModel
                            {
                                Id = x.Id,
                                Headword = x.Headword,
                                PartOfSpeech = x.PartOfSpeech.ToString().ToLower(),
                                Ipa = x.Ipa.WrapWithSlashes(),
                                Translation = x.Translation,
                                CreatedDate = x.CreatedAt
                            })
                            .AsQueryable();

                        await pagination.SetTotalItemCountAsync(data.TotalCount);
                    },
                    error => DialogService.ShowErrorAsync(error.Message));
        }
        finally
        {
            IsRefreshing = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void GoToCreatePage()
        => Navigation.NavigateTo("/learning-entries/create");

    private async Task ShowCreateNotesDialogAsync()
    {
        var dialogContent = new CreateAnkiNotesDialogContent(SelectedGridItemIds.ToList());
        var dialogParams = new DialogParameters
        {
            Title = "Create Anki notes",
            Width = "800px",
            PreventDismissOnOverlayClick = true,
        };

        var dialog = await DialogService.ShowDialogAsync<CreateAnkiNotesDialog>(dialogContent, dialogParams);
        var dialogResult = await dialog.Result;

        if (!dialogResult.Cancelled && dialogResult.Data is not null)
            ToastService.ShowSuccess(
                "The Anki notes are being created.",
                10000,
                "View",
                EventCallback.Factory.Create<ToastResult>(this, GoToAnkiNotesPage));
    }

    private void GoToAnkiNotesPage()
        => Navigation.NavigateTo("/anki-notes");
}
