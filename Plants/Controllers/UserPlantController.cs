using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Plants.Dto.UserPlants;
using Plants.EmailSender;
using Plants.Interfaces;
using Plants.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Plants.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPlantController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserPlantRepository _repository;
        private readonly IPlantRepository _plantRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public UserPlantController(IUserPlantRepository repository, IPlantRepository plantRepository, IEmailService emailService, UserManager<User> userManager, IConfiguration configuration)
        {
            _repository = repository;
            _plantRepository = plantRepository;
            _emailService = emailService;
            _userManager = userManager;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("{token}/GetUserPlants")]
        public async Task<ActionResult<List<UserPlantDto>>> GetPlants(string token)
        {
            try
            {
                string userId = await GetId(token);
                var cartItems = await _repository.GetPlants(userId);

                if (cartItems == null)
                {
                    return NoContent();
                }
                List<Plant> plants = new List<Plant>();
                Plant plant = new Plant();
                foreach (var cartItem in cartItems)
                {
                    
                    if(plant != null && plant.Id != cartItem.PlantId)
                    {
                        plant = await _plantRepository.GetPlant(cartItem.PlantId);
                        plants.Add(plant);
                    }
                        
                }
                List< UserPlantDto> cartItemsDto = cartItems.ConvertToDto(plants);
                List<UserPlantDto> result = cartItemsDto.Distinct().ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UserPlantDto>> PostItem( long plantId, string token)
        {
            try
            {
                string userId = await GetId(token);
                var newCartItem = await _repository.AddPlant(plantId, userId);

                if (newCartItem == null)
                {
                    return default(UserPlantDto);
                }

                var plant = await _plantRepository.GetPlant(newCartItem.PlantId);

                if (plant == null)
                {
                    throw new Exception($"Something went wrong when attempting to retrieve plant (plantId:({plantId})");
                }

                var newCartItemDto = newCartItem.ConvertToDto(plant);
                User user = await _userManager.FindByIdAsync(newCartItem.UserId);
                //var message = new Message(new string[] {$"{user.Email}" }, "Куколд мафия", $" Дорогой {user.Email}, вы добавили {newCartItemDto.PlantName} в свою коллекцию. Не забывай поливать, КУКОЛД!");
                //await _emailService.SendEmail(message);
                return newCartItemDto;


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UserPlantDto>> DeletePlant(long id)
        {
            try
            {
                var cartItem = await _repository.DeletePlant(id);

                if (cartItem == null)
                {
                    return NotFound();
                }

                var plant = await _plantRepository.GetPlant(cartItem.PlantId);

                if (plant == null)
                    return NotFound();

                var cartItemDto = cartItem.ConvertToDto(plant);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UserPlantDto>> UpdateQty(long id)
        {
            try
            {
                var cartItem = await _repository.UpdateWateringTime(id);
                if (cartItem == null)
                {
                    return NotFound();
                }

                var product = await _plantRepository.GetPlant(cartItem.PlantId);

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        private async Task<string> GetId( string token)
        {
            var secret = _configuration["JWTAuth:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims.Identity.Name;
        }


    }
}
