using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly string connectionString;

        public UserService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<UserResponse<List<User>>> GetAllUsers()
        {
            var users = new List<User>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
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
                }
                return new UserResponse<List<User>>(true, "Users retrieved successfully.", users);
            }
            catch (Exception ex)
            {
                return new UserResponse<List<User>>(false, $"Error retrieving users: {ex.Message}", null);
            }
        }

        public async Task<UserResponse<User>> GetUserById(int id)
        {
            User user = null;

            try
            {
                using (var connection = new SqlConnection(connectionString))
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
                }
                if (user != null)
                    return new UserResponse<User>(true, "User found.", user);
                else
                    return new UserResponse<User>(false, "User not found.", null);
            }
            catch (Exception ex)
            {
                return new UserResponse<User>(false, $"Error retrieving user: {ex.Message}", null);
            }
        }
    }
}
