using Microsoft.EntityFrameworkCore;
using Plants.Authorization;
using Plants.Data;
using Plants.Dto.Categories;
using Plants.Interfaces;
using Plants.Models;

namespace Plants.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        private async Task<bool> CategoryExist(string catName)
        {
            return await _context.Categories.AnyAsync(c => c.Name == catName);
        }
        private async Task<bool> CategoryExist(long? catId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == catId);
        }
        public async Task<Category> AddNewCategory(CreateCatDto createCatDto)
        {
            if (await CategoryExist(createCatDto.Name) == false)
            {
                if (createCatDto.ParentId != 0 && await CategoryExist(createCatDto.ParentId) == false)
                    return null;
                Category category = new Category
                {
                    Name = createCatDto.Name,
                    ImageUrl = createCatDto.Image,
                    ParentId = createCatDto.ParentId,
                };
                if (createCatDto.ParentId == 0)
                    category.ParentId = null;
                if (category != null)
                {
                    var result = await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<Response> DeleteCategory(long id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return new Response { Message = "Категория успешно удаленa", Status = "Success" };
            }
            return new Response { Message = "Ошибка удаления", Status = "Fail" };
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoriesParent = categories.Where(c => c.ParentId == null).ToList();
            List<Category> result = new List<Category>();
            foreach (var category in categoriesParent)
            {
                var response = await GetCategory(category.Id);
                result.Add(response);
            }
            return result;
        }

        public async Task<Category> GetCategory(long id)
        {
            if (await CategoryExist(id))
            {
                var category = await _context.Categories.FindAsync(id);
                category.Children = await _context.Categories.Where(c => c.ParentId == category.Id).ToListAsync();
                return category;
            }
            return null;
        }

        public async Task<List<Plant>> GetPlantsByCategory(long catid)
        {
            var plants = await _context.Plants.Include( p => p.Category).Where(p => p.CategoryId == catid).ToListAsync();
            List<Plant> result = new List<Plant>();
            foreach(var plant in plants)
            {
                plant.Category = await _context.Categories.FindAsync(plant.CategoryId);
                result.Add(plant);
            }
            return result;
        }

        public async Task<Category> UpdateCategory(long id, UpdateCatDto updateCatDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.Name = updateCatDto.Name;
                category.ImageUrl = updateCatDto.Image;
                await _context.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
