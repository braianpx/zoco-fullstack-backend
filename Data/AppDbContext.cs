using Microsoft.EntityFrameworkCore;
using Zoco.Api.Models;

namespace Zoco.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        //DbSets
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Study> Studies => Set<Study>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<SessionLog> SessionLogs => Set<SessionLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // CONFIGURACIÓN
            //USER
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // no elimina los roles cuando se borra un usuario

            //STUDY
            modelBuilder.Entity<Study>()
               .HasOne(s => s.User)
               .WithMany(u => u.Studies)
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            //ADDRESS
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //SESSION LOG
            modelBuilder.Entity<SessionLog>()
                .HasOne(sl => sl.User)
                .WithMany(u => u.SessionLogs)
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
