using System.Text.Json.Serialization;

namespace GoalTracker.UI.Blazor.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("totalItemsCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("itemsFrom")]
        public int ItemsFrom { get; set; }

        [JsonPropertyName("itemsTo")]
        public int ItemsTo { get; set; }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        // Calculated properties for convenience
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
