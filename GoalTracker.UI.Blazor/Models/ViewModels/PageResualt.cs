namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemsCount { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}
