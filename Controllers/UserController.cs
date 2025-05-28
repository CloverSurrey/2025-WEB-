using System;
using System.Linq;
using System.Web.Mvc;
using Music_Shopping.Models;
using Music_Shopping.Models.Services;

namespace Music_Shopping.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService; // 依赖接口

        // 无参数构造函数，用于MVC框架（当无DI容器时）
        public UserController()
        {
            var dbContext = new Music_ShoppingEntities();
            _userService = new UserService(dbContext);
        }

        // 构造函数，用于依赖注入
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: users/Register
        public ActionResult Register()
        {    
            return View();
        }

        // POST: users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Email,Pwd,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.RegisterUser(user);
                switch (result)
                {
                    case RegistrationResult.Success:
                        return RedirectToAction("Login");
                    case RegistrationResult.EmailExists:
                        ModelState.AddModelError("Email", "该邮箱已被注册");
                        break;
                    case RegistrationResult.InvalidData:
                        ModelState.AddModelError("", "提交的数据无效，请检查。");
                        break;
                }
            }
            // 如果ModelState无效或注册失败，则返回带有错误信息的视图
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
        public ActionResult Login(string email, string password) // 直接接收email和password
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "邮箱和密码不能为空。");
                return View();
            }

            var result = _userService.LoginUser(email, password);
            if (result.Success)
            {
                Session["UserId"] = result.User.user_id;
                Session["UserName"] = result.User.Name;
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "邮箱或密码错误");
                return View(); 
            }
        }

        // GET: Members/Logout
        public ActionResult Logout()
        { 
            Session.Clear();
            return RedirectToAction("Login");
        }

        // 检查邮箱是否已存在 (通过服务层)
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            var exists = _userService.IsEmailTaken(email);
            return Json(new { exists = exists });
        }
        
    }
}