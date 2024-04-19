using System.ComponentModel.DataAnnotations.Schema;

namespace Plants.Models
{
    public class Plant
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public long WateringTime { get; set; }
        public long CategoryId { get; set; }
        [ForeignKey("СategoryId")]
        public virtual Category Category { get; set; }
    }
}
