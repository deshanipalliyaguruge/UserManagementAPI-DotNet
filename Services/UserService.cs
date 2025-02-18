using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Services
{
    public class UserService
    {
        private readonly UserRepository repository;

        public UserService(UserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await repository.GetAllUsers();
            return users.ConvertAll(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                Gender = user.Gender,
                ContactNumber = user.ContactNumber
            });
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            var user = await repository.GetUserById(id);
            return user == null ? null : new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                Gender = user.Gender,
                ContactNumber = user.ContactNumber
            };
        }

        public async Task AddUser(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Age = userDto.Age,
                Gender = userDto.Gender,
                ContactNumber = userDto.ContactNumber
            };
            await repository.AddUser(user);
        }

        public async Task UpdateUser(int id, UserDto userDto)
        {
            var user = new User
            {
                Id = id,
                Name = userDto.Name,
                Email = userDto.Email,
                Age = userDto.Age,
                Gender = userDto.Gender,
                ContactNumber = userDto.ContactNumber
            };
            await repository.UpdateUser(user);
        }

        public async Task DeleteUser(int id)
        {
            await repository.DeleteUser(id);
        }
    }
}
