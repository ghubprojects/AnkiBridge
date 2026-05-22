namespace AnkiBridge.Web.Components.Abstractions;

public interface IGridItem<TKey>
{
    TKey Id { get; init; }
}