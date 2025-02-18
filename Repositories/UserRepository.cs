using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public class UserRepository
    {
        private readonly SqlConnection connection;

        public UserRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = new List<User>();
            using (var command = new SqlCommand("GetAllUsers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ContactNumber = reader.GetString(5)
                        });
                    }
                }
                await connection.CloseAsync();
            }
            return users;
        }

        public async Task<User?> GetUserById(int id)
        {
            User? user = null;
            using (var command = new SqlCommand("GetUserById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ContactNumber = reader.GetString(5)
                        };
                    }
                }
                await connection.CloseAsync();
            }
            return user;
        }

        public async Task AddUser(User user)
        {
            using (var command = new SqlCommand("InsertUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Age", user.Age);
                command.Parameters.AddWithValue("@Gender", user.Gender ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ContactNumber", user.ContactNumber);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }

        public async Task UpdateUser(User user)
        {
            using (var command = new SqlCommand("UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Age", user.Age);
                command.Parameters.AddWithValue("@Gender", user.Gender ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ContactNumber", user.ContactNumber);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }

        public async Task DeleteUser(int id)
        {
            using (var command = new SqlCommand("DeleteUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }
    }
}
