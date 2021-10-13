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
    public class UserRepository: IUserRepository
    {
        private readonly KanbanContext _context;

        public UserRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            if (_context.Users.Find(user.Email) != null) return (Conflict, -1);

            var u = new User
            {
                Name = user.Name,
                Email = user.Email
            };

            _context.Users.Add(u);

            _context.SaveChanges();

            return (Created, u.Id);
        }

        public IReadOnlyCollection<UserDTO> ReadAll() =>
            _context.Users
                .Select(u => new UserDTO(u.Id, u.Name, u.Email))
                .ToList().AsReadOnly();
        public UserDTO Read(int userId) =>
            _context.Users
                .Select(u => new UserDTO(u.Id, u.Name, u.Email))
                .Where(u => u.Id == userId).FirstOrDefault();

        public Response Update(UserUpdateDTO user)
        {
            var entity = _context.Users.Find(user.Id);

            if (entity == null) return NotFound;
            if (_context.Users.Find(user.Email) != null) return Conflict;

            entity.Name = user.Name;
            entity.Email = user.Email;

            _context.SaveChanges();

            return Updated;
        }

        public Response Delete(int userId, bool force = false)
        {
            if (!force) return Conflict;

            var entity = _context.Users.Find(userId);
            _context.Users.Remove(entity);

            _context.SaveChanges();

            return Deleted;
        }

    }
}
