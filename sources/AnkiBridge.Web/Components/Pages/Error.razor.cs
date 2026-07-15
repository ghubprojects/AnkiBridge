using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace AnkiBridge.Web.Components.Pages;

public partial class Error
{
    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    private string? requestId;
    private bool ShowRequestId => !string.IsNullOrEmpty(requestId);

    protected override void OnInitialized()
    {
        requestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}
