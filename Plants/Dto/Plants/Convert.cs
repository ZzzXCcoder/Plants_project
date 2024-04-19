using Plants.Models;

namespace Plants.Dto.Plants
{
    public static class Convert
    {
        public static PlantDto ConvertToDto(this Plant plant)
        {
            return new PlantDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Image = plant.Image,
                Description = plant.Description,
                WateringTime = plant.WateringTime,
                CategoryId = plant.Category.Id,
                CategoryName = plant.Category.Name,
            };
        }

        public static List<PlantDto> ConvertToDto(this List<Plant> plants)
        {
            if(plants != null)
            {
                return (from plant in plants
                        select new PlantDto
                        {
                            Id = plant.Id,
                            Name = plant.Name,
                            Image = plant.Image,
                            Description = plant.Description,
                            WateringTime = plant.WateringTime,
                            CategoryId = plant.Category.Id,
                            CategoryName = plant.Category.Name,
                        }).ToList();
            }
            return default;
        }
    }
}
