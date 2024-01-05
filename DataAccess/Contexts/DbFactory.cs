using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Contexts;

public class DbFactory : IDesignTimeDbContextFactory<Db>
{
    public Db CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Db>();
        optionsBuilder.UseMySQL("Server=127.0.0.1; Port=8889; Database=GamesProject; Uid=root; Pwd=root");

        return new Db(optionsBuilder.Options);
    }
}