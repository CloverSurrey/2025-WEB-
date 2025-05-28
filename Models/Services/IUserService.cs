using Music_Shopping.Models;
using System.ComponentModel.DataAnnotations;

namespace Music_Shopping.Models.Services
{
    // 用于 UserService 返回注册结果
    public enum RegistrationResult
    {
        Success,
        EmailExists,
        InvalidData
    }

    // 用于 UserService 返回登录结果
    public class LoginResult
    {
        public bool Success { get; set; }
        public User User { get; set; }
        public string ErrorMessage { get; set; }
    }

    public interface IUserService
    {
        RegistrationResult RegisterUser(User user);
        LoginResult LoginUser(string email, string password);
        bool IsEmailTaken(string email);
        User GetUserById(int userId); // 可能用于Session恢复等场景
    }
} 