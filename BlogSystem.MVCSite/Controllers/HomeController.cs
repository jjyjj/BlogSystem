using BlogSystem.BLL;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

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
        public async Task<ActionResult> Register(Models.UserViewModels.RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            IBLL.IUserManager userManager = new UserManager();
            await userManager.RegisterAsync(model.Email, model.Password);
            return Content("注册成功    ");


        }
    }
}