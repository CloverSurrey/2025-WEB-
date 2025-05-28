using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using Music_Shopping.Models;

namespace Music_Shopping.Controllers
{
    public class UserController : Controller
    {
        private readonly Music_ShoppingEntities db = new Music_ShoppingEntities();

        // GET: users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: users/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Email,Pwd,Name")]User user)
        {
            var existed = db.Users.FirstOrDefault(m => m.Email == user.Email);
            if (existed != null)
            {
                ModelState.AddModelError("Email", "该邮箱已被注册");
                return View(user);
            }
            if (ModelState.IsValid)
            {
                user.Pwd = HashPassword(user.Pwd);
                user.Reg_on = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: users/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            // 对输入的密码进行哈希加密
            string hashedPassword = HashPassword(password);
            var user = db.Users.FirstOrDefault(m => m.Email == email && m.Pwd == hashedPassword);
            if (user != null)
            {
                // 登录成功，将用户信息存储在Session中
                Session["UserId"] = user.user_id;
                Session["UserName"] = user.Name;
                
                // 跳转到商品列表页面
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError("", "邮箱或密码错误");
                return View();
            }
        }

        // GET: Members/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // 检查邮箱是否已存在
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            var exists = db.Users.Any(m => m.Email == email);
            return Json(new { exists = exists });
        }

        // 哈希加密密码的方法
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}