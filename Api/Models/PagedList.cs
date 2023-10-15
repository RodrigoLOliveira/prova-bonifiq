namespace ProvaPub.Models
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}
