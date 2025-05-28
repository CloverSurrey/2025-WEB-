using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Shopping.Models
{
    internal interface IUser
    {
        void Add(User user);
        void Save();
        // 检查邮箱是否已存在
        bool IsEmailExists(string email);
        // 注册新用户
        bool Register(User user);

        // 验证登录
        bool Login(string email, string password);
        // 验证密码
        bool ValidatePassword(string password);
    }
}
