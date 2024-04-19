using Plants.Authorization;
using Plants.Dto.Plants;
using Plants.Models;

namespace Plants.Interfaces
{
    public interface IPlantRepository
    {
        Task<Plant> GetPlant(long id);
        public Task<Plant> AddNewPlant(CreatePlantDto plantDto);
        public Task<Plant> UpdatePlant(long id, UpdatePlantDto plantDto);
        public Task<Response> DeletePlant(long id);
    }
}
