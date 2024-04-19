using Plants.Dto.UserPlants;
using Plants.Models;

namespace Plants.Interfaces
{
    public interface IUserPlantRepository
    {
        Task<UserPlant> AddPlant(long plantId, string userId);
        Task<UserPlant> UpdateWateringTime(long id);
        Task<UserPlant> DeletePlant(long id);
        //Task<UserPlant> GetItem(long id);
        Task<List<UserPlant>> GetPlants(string userId);
        Task SendMail();
    }
}
