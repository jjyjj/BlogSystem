using BlogSystem.BLL;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.ArticleViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.MVCSite.Controllers
{

    public class ArticleController : Controller
    {

        // GET: Article
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
                IBLL.IBlogCategoryManager blogCategoryManager = new BlogCategoryManager();
                blogCategoryManager.CreateCategory(model.CategoryName, Guid.Parse(Session["userid"].ToString()));
                return RedirectToAction("CategoryList");
            }
            ModelState.AddModelError("", "您录入的信息有误");
            return View(model);
        }
        [HttpGet]

        public async Task<ActionResult> CategoryList()
        {
            var userid = Guid.Parse(Session["userid"].ToString());
            return View(await new BlogCategoryManager().GetAllCategories());
        }
        [HttpGet]

        public async Task<ActionResult> CreateArticle()
        {
            var userid = Guid.Parse(Session["userid"].ToString());
            ViewBag.CategoryIds = await new BlogCategoryManager().GetAllCategories();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateArticle(CreateArticleViewModel model)
        {


            if (ModelState.IsValid)
            {
                var userid = Guid.Parse(Session["userid"].ToString());
                await new ArticleManager().CreateArticle(model.Title, model.Content, model.CategoryIds, userid);
                return RedirectToAction("ArticleList2");
            }
            ModelState.AddModelError("", "添加失败");
            return View();
        }


        //获取当前用户的所有文章
        [HttpGet]
        [BlogSystemAuthAttribute]
        public async Task<ActionResult> ArticleList2(int pageIndex = 1, int pageSize = 5)
        {
            //需要给页面前端 总页码数，当前页码，可显示的总页码数量

            var articleMgr = new ArticleManager();
            var userid = Guid.Parse(Session["userid"].ToString());
            //当前用户第n页数据
            var artciles = await new ArticleManager().GetAllArticlesByUserId(userid, pageIndex - 1, pageSize);
            //获取总共多少条
            var dataCount = await articleMgr.GetDataCount(userid);
            ViewBag.User = await new UserManager().GetOneUserById(userid);

            //ViewBag.Categories = await new BlogCategoryManager().GetAllCategoriesForAticleByUserId(userid);
            ViewBag.Articles = await new ArticleManager().GetNewArticleForCommentByUserId(userid);
            ViewBag.HotArticles = await new ArticleManager().GetHotArticleByUsersId(userid);

            return View(new PagedList<Dto.ArticleDto>(artciles, pageIndex, pageSize, dataCount));
        }


        //获取所有文章
        [HttpGet]
        public async Task<ActionResult> AllArticleList(int pageIndex = 1, int pageSize = 6)
        {

            var articleMgr = new ArticleManager();
            var articles = await articleMgr.GetAllArticles(false, pageIndex - 1, pageSize);
            var dataCount = await articleMgr.GetDataCount();
            foreach (var item in articles)
            {
              
                item.TotalComments = await articleMgr.GetCommentsForArticleCountByArticleId(item.Id);
            }

            return View(new PagedList<Dto.ArticleDto>(articles, pageIndex, pageSize, dataCount));

        }

        //查看文章详情
        public async Task<ActionResult> ArticleDetails(Guid? id)
        {

            var articleMgr = new ArticleManager();

            if (!await new ArticleManager().ExistsArticle(id.Value) || id == null)

                return RedirectToAction(nameof(AllArticleList));

            ViewBag.Comments = await articleMgr.GetCommentsByArticleId(id.Value);


            return View(await articleMgr.GetOneArticleById(id.Value));

        }
        //编辑
        [HttpGet]
        public async Task<ActionResult> EditArticle(Guid? id)
        {
            if (!await new ArticleManager().ExistsArticle(id.Value) || id == null)

                return RedirectToAction(nameof(AllArticleList));

            else
            {
                IBLL.IArticleManager articleManager = new ArticleManager();
                var data = await articleManager.GetOneArticleById(id.Value);
                var userid = Guid.Parse(Session["userid"].ToString());
                ViewBag.CategoryIds = await new BlogCategoryManager().GetAllCategories();
                return View(new EditArtcileViewModel()
                {
                    Title = data.Title,
                    Content = data.Content,
                    CategoryIds = data.CategoryIds,
                    Id = data.Id
                });
            }
        }
        //编辑文章
        [HttpPost]
        public async Task<ActionResult> EditArticle(EditArtcileViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IArticleManager articleManager = new ArticleManager();
                await articleManager.EditArticle(model.Id, model.Title, model.Content, model.CategoryIds);
                return RedirectToAction("ArticleList2");
            }
            else
            {

                ViewBag.CategoryIds = await new BlogCategoryManager().GetAllCategories();
                return View(model);
            }

        }
        //点赞
        [HttpPost]
        public async Task<ActionResult> GoodCount(Guid id)
        {
            IBLL.IArticleManager articleManager = new ArticleManager();
            await articleManager.GoodCountAdd(id);
            return Json(new { result = "ok" });
        }
        //反对
        [HttpPost]
        public async Task<ActionResult> BadCount(Guid id)
        {
            IBLL.IArticleManager articleManager = new ArticleManager();
            await articleManager.BadCountAdd(id);
            return Json(new { result = "ok" });
        }
        [HttpPost]
        public async Task<ActionResult> BrowseCount(Guid id)
        {
            IBLL.IArticleManager articleManager = new ArticleManager();
            await articleManager.BrowseCountAdd(id);
            return Json(new { result = "ok" });
        }
        //添加评论
        [HttpPost]
        public async Task<ActionResult> AddComment(CreateCommentViewModel model)
        {
            var userid = Guid.Parse(Session["userid"].ToString());
            IBLL.IArticleManager articleManager = new ArticleManager();
            await articleManager.CreateComment(userid, model.Id, model.Content);
            return Json(new { reslut = "ok" });
        }
        //删除文章
        [HttpPost]
        public async Task<ActionResult> DeleteOneArtcileById(Guid? id, bool isConfirm)
        {
          
                IBLL.IArticleManager articleManager = new ArticleManager();

                await articleManager.RemoveArticle(id.Value, isConfirm);
                if (isConfirm)
                {
                    return Json(new { result = "删除成功" });

                }
                else
                {
                    return Json(new { result = "下架成功" });
                }
          


        }

    }
}