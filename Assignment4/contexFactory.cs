using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Assignment4.Entities{
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
 }
}