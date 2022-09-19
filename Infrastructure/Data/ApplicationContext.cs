using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Movie>().HasMany(m => m.Platforms).WithMany(p => p.Movies);
            //modelBuilder.Entity<Serial>().HasMany(s => s.Platforms).WithMany(p => p.Serials);
            modelBuilder.Entity<Platform>().HasData(new Platform{ Id = 1,Name = "Without platform"});
            modelBuilder.Entity<Status>().HasData(new Status {  Id = 1, Name = "Downloaded"}, new Status { Id = 2, Name = "Downloading"}, new Status { Id = 3, Name = "None" }, new Status { Id = 4, Name ="Isn`t downloaded"});
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Serial> Serials { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<PlatformMovie> PlatformMovies { get; set; }
        public virtual DbSet<PlatformSerial> PlatformSerials { get; set; }
    }
}
