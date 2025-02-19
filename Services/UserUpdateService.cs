using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserUpdateService : IUserUpdateService
    {
        private readonly string connectionString;

        public UserUpdateService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ResponseResult> UpdateUser(int id, User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("UpdateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Age", user.Age);
                        command.Parameters.AddWithValue("@Gender", user.Gender ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ContactNumber", user.ContactNumber);

                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                            return new ResponseResult(true, "User updated successfully.");
                        else
                            return new ResponseResult(false, "User not found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                return new ResponseResult(false, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, $"An error occurred: {ex.Message}");
            }
        }
    }
}
