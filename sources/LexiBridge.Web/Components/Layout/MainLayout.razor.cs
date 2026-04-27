using Microsoft.AspNetCore.Components.Web;

namespace LexiBridge.Web.Components.Layout;

public partial class MainLayout
{
    private ErrorBoundary? errorBoundary = default!;

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }
}