using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UserManagementAPI.Models;
using UserManagementAPI.Models.UserModels;

namespace UserManagementAPI.Services
{
    public class UserInteractService : IUserInteractService
    {
        private readonly string connectionString;

        public UserInteractService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ResponseResult<object>> CreateUser(User user)
        {
            try
            {
                string userName = GenerateUserName(user.Name);
                string password = GenerateRandomPassword();
                string passwordHash = HashPassword(password);

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
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        return new ResponseResult<object>(true, $"User created successfully. Username: {userName}");
                    }
                }
            }
            catch (SqlException ex)
            {
                return new ResponseResult<object>(false, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ResponseResult<object>(false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<ResponseResult<object>> DeleteUser(int id)
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
                            return new ResponseResult<object>(true, "User deleted successfully.");
                        else
                            return new ResponseResult<object>(false, "User not found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                return new ResponseResult<object>(false, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ResponseResult<object>(false, $"An error occurred: {ex.Message}");
            }
        }

        private string GenerateUserName(string name)
        {
            return name.Replace(" ", "").ToLower() + new Random().Next(100, 999);
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
