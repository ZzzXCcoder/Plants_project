using Plants.Models;

namespace Plants.Dto.UserPlants
{
    public static class Convert
    {
        public static UserPlantDto ConvertToDto(this UserPlant userPlant, Plant plant)
        {
            return new UserPlantDto
            {
                Id = userPlant.Id,
                PlantName = plant.Name,
                PlantDescription = plant.Description,
                PlantImage = plant.Image,
                LastWateringTime = userPlant.LastWateringTime,
                NextWateringTime = userPlant.LastWateringTime + plant.WateringTime
            };
        }
        public static List<UserPlantDto> ConvertToDto(this List<UserPlant> userPlants, List<Plant> plants)
        {
            return (from userPlant in userPlants
                    join plant in plants
                    on userPlant.PlantId equals plant.Id
                    select new UserPlantDto
                    {
                        Id = userPlant.Id,
                        PlantName = plant.Name,
                        PlantDescription = plant.Description,
                        PlantImage = plant.Image,
                        LastWateringTime = userPlant.LastWateringTime,
                        NextWateringTime = userPlant.LastWateringTime + plant.WateringTime
                    }).ToList();
        }
    }

    
}
