using BlogSystem.BLL;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.UserViewModels;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    
    public class UserController : Controller
    {


        public ActionResult Index()
        {
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
            return RedirectToAction("Login");

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
                        Response.Cookies.Add(new HttpCookie("type")
                        {
                            Value = model.type.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    else
                    {
                        Session["loginName"] = model.Email;
                        Session["type"] = model.type;
                        Session["userid"] = userid;
                    }

                    return RedirectToAction("../Article/ArticleList2");
                }
                else
                {
                    ModelState.AddModelError("", "您的账号密码有误");
                }
            }
            return View(model);
        }
        //修改个人资料
        [BlogSystemAuth]
        [HttpGet]
        public async Task<ActionResult> EditUserInfo()
        {
            IBLL.IUserManager userManager = new UserManager();
            var userid = Guid.Parse(Session["userid"].ToString());
            var user = await userManager.GetOneUserById(userid);
            return View(new ChangeUserInfoViewModel()
            {
                Email = user.Email,
                ImagePath = user.ImagePath,
                SiteName = user.SiteName,
                Id = user.Id,
                PassWord = user.PassWord,
                Motto=user.Motto
            });
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> EditUserInfo(ChangeUserInfoViewModel user)
        {
            IBLL.IUserManager userManager = new UserManager();
            var userid = Guid.Parse(Session["userid"].ToString());
            if (ModelState.IsValid)
            {

                string image = "";
                string result = "";
                HttpPostedFileBase imageName = Request.Files["image"];// 从前台获取文件
                string file = imageName.FileName;
                if (file != "")
                {

                    string fileFormat = file.Split('.')[file.Split('.').Length - 1]; // 以“.”截取，获取“.”后面的文件后缀
                    Regex imageFormat = new Regex(@"^(bmp)|(png)|(gif)|(jpg)|(jpeg)"); // 验证文件后缀的表达式
                    if (string.IsNullOrEmpty(file) || !imageFormat.IsMatch(fileFormat)) // 验证后缀，判断文件是否是所要上传的格式
                    {
                        result = "error";
                    }
                    else
                    {

                        string timeStamp = DateTime.Now.Ticks.ToString(); // 获取当前时间的string类型
                        string firstFileName = timeStamp.Substring(0, timeStamp.Length - 4); // 通过截取获得文件名
                        string imageStr = "../image/"; // 获取保存图片的项目文件夹
                        string uploadPath = Server.MapPath(imageStr); // 将项目路径与文件夹合并
                        string pictureFormat = file.Split('.')[file.Split('.').Length - 1];// 设置文件格式
                        string fileName = userid + "-" + timeStamp + "." + fileFormat;// 设置完整（文件名+文件格式） 
                        string saveFile = uploadPath + fileName;//文件路径
                        imageName.SaveAs(saveFile);// 保存文件
                                                   // 如果单单是上传，不用保存路径的话，下面这行代码就不需要写了！
                        image = imageStr + fileName;// 设置数据库保存的路径


                    }


                }

                else
                {

                    var a = await userManager.GetOneUserById(userid);
                    image = a.ImagePath;

                }
                user.ImagePath = image;
                user.SiteName = Request.Form["name"];

                user.Motto = Request.Form["Motto"];
                await userManager.ChangeUserInformation(userid, user.Email, user.SiteName, user.ImagePath, user.PassWord,user.Motto);
                return RedirectToAction("../Article/ArticleList2");
            }
            else
            {
                return View(user);
            }
        }
        [HttpGet]
        public async Task<ActionResult> PersonalArticleList(int pageIndex = 1, int pageSize = 5)
        {
            //需要给页面前端 总页码数，当前页码，可显示的总页码数量

            var articleMgr = new ArticleManager();
            var userid = Guid.Parse(Session["userid"].ToString());
            //当前用户第n页数据
            var artciles = await new ArticleManager().GetAllArticlesByUserId(userid, pageIndex - 1, pageSize);
            //获取总共多少条
            var dataCount = await articleMgr.GetDataCount(userid);

            ViewBag.Users = await new UserManager().GetOneUserById(userid);
            return View(new PagedList<Dto.ArticleDto>(artciles, pageIndex, pageSize, dataCount));
        }

        [HttpGet]
        public async Task<ActionResult> PersonalCommentList()
        {



            var articleMgr = new ArticleManager();
            var userid = Guid.Parse(Session["userid"].ToString());
            //当前用户第n页数据
            var comments = await new ArticleManager().GetCommentsByUsersId(userid);

            ViewBag.Users = await new UserManager().GetOneUserById(userid);

            return View(comments);
        }

    }
}