using Business.Models;
using Business.Results;
using Business.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IGameService
{
    IQueryable<GameModel> Query();
    Result Add(GameModel model);
    Result Update(GameModel model);
    Result Delete(int id);

}

public class GameService : IGameService
{
    private readonly Db _db;

    public GameService(Db db)
    {
        _db = db;
    }

    public IQueryable<GameModel> Query()
    {
        return _db.Games.Include(e => e.Developer).Include(e => e.Category)
            .OrderByDescending(e => e.Name)
            .ThenBy(e => e.ReleaseDate)
            .Select(e => new GameModel()
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                Name = e.Name,
                Price = e.Price,
                Revenue = e.Revenue,
                ReleaseDate = e.ReleaseDate,
                DeveloperId = e.Developer.Id,
                DeveloperOutput = e.Developer.Name,
                CategoryOutput = e.Category.Name,
                ReleaseDateOutput = e.ReleaseDate.Date.ToString("dd MMM yyyy") ?? ""

            });
    }

    public Result Add(GameModel model)
    {
        if (_db.Games.Any(e => e.Name.ToUpper() == model.Name.ToUpper().Trim()))
            return new ErrorResult("Game with the same name already exists!");

        Game entity = new Game()
        {
            Name = model.Name,
            Price = model.Price,
            Revenue = model.Revenue,
            ReleaseDate = model.ReleaseDate,
            CategoryId = model.CategoryId,
            DeveloperId = model.DeveloperId
        };

        _db.Games.Add(entity);
        _db.SaveChanges();
        return new SuccessResult("Game successfully added!");
    }

    public Result Delete(int id)
    {
        Game entity = _db.Games.SingleOrDefault(g => g.Id == id);
        if (entity is null)
            return new ErrorResult("Game not found!");
        _db.Games.Remove(entity);
        _db.SaveChanges();
        return new SuccessResult("Game deleted successfully.");
    }

    public Result Update(GameModel model)
    {
        if (_db.Games.Any(s => s.Name.ToUpper() == model.Name.ToUpper().Trim() && s.Id != model.Id))
            return new ErrorResult("Game with the same Game name already exists!");
        Game existingEntity = _db.Games.SingleOrDefault(s => s.Id == model.Id);
        if (existingEntity is null)
            return new ErrorResult("Game not found!");
        existingEntity.Id = model.Id;
        existingEntity.Name = model.Name.Trim();
        existingEntity.Revenue = model.Revenue;
        existingEntity.Price = model.Price;
        existingEntity.ReleaseDate = model.ReleaseDate;
        existingEntity.CategoryId = model.CategoryId;
        existingEntity.DeveloperId = model.CategoryId;
        _db.Games.Update(existingEntity);
        _db.SaveChanges();
        return new SuccessResult("Game updated successfully.");
    }
}

