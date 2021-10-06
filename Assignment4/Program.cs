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
            var connectionString = "Server=localhost;Database=Kanban;User Id=sa;Password=7b7249af-0e8d-444c-879b-555e94004d23"; //configuration.GetConnectionString("Kanban"); 

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            using var context = new KanbanContext(optionsBuilder.Options);



            // var Task1 = new Task {Title= "Make homework", AssignedTo = new User { Id = 2, Name= "Tessa", Email="Tessa@mail.dk", Tasks = new List<Task>()},
            //    Description="this is not always a funny task", State = State.Active, Tags = new List<Tag>(){new Tag {Id = 1, Name = "food", Tasks = new List<Task>()}}};
            // context.Task.Add(Task1);
            // context.SaveChanges();

            contextFactory.Seed(context);

            // var users = from u in context.User
            //             select new
            //             {
            //                 u.Name
            //             };
            // foreach (var item in users)
            // {
            //     Console.WriteLine(item.Name);
            // }
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
