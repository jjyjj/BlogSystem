using BlogSystem.BLL;
using BlogSystem.MVCSite.Models.ArticleViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class ArticleController : Controller
    {  // GET: Article
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IArticleManager articleManager = new ArticleManager();
                articleManager.CreateCategory(model.CategoryName, Guid.Parse(Session["userid"].ToString()));
                return RedirectToAction("CategoryList");
            }
            ModelState.AddModelError("", "您录入的信息有误");
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> CategoryList()
        {
            var userid = Guid.Parse(Session["userid"].ToString());
            return View(await new ArticleManager().GetAllCategories(userid));
        }
    }
}