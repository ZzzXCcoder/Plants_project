namespace Plants.Dto.UserPlants
{
    public class UserPlantDto
    {
        public long Id { get; set; }
        public string PlantName { get; set; }
        public string PlantDescription { get; set; }
        public string PlantImage { get; set; }
        public long LastWateringTime { get; set; }
        public long NextWateringTime { get; set; }
    }
}
