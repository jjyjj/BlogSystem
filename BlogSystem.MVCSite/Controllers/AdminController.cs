using BlogSystem.BLL;
using BlogSystem.MVCSite.Models.ArticleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
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
            return View(await new ArticleManager().GetAllCategories());
        }
        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AllArticleList(int pageIndex = 1, int pageSize = 6)
        {

            var articleMgr = new ArticleManager();
            var articles = await articleMgr.GetAllArticles(pageIndex - 1, pageSize);
            var dataCount = await articleMgr.GetDataCount();
            foreach (var item in articles)
            {
                item.TotalComments = await articleMgr.GetCommentsForArticleCountByArticleId(item.Id);
            }

            return View(new PagedList<Dto.ArticleDto>(articles, pageIndex, pageSize, dataCount));

        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AllUserList(int pageIndex = 1, int pageSize = 6)
        {

            var userMgr = new UserManager();
            
            var users = await userMgr.GetAllUsers(pageIndex - 1, pageSize);
            var dataCount = await userMgr.GetDataCount();
        

            return View(new PagedList<Dto.UserInformationDto>(users, pageIndex, pageSize, dataCount));

        }
    }
}