﻿using BlogSystem.BLL;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.UserViewModels;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class HomeController : Controller
    {
        
        [BlogSystemAuth]
        public ActionResult Index()
        {
            return View();
        }
        [BlogSystemAuth]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [BlogSystemAuth]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            IBLL.IUserManager userManager = new UserManager();
            await userManager.RegisterAsync(model.Email, model.Password);
            return Content("注册成功");

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IUserManager userManager = new UserManager();
                Guid userid;
                if (userManager.Login(model.Email, model.LoginPwd, out userid))
                {
                    //跳转
                    //判断是用session还是用cookie
                    if (model.RememberMe)
                    {
                        Response.Cookies.Add(new HttpCookie("loginName")
                        {
                            Value = model.Email,
                            Expires = DateTime.Now.AddDays(7)
                        });
                        Response.Cookies.Add(new HttpCookie("userId")
                        {
                            Value = userid.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    else
                    {
                        Session["loginName"] = model.Email;
                        Session["userid"] = userid;
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "您的账号密码有误");
                }
            }
            return View(model);
        }
    }
}