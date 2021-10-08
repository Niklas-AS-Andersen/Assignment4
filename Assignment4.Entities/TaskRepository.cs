using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using Assignment4.Core;
using static Assignment4.Core.Response;

namespace Assignment4.Entities
{
    public class TaskRepository: ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            var t = new Task{
                Title = task.Title,
                Description = task.Description,
                AssignedTo = _context.Users.Find(task.AssignedToId),
                Tags = tagsHelper(task.Tags)
            };

            _context.Tasks.Add(t);

            _context.SaveChanges();

            return (Created, t.Id);
        }

        private IEnumerable<Tag> tagsHelper(ICollection<string> tags)
        {
            var existing = _context.Tags.Where(t => tags.Contains(t.Name)).ToDictionary(t => t.Name);

            foreach (var tag in tags)
            {
                yield return existing.TryGetValue(tag, out var t) ? t : new Tag{Name = tag};
            }
        }

        public Response Delete(int taskId)
        {
            var entity = _context.Tasks.Find(taskId);

            if (entity == null)
            {
                return NotFound;
            }

            _context.Tasks.Remove(_context.Tasks.Find(taskId));
            _context.SaveChanges();

            return Deleted;
        }

        public TaskDetailsDTO Read(int taskId)
        {
            var task = _context.Tasks.Find(taskId);

            if (task == null) return null;

            var taskDto = from t in _context.Tasks
                       where t.Id == taskId
                       select new TaskDetailsDTO(
                           t.Id,
                           t.Title,
                           t.Description,
                           DateTime.Now,        // Created, skal ændres
                           t.AssignedTo.Name,
                           _context.Tags.Select(t => t.Name).ToList(),
                           t.State,
                           DateTime.Now         // StateUpdated, skal ændres
                       );

            return taskDto.FirstOrDefault();
        }
        

        public Response Update(TaskUpdateDTO task)
        {
            var entity = _context.Tasks.Find(task.Id);

            if (entity == null) return NotFound;

            entity.Title = task.Title;
            entity.AssignedTo = _context.Users.Find(task.AssignedToId);
            entity.Description = task.Description;
            entity.State = task.State;
            entity.Tags = tagsHelper(task.Tags);

            _context.SaveChanges();

            return Updated;
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            throw new NotImplementedException();
        }
    }
}
