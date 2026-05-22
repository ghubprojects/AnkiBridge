using AnkiBridge.Web.Features.Learning.Helpers;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AnkiBridge.Web.Components.Abstractions;

public abstract class GridPageComponentBase<TGridItem, TKey> : RootComponentBase
    where TGridItem : IGridItem<TKey> 
    where TKey : notnull
{
    protected FluentDataGrid<TGridItem> dataGrid = default!;

    protected readonly PaginationState pagination = new()
    {
        ItemsPerPage = SelectOptions.DefaultPageSize
    };

    protected readonly IEnumerable<Option<int>> itemsPerPageOptions =
        SelectOptions.AllowedPageSizes
            .Select(x => new Option<int>
            {
                Value = x
            });

    protected IQueryable<TGridItem> GridItems { get; set; } = Enumerable.Empty<TGridItem>().AsQueryable();
    protected HashSet<TKey> SelectedGridItemIds { get; set; } = [];

    protected bool IsSingleItemSelected => SelectedGridItemIds.Count == 1;
    protected bool HasAnyItemSelected => SelectedGridItemIds.Count > 0;

    protected bool IsRefreshing { get; set; }

    protected abstract Task RefreshItemsAsync(GridItemsProviderRequest<TGridItem> request);

    protected Task RefreshDataAsync()
      => dataGrid.RefreshDataAsync(true);

    protected void HandleSelectItem((TGridItem Item, bool Selected) e)
    {
        if (IsRefreshing)
            return;

        if (e.Selected)
            SelectedGridItemIds.Add(e.Item.Id);
        else
            SelectedGridItemIds.Remove(e.Item.Id);
    }

    protected void HandleSelectAllChanged(bool? isAllSelected)
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
}