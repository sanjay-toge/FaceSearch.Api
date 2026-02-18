using Microsoft.EntityFrameworkCore;

namespace FaceSearch.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}
