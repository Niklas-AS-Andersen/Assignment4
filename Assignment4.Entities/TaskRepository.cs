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
        private readonly SqlConnection _connection;

        public TaskRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public IReadOnlyCollection<TaskDTO> All()
        {
            return null;
        }

        public int Create(TaskDTO task)
        {
            var cmdText = @"INSERT Tasks (Title, Description, AssignedToId)
                            VALUES(@Title, @Description, @AssignedToId;
                            SELECT SCOPE_IDENTITY()";
            
            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@AssignedToId", task.AssignedToId);

            OpenConnection();

            var id = command.ExecuteScalar();

            CloseConnection();

            return (int)id;
        }

        public void Delete(int taskId)
        {
            var cmdText = @"DELETE Tasks WHERE Id = @Id";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Id", taskId);

            OpenConnection();

            command.ExecuteNonQuery();

            CloseConnection();
        }

  public TaskDetailsDTO FindById(int id)
        {
            var cmdText = @"SELECT t.Id, t.Title, T.Description, t.AssignedToId, u.AssignedToName AS uName, u.AssignedToEmail AS uEmail, t.State 
                            FROM Tasks AS t
                            LEFT JOIN Users AS u ON t.AssignedToId = u.Id
                            JOIN Tags AS ta ON t.tag ......
                            WHERE t.id = @id";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@id", id);

            OpenConnection();

            using var reader = command.ExecuteReader();

            var task = reader.Read()
                ? new TaskDetailsDTO
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    AssignedToId = reader.GetInt32("AssignedToId"),
                    AssignedToName = reader.GetString("uName"),
                    AssignedToEmail= reader.GetString("uEmail"),
                    Tags = 
                    State = reader.GetString("State")
                }
                : null;

            CloseConnection();

            return task;
        }
        

        public void Update(TaskDTO task)
        {
            var cmdText = @"UPDATE Tasks SET
                            Title = @Title,
                            Description = @Description,
                            AssignedToId = @AssignedToId
                            WHERE Id = @Id";

            using var command = new SqlCommand(cmdText, _connection);

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
            _connection.Dispose();
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
