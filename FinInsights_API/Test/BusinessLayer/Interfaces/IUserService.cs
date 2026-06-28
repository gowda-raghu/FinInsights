using System.Reflection.Metadata;
using Test.DataModels;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<List<User>> GetUserById(Guid id);

    Task<User> GetUserByID(Guid id);


    Task<User> GetUserByEmail(string email);

    Task<UserDto> CreateUser(UserDto dto);

    Task<UserDto> GenerateOtp(UserDto dto);

    object PrettifyUser(User user);
}