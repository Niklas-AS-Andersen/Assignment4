using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Assignment4.Entities;
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

    public static void Seed(KanbanContext context){
    context.Database.ExecuteSqlRaw("DELETE dbo.TagTask");
    context.Database.ExecuteSqlRaw("DELETE dbo.Task");
    context.Database.ExecuteSqlRaw("DELETE dbo.Tag");
    context.Database.ExecuteSqlRaw("DELETE dbo.User");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Task', RESEED, 0)");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tag', RESEED, 0)");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.User', RESEED, 0)");
    

    var user1 = new User {Name= "Tessa", Email="Tessa@mail.dk", Tasks = new List<Task>()};
    var user2 = new User {Name= "Leonora", Email="Leonora@mail.dk", Tasks = new List<Task>()};
    var user3 = new User { Name= "Frida", Email="Frida@mail.dk", Tasks = new List<Task>()};

    var foodTag = new Tag {Name = "food", Tasks = new List<Task>()};
    var SchoolTag = new Tag { Name = "School", Tasks = new List<Task>()};
    var FreeTimeTag = new Tag {Name = "Free Time", Tasks = new List<Task>()};


    /*var Task1 = new Task { Id = 1, Title= "Make homework", AssignedTo = user1,
               Description="this is not always a funny task", State = State.Active, Tags = new List<Tag>(){foodTag}}
    //var Task2 = new Task { Id = 2, Title= "Go for a run", AssignedTo = user3,
               Description="this is soooo funny", State = State.Active, Tags = new List<Tag>(){FreeTimeTag}};
    var Task3 = new Task { Id = 3, Title= "Eat a cupcake",
               Description="mums", State = State.Resolved, Tags = new List<Tag>(){foodTag,FreeTimeTag}};*/
    
    // foodTag.Tasks.Add(Task3);
    // SchoolTag.Tasks.Add(Task1);
    // FreeTimeTag.Tasks.AddRange(new List<Task>(){Task2, Task3});

    // user1.Tasks.Add(Task1);
    // user3.Tasks.Add(Task2);

   
    context.Task.AddRange(
        new Task {Title= "Go for a run", AssignedTo = user3,
               Description="this is soooo funny", State = State.Active, Tags = new List<Tag>(){FreeTimeTag}},
        new Task {Title= "Eat a cupcake",
               Description="mums", State = State.Resolved, Tags = new List<Tag>(){foodTag,FreeTimeTag}},
        new Task {Title= "Make homework", AssignedTo = user1,
               Description="this is not always a funny task", State = State.Active, Tags = new List<Tag>(){foodTag}}
    );

    context.SaveChanges();
}
 }
}