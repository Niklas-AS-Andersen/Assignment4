using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using Assignment4.Core;
using static Assignment4.Core.Response;
using Microsoft.EntityFrameworkCore;

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
            if (_context.Users.Find(task.AssignedToId) == null) return (BadRequest, -1);

            var t = new Task{
                Title = task.Title,
                Description = task.Description,
                AssignedTo = _context.Users.Find(task.AssignedToId),
                Created = DateTime.UtcNow,
                State = State.New,
                Tags = tagsHelper(task.Tags).ToHashSet(),
                StateUpdated = DateTime.UtcNow
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

            if (entity.State == State.New) _context.Tasks.Remove(entity);
            else if (entity.State == State.Active) 
            {
                entity.State = State.Removed;
                entity.StateUpdated = DateTime.UtcNow;
            }
            else return Conflict;

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
                           t.Created,
                           t.AssignedTo.Name,
                           t.Tags.Select(t => t.Name).ToList().AsReadOnly(),
                           t.State,
                           t.StateUpdated
                       );

            return taskDto.FirstOrDefault();
        }
        

        public Response Update(TaskUpdateDTO task)
        {
            var entity = _context.Tasks.Find(task.Id);

            if (entity == null) return NotFound;

            if (_context.Users.Find(task.AssignedToId) == null) return BadRequest;

            entity.Title = task.Title;
            entity.AssignedTo = _context.Users.Find(task.AssignedToId);
            entity.Description = task.Description;
            
            if (entity.State != task.State)
            { 
                entity.StateUpdated = DateTime.UtcNow;
                entity.State = task.State;
            }
            
            entity.Tags = tagsHelper(task.Tags).ToHashSet();

            _context.SaveChanges();

            return Updated;
        }

        public IReadOnlyCollection<TaskDTO> ReadAll() =>
            _context.Tasks
                .Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(t => t.Name).ToList().AsReadOnly(), t.State))
                .ToList().AsReadOnly();

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved() =>
            _context.Tasks
                .Where(t => t.State == State.Removed)
                .Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(t => t.Name).ToList().AsReadOnly(), t.State))
                .ToList().AsReadOnly();

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag) {

            var tasks = _context.Tasks.Include(t => t.Tags).Select(t => t);
            var tempList = new List<TaskDTO>();

            foreach (var task in tasks)
            {
                var tagname = task.Tags.Select(t => t.Name);
                if(tagname.Contains(tag)) {
                    tempList.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(t => t.Name).ToList().AsReadOnly(), task.State));
                }
                
            }

            return tempList.AsReadOnly();

        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId) =>
            _context.Tasks
                .Where(t => t.AssignedTo.Id == userId)
                .Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(t => t.Name).ToList().AsReadOnly(), t.State))
                .ToList().AsReadOnly();

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state) =>
            _context.Tasks
                .Where(t => t.State.Equals(state))
                .Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(t => t.Name).ToList().AsReadOnly(), t.State))
                .ToList().AsReadOnly();
    }
}
