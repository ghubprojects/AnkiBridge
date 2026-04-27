using Humanizer;
using LexiBridge.Application.Features.Learning.UseCases.CreateLearningItem;
using LexiBridge.Domain.Enums;
using LexiBridge.Web.Features.Learning.Helpers;
using LexiBridge.Web.Features.Learning.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace LexiBridge.Web.Features.Learning.Pages;

public partial class LearningItemCreate : ComponentBase
{
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected IToastService ToastService { get; set; } = default!;

    [Inject]
    protected ISender Sender { get; set; } = default!;

    private EditContext editContext = default!;

    private LearningItemDetailViewModel Detail { get; set; } = new();

    private bool IsLookingUp { get; set; }
    private bool IsSubmitting { get; set; }

    private readonly IEnumerable<Option<PartOfSpeech>> partOfSpeechOptions =
        SelectOptions.OrderedPartsOfSpeech.Select(x => new Option<PartOfSpeech>
        {
            Value = x,
            Text = x.ToString().Humanize()
        });

    private readonly IEnumerable<Option<Accent>> accentOptions =
        SelectOptions.OrderedAccents.Select(x => new Option<Accent>
        {
            Value = x,
            Text = SelectOptions.AccentDisplayLabels.TryGetValue(x, out var label)
                ? label
                : x.ToString().Humanize()
        });

    protected override void OnInitialized()
    {
        editContext = new EditContext(Detail);
    }

    private void HandleSelectPartOfSpeech(string? value)
    {
        if (Enum.TryParse<PartOfSpeech>(value, out var result))
            Detail.PartOfSpeech = result;
    }

    private void HandleSelectAccent(string? value)
    {
        if (Enum.TryParse<Accent>(value, out var result))
            Detail.Accent = result;
    }

    private void AddExample()
    {
        if (Detail.Examples.Count >= 3)
            return;

        Detail.Examples.Add(string.Empty);
    }

    private void RemoveExample(int index)
    {
        if (index >= 0 && index < Detail.Examples.Count)
            Detail.Examples.RemoveAt(index);
    }

    private async Task SubmitAsync()
    {
        if (!editContext.Validate())
            return;

        IsSubmitting = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            var command = new CreateLearningItemCommand(
                Headword: Detail.Headword,
                PartOfSpeech: Detail.PartOfSpeech,
                Ipa: Detail.Ipa,
                Accent: Detail.Accent,
                Cloze: Detail.Cloze,
                Definition: Detail.Definition,
                Translation: Detail.Translation,
                Examples: Detail.Examples,

                AudioStream: audioFile?.OpenReadStream(10 * 1024 * 1024),
                AudioFileName: audioFile?.Name,
                AudioContentType: audioFile?.ContentType,

                ImageStream: imageFile?.OpenReadStream(10 * 1024 * 1024),
                ImageFileName: imageFile?.Name,
                ImageContentType: imageFile?.ContentType,
                DictionaryEntryId: null
            );

            var result = await Sender.Send(command);

            await result.Match(
                async _ =>
                {
                    ToastService.ShowSuccess("Learning item created successfully.");
                    ClearForm();
                },
                error => DialogService.ShowErrorAsync(error.Message));
        }
        finally
        {
            IsSubmitting = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void ClearForm()
    {
        Detail = new LearningItemDetailViewModel();
        editContext = new EditContext(Detail);
    }

    private async Task LookupAsync()
    {

    }

    private async Task ChangeHeadwordAsync()
    {
    }

    private async Task ChangeDefinitionAsync()
    {
    }

    private IBrowserFile? audioFile;
    private IBrowserFile? imageFile;

    private string? audioPreviewUrl;
    private string? imagePreviewUrl;

    private async Task HandleChangeAudioAsync(InputFileChangeEventArgs e)
    {
        audioFile = e.File;

        using var stream = audioFile.OpenReadStream(10 * 1024 * 1024);
        using var ms = new MemoryStream();

        await stream.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var base64 = Convert.ToBase64String(bytes);
        audioPreviewUrl = $"data:{audioFile.ContentType};base64,{base64}";
    }

    private async Task HandleChangeImageAsync(InputFileChangeEventArgs e)
    {
        imageFile = e.File;

        using var stream = imageFile.OpenReadStream(10 * 1024 * 1024);
        using var ms = new MemoryStream();

        await stream.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var base64 = Convert.ToBase64String(bytes);
        imagePreviewUrl = $"data:{imageFile.ContentType};base64,{base64}";
    }

    private void ClearAudio()
    {
        audioFile = null;
        audioPreviewUrl = null;
    }

    private void ClearImage()
    {
        imageFile = null;
        imagePreviewUrl = null;
    }
}
