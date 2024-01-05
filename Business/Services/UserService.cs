using Business.Models;
using Business.Results;
using Business.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services;

public interface IUserService
{
    IQueryable<UserModel> Query();
    Result Add(UserModel model);
    Result Update(UserModel model);
    Result Delete(int id);

}

public class UserService : IUserService
{
    private readonly Db _db;

    public UserService(Db db)
    {
        _db = db;
    }

    public IQueryable<UserModel> Query()
    {
        return _db.Users.OrderByDescending(e => e.UserName)
           .ThenBy(e => e.IsAdmin)
           .Select(e => new UserModel()
           {
               Id = e.Id,
               UserName = e.UserName,
               Password = e.Password,
               PasswordOutput = new string('*', e.Password.Length),
               IsAdmin = e.IsAdmin,
               IsAdminOutput = e.IsAdmin ? "Admin" : "Regular"
           });
    }

    public Result Add(UserModel model)
    {
        if (_db.Users.Any(s => s.UserName.ToUpper() == model.UserName.ToUpper().Trim()))
            return new ErrorResult("User with the same user name already exists!");

        User entity = new User()
        {

            Id = model.Id,
            UserName = model.UserName,
            Password = model.Password,
            IsAdmin= model.IsAdmin,
        };

        _db.Users.Add(entity);
        _db.SaveChanges();
        return new SuccessResult("User added successfully");
    }

    public Result Delete(int id)
    {
        User entity = _db.Users.SingleOrDefault(s => s.Id == id);
        if (entity is null)
            return new ErrorResult("User not found!");
        _db.Users.Remove(entity);
        _db.SaveChanges();
        return new SuccessResult("User deleted successfully.");
    }

    public Result Update(UserModel model)
    {
        var existingUsers = _db.Users.Where(u => u.Id != model.Id).ToList();
        if (existingUsers.Any(u => u.UserName.Equals(model.UserName.Trim(), StringComparison.OrdinalIgnoreCase)))
            return new ErrorResult("User with the same user name already exists!");

        var existingEntity = _db.Users.SingleOrDefault(u => u.Id == model.Id);
        if (existingEntity is null)
            return new ErrorResult("User not found!");

        existingEntity.Id = model.Id;
        existingEntity.UserName = model.UserName.Trim();
        existingEntity.Password = model.Password.Trim();
        existingEntity.IsAdmin = model.IsAdmin;

        _db.Users.Update(existingEntity);
        _db.SaveChanges();
        return new SuccessResult("User updated successfully.");
    }
}

