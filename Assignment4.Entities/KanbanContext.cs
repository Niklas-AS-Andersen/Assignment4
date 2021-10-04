using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Assignment4.Entities
{
    public class KanbanContext : DbContext
    {
        //const string conString = "Server=localhost;Database=Kanban;User Id=sa;Password=14a7372d-9ca3-468a-9b75-e2266bd96ebe";
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Task>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (State)Enum.Parse(typeof(State), v));
        }
    }
}
