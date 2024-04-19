using Plants.Authorization;
using Plants.Dto.Categories;
using Plants.Models;

namespace Plants.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(long id);
        Task<List<Plant>> GetPlantsByCategory(long catid);
        public Task<Category> AddNewCategory(CreateCatDto createCatDto);
        public Task<Category> UpdateCategory(long id, UpdateCatDto updateCatDto);
        public Task<Response> DeleteCategory(long id);
    }
}
