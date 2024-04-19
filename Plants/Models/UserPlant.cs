namespace Plants.Models
{
    public class UserPlant
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long PlantId { get; set; }
        public long LastWateringTime { get; set; }
    }
}
