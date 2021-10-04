using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Collections.Generic;

namespace Assignment4.Entities
{
    public class KanbanContext : DbContext
    {
        public DbSet<Task> Task { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<User> User { get; set; }

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }
        
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Task>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (State)Enum.Parse(typeof(State), v));
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasIndex(e => e.Email)
                    .IsUnique();
                entity.HasMany(e => e.Tasks);
            });
            
            var tag1 = new Tag 
            {
                Id = 1,
                Name = "SWU software"
            };
            
            var user1 = new User
            {
                Id = 1,
                Name = "Paolo",
                Email = "paolo@itu.dk"
            };

            var task1 = new Task
            {
                Id = 1,
                Title = "Assignment 4 SE",
                AssignedTo = user1,
                Description = "Write the SE part",
                State = State.New,
                Tags = new List<Tag>(){tag1}
            };

            // tag1.Tasks.Add(task1);
            // user1.Tasks.Add(task1);

            //modelBuilder.Entity<Task>().Ignore(e => e.AssignedTo);
            modelBuilder.Entity<User>().HasData(user1);
            //modelBuilder.Entity<Tag>().HasData(tag1);
            //modelBuilder.Entity<Task>().HasData(task1);
        }
    }
}
