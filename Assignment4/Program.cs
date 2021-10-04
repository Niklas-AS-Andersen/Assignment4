using System;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Assignment4.Entities;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var connectionString = configuration.GetConnectionString("Kanban");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            using var context = new KanbanContext(optionsBuilder.Options);

            var tag1 = new Tag 
            {
                Name = "SWU software",
                Tasks = new List<Task>()
            };

            var user1 = new User
            {
                Name = "Paolo",
                Email = "paolo@itu.dk",
                Tasks = new List<Task>()
            };

            var task1 = new Task 
            {
                Title = "Assignment 4 SE",
                AssignedTo = user1,
                Description = "Write the SE part",
                State = State.New,
                Tags = new List<Tag>(){tag1}
            };

            tag1.Tasks.Add(task1);
            user1.Tasks.Add(task1);

            var tasks = from t in context.Tasks
                        select new
                        {
                            t.Title
                        };
            foreach (var item in tasks)
            {
                Console.WriteLine(item);
            }
        }

        static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>();

            return builder.Build();
        }
    }
}
