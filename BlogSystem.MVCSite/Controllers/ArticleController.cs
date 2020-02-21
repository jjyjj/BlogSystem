using BlogSystem.BLL;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.ArticleViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    [BlogSystemAuthAttribute]
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
        [HttpGet]
        public async Task<ActionResult> CreateArticle()
        {
            var userid = Guid.Parse(Session["userid"].ToString());
            ViewBag.CategoryIds = await new ArticleManager().GetAllCategories(userid);
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
        //文章列表
        [HttpGet]
        public async Task<ActionResult> ArticleList(int pageIndex = 0, int pageSize = 3)
        {
            //需要给页面前端 总页码数，当前页码，可显示的总页码数量

            var articleMgr = new ArticleManager();
            var userid = Guid.Parse(Session["userid"].ToString());
            var artciles = await new ArticleManager().GetAllArticlesByUserId(userid, pageIndex, pageSize);
            var dataCount = await articleMgr.GetDataCount(userid);

            ViewBag.PageCount = dataCount % pageSize == 0 ? dataCount / pageSize : dataCount / pageSize + 1;
            ViewBag.PageIndex = pageIndex;
            return View(artciles);
        }
        [HttpGet]
        public async Task<ActionResult> ArticleList2(int pageIndex = 1, int pageSize = 5)
        {
            //需要给页面前端 总页码数，当前页码，可显示的总页码数量

            var articleMgr = new ArticleManager();
            var userid = Guid.Parse(Session["userid"].ToString());
            //当前用户第n页数据
            var artciles = await new ArticleManager().GetAllArticlesByUserId(userid, pageIndex - 1, pageSize);
            //获取总共多少条
            var dataCount = await articleMgr.GetDataCount(userid);


            return View(new PagedList<Dto.ArticleDto>(artciles, pageIndex, pageSize, dataCount));
        }
        //查看文章详情
        public async Task<ActionResult> ArticleDetails(Guid? id)
        {

            var articleMgr = new ArticleManager();

            if (!await new ArticleManager().ExistsArticle(id.Value) || id == null)

                return RedirectToAction(nameof(ArticleList));

            ViewBag.Comments = await articleMgr.GetCommentsByArticleId(id.Value);

            return View(await articleMgr.GetOneArticleById(id.Value));

        }
        //编辑
        [HttpGet]
        public async Task<ActionResult> EditArticle(Guid id)
        {

            IBLL.IArticleManager articleManager = new ArticleManager();
            var data = await articleManager.GetOneArticleById(id);
            var userid = Guid.Parse(Session["userid"].ToString());
            ViewBag.CategoryIds = await new ArticleManager().GetAllCategories(userid);
            return View(new EditArtcileViewModel()
            {
                Title = data.Title,
                Content = data.Content,
                CategoryIds = data.CategoryIds,
                Id = data.Id
            });
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
                var userid = Guid.Parse(Session["userid"].ToString());
                ViewBag.CategoryIds = await new ArticleManager().GetAllCategories(userid);
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
        public async Task<ActionResult> DeleteOneArtcileById(Guid id)
        {
            IBLL.IArticleManager articleManager = new ArticleManager();
         
                await articleManager.RemoveArticle(id);
       
          
                
         
           
            return Json(new { result = "ok" });
        }


    }
}