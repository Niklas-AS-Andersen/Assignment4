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
        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            throw new NotImplementedException();
        }

        public UserDTO Read(int userId)
        {
            throw new NotImplementedException();
        }

        public Response Update(UserUpdateDTO user)
        {
            throw new NotImplementedException();
        }

        public Response Delete(int userId, bool force = false)
        {
            throw new NotImplementedException();
        }

    }
}
