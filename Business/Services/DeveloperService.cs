using Business.Models;
using Business.Results;
using Business.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services;

public interface IDeveloperService
{
    IQueryable<DeveloperModel> Query();
    Result Add(DeveloperModel model);
    Result Update(DeveloperModel model);
    Result Delete(int id);

}

public class DeveloperService : IDeveloperService
{
	Db _db;

	public DeveloperService(Db db)
	{
		_db = db;
	}

    public IQueryable<DeveloperModel> Query()
    {
        return _db.Developers.OrderByDescending(e => e.Name)
           .Select(e => new DeveloperModel()
           {
               Id = e.Id,
               Name = e.Name,
               FoundingDate = e.FoundingDate,
               FoundingDateOutput = e.FoundingDate.Date.ToString("dd MMM yyyy") ?? ""
           });
    }

    public Result Add(DeveloperModel model)
    {
        if (_db.Developers.Any(s => s.Name.ToUpper() == model.Name.ToUpper().Trim()))
            return new ErrorResult("Developer with the same name already exists!");

        Developer entity = new Developer()
        {
            Id = model.Id,
            Name = model.Name,
            FoundingDate = model.FoundingDate
        };

        _db.Developers.Add(entity);
        _db.SaveChanges();
        return new SuccessResult("Developer added successfully");
    }

    public Result Delete(int id)
    {
        Developer entity = _db.Developers.SingleOrDefault(d => d.Id == id);
        if (entity is null)
            return new ErrorResult("Developer not found!");
        _db.Developers.Remove(entity);
        _db.SaveChanges();
        return new SuccessResult("Developer deleted successfully.");
    }


    public Result Update(DeveloperModel model)
    {
        if(_db.Developers.Any(s => s.Name.ToUpper() == model.Name.ToUpper().Trim() && s.Id != model.Id))
            return new ErrorResult("Developer with the same name already exists!");
        Developer existingEntity = _db.Developers.SingleOrDefault(s => s.Id == model.Id);
        if (existingEntity is null)
            return new ErrorResult("Developer not found!");
        existingEntity.Id = model.Id;
        existingEntity.Name = model.Name.Trim();
        existingEntity.FoundingDate = model.FoundingDate;
        _db.Developers.Update(existingEntity);
        _db.SaveChanges();
        return new SuccessResult("Developer updated successfully.");
    }
}