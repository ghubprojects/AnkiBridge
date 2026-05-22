using Microsoft.AspNetCore.Components.Web;

namespace AnkiBridge.Web.Components.Layout;

public partial class MainLayout
{
    private ErrorBoundary? errorBoundary = default!;

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }
}