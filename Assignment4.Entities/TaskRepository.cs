using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class TaskRepository: ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<TaskDTO> All()
        {
            return null;
        }

        public int Create(TaskDTO task)
        {
            var t = new Task{
                Title = task.Title,
                Description = task.Description,
                AssignedTo = _context.Users.Find(task.AssignedToId)
            };

            _context.Tasks.Add(t);

            _context.SaveChanges();

            return t.Id;
        }

        public void Delete(int taskId)
        {
            _context.Tasks.Remove(_context.Tasks.Find(taskId));
            _context.SaveChanges();
        }

  public TaskDetailsDTO FindById(int id)
        {
            var task = _context.Tasks.Find(id);

            var taskDto = new TaskDetailsDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AssignedToId = task.AssignedTo.Id,
                AssignedToName = task.AssignedTo.Name,
                AssignedToEmail = task.AssignedTo.Email,
                Tags = helper(task.Tags),
                State = task.State
            };

            return taskDto;
        }

        public IEnumerable<string> helper(List<Tag> tag)
        {
            foreach (var item in tag)
            {
                yield return item.Name;
            }
        }
        

        public void Update(TaskDTO task)
        {
            var cmdText = @"UPDATE Tasks SET
                            Title = @Title,
                            Description = @Description,
                            AssignedToId = @AssignedToId
                            WHERE Id = @Id";

            using var command = new SqlCommand(ctext);

            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@AssignedToId", task.AssignedToId);

            OpenConnection();

            command.ExecuteNonQuery();

            CloseConnection();
        }

        public void Dispose()
        {
            _connectiontextDispose();
        }

        private void OpenConnection()
        {
            if (_context.State == ConnectionState.Closed)
            {
                _context.Open();
            }
        }

        private void CloseConnection()
        {
            if (_context.State == ConnectionState.Open)
            {
                _context.Close();
            }
        }
    }
}
