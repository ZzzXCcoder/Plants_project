using Microsoft.AspNetCore.Mvc;
using Plants.Authorization;
using Plants.Dto.Categories;
using Plants.Dto.Plants;
using Plants.Interfaces;
using Plants.Repository;

namespace Plants.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("getCategories")]
        public async Task<ActionResult<List<CatDto>>> GetCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetCategories();
                if (categories == null)
                {
                    return NotFound();
                }
                else
                {
                    var categoriesDto = categories.ConvertToDto();
                    return Ok(categoriesDto);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ошибка получения данных из базы данных");
            }
        }

        [HttpGet]
        [Route("getCategory/{id:long}")]
        public async Task<ActionResult<CatDto>> GetCategory(long id)
        {
            try
            {
                var category = await _categoryRepository.GetCategory(id);
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    var categoryDto = category.ConverToDto();
                    return Ok(categoryDto);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ошибка получения данных из базы данных");
            }
        }

        [HttpGet]
        [Route("getPlantsByCategory/{id:long}")]
        public async Task<ActionResult<CatDto>> GetPlantsByCategory(long id)
        {
            try
            {
                var plants = await _categoryRepository.GetPlantsByCategory(id);
                if (plants == null)
                {
                    return NotFound();
                }
                else
                {
                    var plantsDto = plants.ConvertToDto();
                    return Ok(plantsDto);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ошибка получения данных из базы данных");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCategory([FromBody] CreateCatDto categoryDto)
        {
            try
            {
                var newCategory = await _categoryRepository.AddNewCategory(categoryDto);
                if (newCategory == null)
                {
                    return NoContent();
                }
                return Ok(newCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ошибка создания");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(long id, UpdateCatDto categoryDto)
        {
            try
            {
                var updateCategory = await _categoryRepository.UpdateCategory(id, categoryDto);
                if (updateCategory == null)
                {
                    return NoContent();
                }
                var response = await _categoryRepository.GetCategory(updateCategory.Id);
                var result = response.ConverToDto();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteCategory")]
        public async Task<ActionResult<Response>> DeleteCategory(long id)
        {
            try
            {
                var category = await _categoryRepository.DeleteCategory(id);
                return category;
            }
            catch (Exception ex)
            {
                return new Response { Message = ex.Message, Status = "Fail" };
            }
        }
    }

    
}
