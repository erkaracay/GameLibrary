using Business.Models;
using DataAccess.Contexts;

namespace Business.Services;

public interface ICategoryService
{
    IQueryable<CategoryModel> Query();
}

public class CategoryService : ICategoryService
{
    Db _db;

    public CategoryService(Db db)
	{
        _db = db;
	}

    public IQueryable<CategoryModel> Query()
    {
        return _db.Categories.OrderByDescending(e => e.Name)
            .Select(e => new CategoryModel()
            {
                Id = e.Id,
                Name = e.Name
            });
    }
}