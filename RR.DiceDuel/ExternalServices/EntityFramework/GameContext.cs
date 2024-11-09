using Microsoft.EntityFrameworkCore;
using RR.DiceDuel.ExternalServices.EntityFramework.Entities;

namespace RR.DiceDuel.ExternalServices.EntityFramework;

public class GameContext(DbContextOptions<GameContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<StatisticEntity> Statistics { get; set; }
    
    public DbSet<ConfigEntity> Config { get; set; }
}