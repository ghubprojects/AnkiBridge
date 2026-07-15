using AnkiBridge.Application.Features.Learning.Queries.GetLearningEntries;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace AnkiBridge.Web.Components.Pages.Learning;

public partial class LearningItems
{
    [Inject]
    private IMediator Mediator { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<LearningEntryListItem> _entries = [];
    private bool _isLoading = true;
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _entries = (await Mediator.Send(new GetLearningEntriesQuery())).ToList();
        }
        catch (Exception)
        {
            _errorMessage = "Your learning library could not be loaded. Check the database connection and try again.";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void CreateNew() => Navigation.NavigateTo("/learning-items/new");
}
