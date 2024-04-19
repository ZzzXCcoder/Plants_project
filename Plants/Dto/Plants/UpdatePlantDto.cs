namespace Plants.Dto.Plants
{
    public class UpdatePlantDto
    {
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public long WateringTime { get; set; }
    }
}
