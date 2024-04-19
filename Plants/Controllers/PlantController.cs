using Microsoft.AspNetCore.Mvc;
using Plants.Authorization;
using Plants.Dto.Plants;
using Plants.Interfaces;

namespace Plants.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly IPlantRepository _plant;
        public PlantController(IPlantRepository plant)
        {
            _plant = plant;
        }

        [HttpGet]
        [Route("getPlant/{id:long}")]
        public async Task<ActionResult<PlantDto>> GetPlant(long id)
        {
            try
            {
                var plant = await _plant.GetPlant(id);
                if (plant == null)
                {
                    return BadRequest();
                }
                else
                {
                    var plantDto = plant.ConvertToDto();

                    return Ok(plantDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ошибка получения данных из базы данных");

            }
        }

        [HttpPost]
        [Route("addNewPlant")]
        public async Task<IActionResult> AddNewPlant([FromBody] CreatePlantDto plantDto)
        {
            try
            {

                var newpPlant = await _plant.AddNewPlant(plantDto);

                if (newpPlant == null)
                {
                    return NoContent();
                }
                return Ok(newpPlant.ConvertToDto());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка создания");
            }
        }

        [HttpPut]
        [Route("updatePlant/{id:long}")]
        public async Task<IActionResult> UpdatePlant(long id, UpdatePlantDto plantDto)
        {
            try
            {
                var plant = await _plant.UpdatePlant(id, plantDto);
                if (plant == null)
                {
                    return NotFound();
                }

                var responce = await _plant.GetPlant(plant.Id);
                var result = responce.ConvertToDto();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePlant")]
        public async Task<ActionResult<Response>> DeletePlant(long id)
        {
            try
            {
                var category = await _plant.DeletePlant(id);
                return category;
            }
            catch (Exception ex)
            {
                return new Response { Message = ex.Message, Status = "Fail" };
            }
        }
    }
}
