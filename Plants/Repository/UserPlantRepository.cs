using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plants.Data;
using Plants.Dto.UserPlants;
using Plants.EmailSender;
using Plants.Interfaces;
using Plants.Models;

namespace Plants.Repository
{
    public class UserPlantRepository : IUserPlantRepository
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _manager;

        public UserPlantRepository(DataContext context, UserManager<User> manager, IEmailService emailService)
        {
            _context = context;
            _manager = manager;
            _emailService = emailService;
        }

        public async Task<UserPlant> AddPlant(long plantId, string userId)
        {
            var userPlant = await (from plant in _context.Plants
                                   where plant.Id == plantId
                                   select new UserPlant
                                   {
                                       PlantId = plant.Id,
                                       UserId = userId,

                                   }).SingleOrDefaultAsync();
            if(userPlant != null)
            {
                userPlant.LastWateringTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                var result = await _context.UserPlants.AddAsync(userPlant);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            return default;
        }

        public async Task<UserPlant> DeletePlant(long id)
        {
            var userPlant = await _context.UserPlants.FindAsync(id);
            if(userPlant != null)
            {
                _context.UserPlants.Remove(userPlant);
                await _context.SaveChangesAsync();
            }
            return userPlant;
        }

        //public Task<UserPlant> GetItem(long id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<UserPlant>> GetPlants(string userId)
        {
            var user = await _manager.FindByIdAsync(userId);
            if(user != null)
            {
                List<UserPlant> userPlants = await _context.UserPlants.Where(p => p.UserId == userId).ToListAsync();
                
                return userPlants;
            }
            return default;
        }

        public async Task SendMail()
        {
            var currentTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var userPlants = await _context.UserPlants.Where(t => t.LastWateringTime < currentTime).ToListAsync();
            
            foreach (var userPlant in userPlants)
            {
                User user = await _manager.FindByIdAsync(userPlant.UserId);
                Plant plant = await _context.Plants.FindAsync(userPlant.PlantId);
                var message = new Message(new string[] { $"{user.Email}" }, "Служба спасения растений", $" Дорогой {user.Email}, ваше растение: {plant.Name} ждет своего полива 👉👈");
                long time = currentTime - userPlant.LastWateringTime;
                if ((time) < 300)
                {
                    await _emailService.SendEmail(message);
                }
                if ((time) >  86400 && (time) < 86700)
                {
                    await _emailService.SendEmail(message);
                }
                if ((time) > 172800 && time < 173100)
                {
                    await _emailService.SendEmail(message);
                }
            }
        }

        public async Task<UserPlant> UpdateWateringTime(long id)
        {
            var updateUP = await _context.UserPlants.FindAsync(id);
            if(updateUP != null)
            {
                updateUP.LastWateringTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                await _context.SaveChangesAsync();
                return updateUP;
            }
            return default;
        }
    }
}
