namespace Plants.Dto.Categories
{
    public class CreateCatDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public long? ParentId { get; set; }
    }
}
