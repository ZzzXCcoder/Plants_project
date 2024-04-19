namespace Plants.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public long? ParentId { get; set; }
        public Category? Parent { get; set; }
        public List<Category>? Children { get; set; }
    }
}
