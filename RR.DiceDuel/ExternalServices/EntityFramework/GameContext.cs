using Microsoft.EntityFrameworkCore;
using RR.DiceDuel.ExternalServices.EntityFramework.Entities;

namespace RR.DiceDuel.ExternalServices.EntityFramework;

public class GameContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    
    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }
}