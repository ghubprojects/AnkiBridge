using AnkiBridge.Application.Features.AnkiIntegration.UseCases.CreateAnkiNotes;
using AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchAnkiNoteTypes;
using AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchDecks;
using AnkiBridge.Application.Features.AnkiIntegration.UseCases.SyncAnkiDecks;
using AnkiBridge.Application.Features.AnkiIntegration.UseCases.SyncAnkiNoteTypes;
using AnkiBridge.Shared.Extensions;
using AnkiBridge.Web.Features.Anki.Models;
using Azure.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AnkiBridge.Web.Features.Anki.Components;

public partial class CreateAnkiNotesDialog
{
    [CascadingParameter] public FluentDialog Dialog { get; set; } = default!;

    private EditContext editContext = default!;

    private AnkiNoteFormViewModel Form { get; set; } = new();

    private bool isSubmitting;

    protected override async Task OnInitializedAsync()
    {
        editContext = new EditContext(Form);

        await LoadNoteTypesAsync();
        await LoadDecksAsync();
    }

    private async Task LoadNoteTypesAsync()
    {
        await Dispatcher
            .Send(new SearchAnkiNoteTypesQuery
            {
                PageNumber = 1,
                PageSize = 100
            })
            .Match(
                async data =>
                {
                    Form.NoteTypeOptions = data.Items
                        .Select(n => new AnkiOption(n.Id, n.Name))
                        .ToList();

                    if (Form.NoteTypeOptions.Any())
                        Form.SelectedNoteType = Form.NoteTypeOptions.First();
                },
                error => DialogService.ShowErrorAsync(error.Message));
    }

    private async Task LoadDecksAsync()
    {
        await Dispatcher
            .Send(new SearchAnkiDecksQuery
            {
                PageNumber = 1,
                PageSize = 100
            })
            .Match(
                async data =>
                {
                    Form.DeckOptions = data.Items
                        .Select(d => new AnkiOption(d.Id, d.Name))
                        .ToList();

                    if (Form.DeckOptions.Any())
                        Form.SelectedDeck = Form.DeckOptions.First();
                },
                error => DialogService.ShowErrorAsync(error.Message));
    }

    private async Task SyncNoteTypesAsync()
    {
        await Dispatcher
            .Send(new SyncAnkiNoteTypesCommand())
            .Match(
                async () =>
                {
                    ToastService.ShowSuccess("The Anki note types have been synchronized successfully!");
                    await LoadNoteTypesAsync();
                },
                error => DialogService.ShowErrorAsync(error.Message));
    }

    private async Task SyncDecksAsync()
    {
        await Dispatcher
            .Send(new SyncAnkiDecksCommand())
            .Match(
                async () =>
                {
                    ToastService.ShowSuccess("The Anki decks have been synchronized successfully!");
                    await LoadDecksAsync();
                },
                error => DialogService.ShowErrorAsync(error.Message));
    }

    private async Task OnNoteTypeChangedAsync(AnkiOption option)
    {
        Form.SelectedNoteType = option;
    }

    private async Task OnDeckChangedAsync(AnkiOption option)
    {
        Form.SelectedDeck = option;
    }

    private async Task SubmitAsync()
    {
        if (!editContext.Validate())
            return;

        await Dispatcher
            .Send(new CreateAnkiNotesCommand(
                LearningEntryIds: Content.LearningEntryIds,
                NoteTypeId: Form.SelectedNoteType.Id,
                DeckId: Form.SelectedDeck.Id))
            .Match(
                async _ =>
                {
                    await Dialog.CloseAsync();
                    ToastService.ShowSuccess("Đã tạo thẻ Anki thành công!");
                },
                error => DialogService.ShowErrorAsync(error.Message));
    }

    private async Task CancelAsync() => await Dialog.CancelAsync();
}

public sealed record CreateAnkiNotesDialogContent(IReadOnlyList<Guid> LearningEntryIds);