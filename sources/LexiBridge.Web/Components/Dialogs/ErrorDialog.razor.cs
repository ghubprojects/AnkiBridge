using Microsoft.AspNetCore.Components;

namespace LexiBridge.Web.Components.Dialogs;

public partial class ErrorDialog
{
    [Inject] 
    protected NavigationManager Navigation { get; private set; } = default!;

    [Inject] 
    protected ILogger<ErrorDialog> Logger { get; private set; } = default!;

    [EditorRequired]
    [Parameter] 
    public Exception Exception { get; set; } = default!;

    private string? Message { get; set; }

    protected override void OnInitialized()
    {
        switch (Exception)
        {
            case Exception ex:
                Message = ex.Message;

                break;
            default:
                if (Exception.InnerException != null)
                    while (Exception.InnerException != null)
                        Exception = Exception.InnerException;

                Message = Exception.Message;
                break;
        }
    }

    private void HandleRefresh()
        => Navigation.NavigateTo(Navigation.Uri, true);
}
