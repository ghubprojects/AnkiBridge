using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AnkiBridge.Web.Components.Abstractions;

public abstract class DialogComponentBase : RootComponentBase, IDialogContentComponent
{
}

public abstract class DialogComponentBase<TContent> : RootComponentBase, IDialogContentComponent<TContent>
    where TContent : notnull
{
    [Parameter]
    public virtual TContent Content { get; set; } = default!;
}
