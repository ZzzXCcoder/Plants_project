namespace Plants.Dto.Categories
{
    public class CatDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public long? ParentId { get; set; }
        public List<CatDto>? Children { get; set; }
    }
}
