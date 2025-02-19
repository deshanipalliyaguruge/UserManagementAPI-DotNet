using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserInteractService : IUserInteractService
    {
        private readonly string connectionString;

        public UserInteractService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ResponseResult> CreateUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("InsertUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Age", user.Age);
                        command.Parameters.AddWithValue("@Gender", user.Gender ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ContactNumber", user.ContactNumber);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return new ResponseResult(true, "User created successfully.");
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

        public async Task<ResponseResult> DeleteUser(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("DeleteUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);

                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                            return new ResponseResult(true, "User deleted successfully.");
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
