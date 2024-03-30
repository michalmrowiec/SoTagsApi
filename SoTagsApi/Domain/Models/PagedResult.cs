namespace SoTagsApi.Domain.Models
{
    public class PagedResult<T> where T : class
    {
        public IList<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalPages { get; set; }

        public PagedResult()
        { }
        public PagedResult(IList<T> items, int totalItems, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
        }
    }
}
