using LexiBridge.Application.Features.Learning.UseCases.SearchLearningItems;
using LexiBridge.Shared.Extensions;
using LexiBridge.Web.Features.Learning.Models;
using MediatR;
using Humanizer;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using LexiBridge.Web.Features.Learning.Helpers;

namespace LexiBridge.Web.Features.Learning.Pages;

public partial class LearningItemList : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected ISender Sender { get; set; } = default!;

    private FluentDataGrid<LearningItemGridItemViewModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = SelectOptions.DefaultPageSize };
    private readonly IEnumerable<Option<int>> itemsPerPageOptions =
        SelectOptions.AllowedPageSizes
            .Select(x => new Option<int>
            {
                Value = x
            });

    private IQueryable<LearningItemGridItemViewModel> GridItems { get; set; } = Enumerable.Empty<LearningItemGridItemViewModel>().AsQueryable();
    private HashSet<Guid> SelectedGridItemIds { get; set; } = [];

    private bool IsRefreshing { get; set; }
    private string Keyword { get; set; } = string.Empty;

    private bool IsSingleItemSelected => SelectedGridItemIds.Count == 1;
    private bool HasAnyItemSelected => SelectedGridItemIds.Count > 0;

    private async Task HandleTabChangeAsync(FluentTab tab)
    {
        if (tab.Id is null || !Enum.TryParse<LearningItemStatus>(tab.Id, out var status))
            return;

        switch (status)
        {
            case LearningItemStatus.All:
                await RefreshDataAsync();
                break;
            case LearningItemStatus.NotStarted:
            case LearningItemStatus.Success:
            case LearningItemStatus.Failed:
                break;
        }
    }

    private async Task RefreshItemsAsync(GridItemsProviderRequest<LearningItemGridItemViewModel> request)
    {
        if (request.Count is null)
            return;

        IsRefreshing = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            var query = new SearchLearningItemsQuery
            {
                Keyword = Keyword,
                PageNumber = request.StartIndex / request.Count.Value + 1,
                PageSize = request.Count.Value
            };

            var result = await Sender.Send(query);

            await result.Match(
                async data =>
                {
                    GridItems = data.Items
                        .Select(x => new LearningItemGridItemViewModel
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

    private Task RefreshDataAsync()
      => dataGrid.RefreshDataAsync(true);

    private void HandleSelectItem((LearningItemGridItemViewModel Item, bool Selected) e)
    {
        if (IsRefreshing)
            return;

        if (e.Selected)
            SelectedGridItemIds.Add(e.Item.Id);
        else
            SelectedGridItemIds.Remove(e.Item.Id);
    }

    private void HandleSelectAllChanged(bool? isAllSelected)
    {
        if (isAllSelected == true)
        {
            foreach (var item in GridItems)
                SelectedGridItemIds.Add(item.Id);
        }
        else
        {
            foreach (var item in GridItems)
                SelectedGridItemIds.Remove(item.Id);
        }
    }

    private void GoToCreatePage()
        => Navigation.NavigateTo("/learning-items/create");

    public enum LearningItemStatus
    {
        All,
        NotStarted,
        Success,
        Failed
    }
}
