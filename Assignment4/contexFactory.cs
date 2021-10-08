using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Assignment4.Entities;
using Assignment4.Core;
using System.Collections.Generic;

namespace Assignment4 {
 public class contextFactory : IDesignTimeDbContextFactory<KanbanContext>{
    public KanbanContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddUserSecrets<Program>()
                    .AddJsonFile("appsettings.json")
                    .Build();
                //Leonora connectionString
                var connectionString = "Server=localhost;Database=Kanban;User Id=sa;Password=7b7249af-0e8d-444c-879b-555e94004d23";

                var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>()
                    .UseSqlServer(connectionString);

                return new KanbanContext(optionsBuilder.Options);
            }
 public static void Seed(KanbanContext context)
        {
        //     context.Database.ExecuteSqlRaw("DELETE dbo.Tags");
        //     context.Database.ExecuteSqlRaw("DELETE dbo.Tasks");
        //     context.Database.ExecuteSqlRaw("DELETE dbo.Users");
        //     context.Database.ExecuteSqlRaw("DELETE dbo.TagTask");
        //    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tags', RESEED, 0)");
        //     context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tasks', RESEED, 0)");
        //    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Users', RESEED, 0)");
        //    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.TagTask', RESEED, 0)");
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

            //Users
            var leo = new User { Name = "Leo", Email = "Leo@itu.dk" };
            var tessa = new User { Name = "Tessa", Email = "Tessa@itu.dk" };
            var frida = new User { Name = "Frida", Email = "Frida@itu.dk" };

            //Tags
            var foodTag = new Tag { Name = "Food" };
            var freeTimeTag = new Tag { Name = "FreeTime" };
            var homeworkTag = new Tag { Name = "HomeWork" };
            var programmingTag = new Tag { Name = "Programming" };
            var havingFunTag = new Tag { Name = "Having_Fun" };

            //Tasks
            var task1 = new Task { Title = "make cupcakes", AssignedTo = leo, Description = "We always like to make cake", State = State.New, Tags = new List<Tag>(){ foodTag, freeTimeTag, havingFunTag } };
            var task2 = new Task { Title = "take a run", AssignedTo = tessa, Description = "running is fun", State = State.New, Tags = new List<Tag>(){ freeTimeTag } };
            var task3 = new Task { Title = "Make Assignment 4", AssignedTo = frida, Description = "not so fun, but fine", State = State.Active, Tags = new List<Tag>(){ homeworkTag, programmingTag } };
            var task4 = new Task { Title = "Make Homework2", AssignedTo = leo, Description = "very funyyyy", State = State.Resolved, Tags = new List<Tag>(){ homeworkTag } };

            context.Tasks.AddRange(
                task1, task2, task3, task4
            );
            context.SaveChanges();
        }
 }
}
