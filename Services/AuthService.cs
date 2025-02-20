using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserManagementAPI.Models;
using UserManagementAPI.Models.AuthModels;
using System.Security.Cryptography;
using System.Diagnostics;


namespace UserManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }

        public async Task<ResponseResult<LoginResponse>> SignIn(LoginRequest loginRequest)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("AuthenticateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        string hashedPassword = HashPassword(loginRequest.Password);
                        Console.WriteLine($"Hashed Password: {hashedPassword}");

                        command.Parameters.AddWithValue("@UserName", loginRequest.UserName);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword); // Hash before passing

                        await connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        if (reader.Read())
                        {
                            string userName = reader.GetString(6);
                            int userId = reader.GetInt32(0);
                            Debug.WriteLine(userId,userName);
                            string token = GenerateJwtToken(userName, userId);
                            Debug.WriteLine(token);
                            return new ResponseResult<LoginResponse>(true, "Login successful", new LoginResponse(token, "Login successful"));

                        }
                        return new ResponseResult<LoginResponse>(false, "Invalid credentials");

                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult<LoginResponse>(false, $"Error: {ex.Message}");
            }
        }



        public async Task<ResponseResult<object>> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("ChangeUserPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", request.UserId);
                        command.Parameters.AddWithValue("@NewPasswordHash", HashPassword(request.NewPassword)); // Hash before storing

                        SqlParameter outputParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        string resultMessage = outputParam.Value.ToString();

                        if (resultMessage == "Success")
                            return new ResponseResult<object>(true, "Password updated successfully.");
                        else
                            return new ResponseResult<object>(false, resultMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult<object>(false, $"Error: {ex.Message}");
            }
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


        private string GenerateJwtToken(string username, int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", userId.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
