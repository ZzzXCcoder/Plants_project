namespace Plants.Dto.Plants
{
    public class CreatePlantDto
    {
        public long Id {get; set;}
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public long WateringTime { get; set; }
        public long CategoryId {get; set;}
    }
}
