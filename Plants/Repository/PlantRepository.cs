using Microsoft.EntityFrameworkCore;
using Plants.Authorization;
using Plants.Data;
using Plants.Dto.Plants;
using Plants.Interfaces;
using Plants.Models;

namespace Plants.Repository
{
    public class PlantRepository : IPlantRepository
    {
        private readonly DataContext _context;
        public PlantRepository(DataContext context)
        {
            _context = context;
        }

        private async Task<bool> PlantExist(string plantName)
        {
            return await _context.Plants.AnyAsync( p => p.Name == plantName );
        }
        private async Task<bool> PlantExist(long plantId)
        {
            return await _context.Plants.AnyAsync(p => p.Id == plantId);
        }

        public async Task<Plant> AddNewPlant(CreatePlantDto plantDto)
        {
            if(!await PlantExist(plantDto.Name))
            {
                Plant plant = new()
                {
                    Id = plantDto.Id,
                    Name = plantDto.Name,
                    Image = plantDto.Image,
                    Description = plantDto.Description,
                    WateringTime = plantDto.WateringTime,
                    CategoryId = plantDto.CategoryId,
                    

                };
                
                if (plant != null )
                {
                    plant.Category = await _context.Categories.Where(p => p.Id == plantDto.CategoryId).SingleAsync();
                    var result = await _context.Plants.AddAsync(plant);
                    await _context.SaveChangesAsync();
                    return plant;
                }
            }
            return default;
        }

        public async Task<Response> DeletePlant(long id)
        {
            Plant plant = await _context.Plants.FindAsync(id);
            if(plant != null )
            {
                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();
                return new Response { Message = "Растение успешно удалено из бд", Status = "Success" };
            }
            return new Response { Message = "Ошибка удаления", Status = "Fail" };
        }

        public async Task<Plant> GetPlant(long id)
        {
            if( await PlantExist(id))
            {
                Plant plant = await _context.Plants.FindAsync(id);
                plant.Category = await _context.Categories.FindAsync(plant.CategoryId);
                return plant;
            }
            return default;
        }

        public async Task<Plant> UpdatePlant(long id, UpdatePlantDto plantDto)
        {
            if(await PlantExist(id))
            {
                Plant plant = await _context.Plants.FindAsync(id);
                plant.Name = plantDto.Name;
                plant.Image = plantDto.Image;
                plant.Description = plantDto.Description;
                plant.WateringTime = plantDto.WateringTime;
                await _context.SaveChangesAsync();
                return plant;
            }
            return default;
        }
    }
}
