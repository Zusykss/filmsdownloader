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
            modelBuilder.Entity<Status>().HasData(new Status {  Id = 1, Name = "Downloaded"}, new Status { Id = 2, Name = "Downloading"}, new Status { Id = 3, Name = "None" }, new Status { Id = 4, Name ="Isn`t downloaded"});
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Serial> Serials { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
