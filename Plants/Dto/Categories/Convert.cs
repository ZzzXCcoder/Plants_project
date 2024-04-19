using Plants.Models;

namespace Plants.Dto.Categories
{
    public static class Convert
    {
        public static List<CatDto> ConvertToDto(this List<Category> categories)
        {
            if (categories != null)
            {
                return (from category in categories
                        select new CatDto
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Image = category.ImageUrl,
                            ParentId = category.ParentId,
                            Children = ConvertToDto(category.Children)
                        }).ToList();
            }
            return null;
        }

        public static CatDto ConverToDto(this Category category)
        {
            return new CatDto
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.ImageUrl,
                ParentId = category.ParentId,
                Children = ConvertToDto(category.Children)
            };
        }
    }
}
