using Microsoft.EntityFrameworkCore;
using YnabCli.Database.Commitments;
using YnabCli.Database.Milestones;
using YnabCli.Database.Settings;
using YnabCli.Database.Users;

namespace YnabCli.Database;

public class YnabCliDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingType> SettingTypes { get; set; }
    public DbSet<Commitment> Commitments { get; set; }
    public DbSet<CommitmentType> CommitmentTypes { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=YnabCli.db");
        optionsBuilder.LogTo(Console.WriteLine);
    }
}