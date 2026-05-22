using AnkiBridge.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace AnkiBridge.Web.Features.Anki.Components;

public partial class ExportStatusBadge
{
    [Parameter]
    public ExportStatus Status { get; set; }

    private string? BackgroundColor => Status switch
    {
        ExportStatus.NotStarted => "#adadad",
        ExportStatus.Processing => "#eaa300",
        ExportStatus.Success => "#13a10e",
        ExportStatus.Failed => "#d13438",
        ExportStatus.Cancelled => "#616161",
        _ => null
    };
}
