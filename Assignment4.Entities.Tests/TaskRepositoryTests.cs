using System;
using Assignment4;
using Assignment4.Core;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests : IDisposable
    {

        private readonly KanbanContext _context;
        private readonly TaskRepository _repository;


        public TaskRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
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
            var task1 = new Task { Id = 1, Title = "make cupcakes", AssignedTo = leo, Description = "We always like to make cake", State = State.New, Tags = new List<Tag>(){ foodTag, freeTimeTag, havingFunTag }};
            var task2 = new Task { Id = 2, Title = "take a run", AssignedTo = tessa, Description = "running is fun", State = State.New, Tags =new List<Tag>(){ freeTimeTag }};
            var task3 = new Task { Id = 3, Title = "Make Assignment 4", AssignedTo = frida, Description = "not so fun, but fine", State = State.Active, Tags = new List<Tag>(){ homeworkTag, programmingTag }};
            var task4 = new Task { Id = 4, Title = "Make Homework2", AssignedTo = leo, Description = "very funyyyy", State = State.Resolved, Tags = new List<Tag>(){ homeworkTag }};

            context.Tasks.AddRange(
                task1, task2, task3, task4
            );
            context.Tags.AddRange(
                foodTag, freeTimeTag, homeworkTag, programmingTag, havingFunTag 
            );
            context.Users.AddRange(
                leo, tessa, frida
            );


            _context = context;
            _repository = new TaskRepository(_context);

            context.SaveChanges();
        }

        [Fact]
        public void Create_creates_new_task_with_generated_id()
        {
            var repository = new TaskRepository(_context);

            var task = new TaskCreateDTO
            {
                Title = "test test class",
                AssignedToId = 1,
                Description= "working on this test",
                Tags = new HashSet<string>{"Homework"}
            };

            var created = repository.Create(task);

            Assert.Equal((Response.Created, 5), created);
        }

        [Fact]
        public void ReadAll_returns_all_tasks()
        {
            var tasks = _repository.ReadAll();

            Assert.Collection(tasks,
                c => Assert.Equal(new TaskDTO(1,"make cupcakes", "Leo", new List<string>(){ "Food", "FreeTimeTag", "Having_Fun"}.AsReadOnly(), State.New).ToString(), c.ToString()),
                c => Assert.Equal(new TaskDTO(2,"take a run", "Tessa", new List<string>(){"FreeTimeTag"}.AsReadOnly(), State.New).ToString(), c.ToString()),
                c => Assert.Equal(new TaskDTO(3,"Make Assignment 4", "Frida", new List<string>(){"Programming", "HomeWork"}.AsReadOnly(), State.Active).ToString(), c.ToString()),
                c => Assert.Equal(new TaskDTO(4,"Make Homework2", "Leo", new List<string>(){ "HomeWork"}.AsReadOnly(), State.Resolved).ToString(), c.ToString())
            );
        }

        [Fact]
        public void ReadAllByTag_returns_all_task_with_given_tag()
        {
            var tasks = _repository.ReadAllByTag("Food");

            Assert.Collection(tasks,
                c => Assert.Equal(new TaskDTO(1,"make cupcakes", "Leo", new List<string>(){ "Food", "FreeTimeTag", "Having_Fun"}.AsReadOnly(), State.New).ToString(), c.ToString())
            );
        }

        [Fact]
        public void ReadAllByUser_returns_all_task_by_that_user()
        {
            var tasks = _repository.ReadAllByUser(2);

            Assert.Collection(tasks,
               c => Assert.Equal(new TaskDTO(2,"take a run", "Tessa", new List<string>(){"FreeTimeTag"}.AsReadOnly(), State.New).ToString(), c.ToString())
            );
        }

        [Fact]
        public void ReadAllByState_returns_all_task_with_that_State()
        {
            var tasks = _repository.ReadAllByState(State.Active);

            Assert.Collection(tasks,
               c => Assert.Equal(new TaskDTO(3,"Make Assignment 4", "Frida", new List<string>(){"Programming", "HomeWork"}.AsReadOnly(), State.Active).ToString(), c.ToString())
            );
        }

        [Fact]
        public void ReadAllRemoved_returns_all_task_with_State_Removed()
        {
            var tasks = _repository.ReadAllRemoved();

            Assert.Empty(tasks);
 
        }
        [Fact]
        public void Delete_task_with_state_Active_Returns_Response_Deleted_but_still_exists(){
            
            var repository = new TaskRepository(_context);

            var deleted = repository.Delete(3);

            Assert.Equal(Response.Deleted, deleted);
            Assert.NotNull(_context.Tasks.Find(3));    

        }

         [Fact]
        public void Delete_task_with_state_Active_Change_State_of_task_to_Removed(){
            
            var repository = new TaskRepository(_context);

            var deleted = repository.Delete(3);

            Assert.Equal(Response.Deleted, deleted);
            Assert.Equal(State.Removed,_context.Tasks.Find(3).State);
               
        }

         [Fact]
        public void Delete_given_non_existing_id_returns_NotFound()
        {
            var repository = new TaskRepository(_context);

            var deleted = repository.Delete(21);

            Assert.Equal(Response.NotFound, deleted);
        }


        [Fact]
        public void Delete_given_existing_id_deletes()
        {
            var repository = new TaskRepository(_context);

            var deleted = repository.Delete(1);

            Assert.Equal(Response.Deleted, deleted);
            Assert.Null(_context.Tasks.Find(1));
        }


          [Fact]
        public void Read_given_id_does_not_exist_returns_null()
        {
            var task = _repository.Read(21);

            Assert.Null(task);
        }

        [Fact]
        public void Read_given_id_exists_returns_task()
        {
            var repository = new TaskRepository(_context);

            var task = repository.Read(1);
            Assert.Equal(1, task.Id);
            Assert.Equal("make cupcakes", task.Title);
            Assert.Equal("We always like to make cake", task.Description);
            Assert.Equal(_context.Users.Find(1).Name, task.AssignedToName);
            Assert.Equal(State.New, task.State);
            Assert.Equal(task.Tags.ToList().ToString(), new List<string>(){"Food", "FreeTimeTag", "Having_Fun"}.ToString());
        }


         [Fact]
        public void Update_updates_existing_task()
        {
            var repository = new TaskRepository(_context);

            var task = new TaskUpdateDTO
            {
                Title = "make cupcakes",
                AssignedToId = 1,
                Description= "makeeeee cake",
                Tags = new HashSet<string>(){"Food", "FreeTimeTag", "Having_Fun"},
                Id = 1,
                State = State.New
            };

            var updated = repository.Update(task);

            Assert.Equal(Response.Updated, updated);

            var updatedTask = repository.Read(1);
            Assert.Equal(1, updatedTask.Id);
            Assert.Equal("make cupcakes", updatedTask.Title);
            Assert.Equal("makeeeee cake", updatedTask.Description);
            Assert.Equal(_context.Users.Find(1).Name, updatedTask.AssignedToName);
            Assert.Equal(State.New, updatedTask.State);
            Assert.Equal(task.Tags.ToList().ToString(), new List<string>(){"Food", "FreeTimeTag", "Having_Fun"}.ToString());
        }


        [Fact]
        public void Update_given_non_existing_id_returns_NotFound()
        {
            var repository = new TaskRepository(_context);

            var character = new TaskUpdateDTO
            {
                Title = "make cupcakes",
                AssignedToId = 1,
                Description= "makeeeee cake",
                Tags = new HashSet<string>(){},
                Id = 21,
                State = State.New
            };

            var updated = repository.Update(character);

            Assert.Equal(Response.NotFound, updated);
        }


        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
