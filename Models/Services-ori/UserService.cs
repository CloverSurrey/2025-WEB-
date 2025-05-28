using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Music_Shopping.Models.Services
{
    public class UserService : IUserService
    {
        private readonly Music_ShoppingEntities _db;

        public UserService(Music_ShoppingEntities dbContext)
        {
            _db = dbContext;
        }

        public RegistrationResult RegisterUser(User user)
        {
            if (IsEmailTaken(user.Email))
            {
                return RegistrationResult.EmailExists;
            }

            // 可以在这里添加更复杂的用户数据验证逻辑
            // if (!IsValid(user)) return RegistrationResult.InvalidData;

            user.Pwd = HashPassword(user.Pwd);
            user.Reg_on = DateTime.Now;
            
            _db.Users.Add(user);
            _db.SaveChanges();
            
            return RegistrationResult.Success;
        }

        public LoginResult LoginUser(string email, string password)
        {
            string hashedPassword = HashPassword(password);
            var user = _db.Users.FirstOrDefault(m => m.Email == email && m.Pwd == hashedPassword);

            if (user != null)
            {
                return new LoginResult { Success = true, User = user };
            }
            else
            {
                return new LoginResult { Success = false, ErrorMessage = "邮箱或密码错误" };
            }
        }

        public bool IsEmailTaken(string email)
        {
            return _db.Users.Any(m => m.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _db.Users.FirstOrDefault(u => u.user_id == userId);
        }

        // 哈希加密密码的方法 (可以设为 private, 因为只在服务内部使用)
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty; // 处理空密码或null密码的情况
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
} 