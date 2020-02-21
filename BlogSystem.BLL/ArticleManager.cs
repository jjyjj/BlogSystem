using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class ArticleManager : IArticleManager
    {
        public async Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId)
        {
            using (var articleSvc = new ArticleService())
            {
                var article = new Article()
                {
                    Title = title,
                    Content = content,
                    UserId = userId
                };
                await articleSvc.CreateAsync(article);

                Guid articleId = article.Id;
                using (var articleToCategorySvc = new ArticleToCategoryService())
                {
                    foreach (var categoryId in categoryIds)
                    {
                        await articleToCategorySvc.CreateAsync(new ArticleToCategory()
                        {
                            ArticleId = articleId,
                            BlogCategoryId = categoryId
                        }, saved: false);
                    }

                    await articleToCategorySvc.Save();

                }
            }

        }

        public async Task CreateCategory(string name, Guid userId)
        {
            //using (var categorySvc = new BlogCategoryService())
            //{
            //    await categorySvc.CreateAsync(new BlogCategory()
            //    {
            //        CategoryName = name,
            //        UserId = userId
            //    });
            //}
            using (IDAL.IBlogCategory s = new BlogCategoryService())
            {
                await s.CreateAsync(new BlogCategory()
                {
                    CategoryName = name,

                    UserId = userId
                });
            }
        }
        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public async Task EditArticle(Guid articleId, string title, string content, Guid[] categoryIds)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var article = await articleService.GetOneByIdAsync(articleId);
                article.Title = title;
                article.Content = content;
                await articleService.EditAsync(article);
                using (IDAL.IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    foreach (var categoryId in articleToCategoryService.GetAllAsync().Where(m => m.ArticleId == articleId))
                    {
                        await articleToCategoryService.RemoveAsync(categoryId, false);
                    }

                    foreach (var categroyId in categoryIds)
                    {
                        await articleToCategoryService.CreateAsync(new ArticleToCategory()
                        {
                            ArticleId = articleId,
                            BlogCategoryId = categroyId
                        }, false);
                    }
                    await articleToCategoryService.Save();

                }
            }
        }
        /// <summary>
        /// 编辑文章类别
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="newCategoryName"></param>
        /// <returns></returns>
        public Task EditCategory(Guid categoryId, string newCategoryName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsArticle(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                return await articleService.GetAllAsync().AnyAsync(m => m.Id == articleId);
            }

        }

        /// <summary>
        /// 根据文章类别查找文章
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据用户Email查找文章
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<List<ArticleDto>> GetAllArticlesByEmail(string email)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查找当前用户所有文章
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize)
        {
            using (var articleService = new ArticleService())
            {
                var list = await articleService
                    .GetAllByPageOrderAsync(pageSize, pageIndex, false)

                    .Include(m => m.User)
                    .Where(m => m.UserId == userId)
                    .Select(m => new ArticleDto()
                    {
                        Title = m.Title,
                        Content = m.Content,
                        GoodConut = m.GoodConut,
                        Eamil = m.User.Email,
                        CreateTime = m.CreateTime,
                        BadCount = m.BadCount,
                        Id = m.Id,
                        ImagePath = m.User.ImagePath
                    }).ToListAsync();
                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    foreach (var articleDto in list)
                    {
                        var cates = await articleToCategoryService.GetAllAsync().Include(m => m.BlogCategory).Where(m => m.ArticleId == articleDto.Id).ToListAsync();
                        articleDto.CategoryIds = cates.Select(m => m.BlogCategoryId).ToArray();
                        articleDto.CategoryNames = cates.Select(m => m.BlogCategory.CategoryName).ToArray();
                    }
                    return list;
                }

            }
        }

        public Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查找当前用户所有文章类别
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<BlogCategoryDto>> GetAllCategories(Guid userId)
        {
            using (IDAL.IBlogCategory categoryService = new BlogCategoryService())
            {
                return await categoryService.GetAllAsync().Where(m => m.UserId == userId).Select(m => new BlogCategoryDto()
                {
                    Id = m.Id,
                    CategoryName = m.CategoryName

                }).ToListAsync();
            }
        }

        public Task<List<BlogCategoryDto>> GetAllCategories(Guid userId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task RemoveArticle(Guid articleId)
        {

            using (IDAL.IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
            {
                foreach (var categoryId in articleToCategoryService.GetAllAsync().Where(m => m.ArticleId == articleId))
                {
                    await articleToCategoryService.RemoveAsync(categoryId);
                }
              
                using (IDAL.IArticleService articleService = new ArticleService())
                {
                    await articleService.RemoveAsync(articleId);
                    await articleToCategoryService.Save();
                    await articleService.Save();
                }

            }
          

        }
        /// <summary>
        /// 删除文章类别
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task RemoveCategory(Guid categoryId)
        {

            IDAL.IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService();
            await articleToCategoryService.RemoveAsync(categoryId, false);
            await articleToCategoryService.Save();

        }

        //获取一篇文章详情
        public async Task<ArticleDto> GetOneArticleById(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var data = await articleService.GetAllAsync()
                    .Include(m => m.User)//联表查询
                    .Where(m => m.Id == articleId)//查询条件
                    .Select(m => new Dto.ArticleDto()
                    {
                        Id = m.Id,
                        BadCount = m.BadCount,
                        Title = m.Title,
                        Content = m.Content,
                        CreateTime = m.CreateTime,
                        Eamil = m.User.Email,
                        GoodConut = m.GoodConut,
                        ImagePath = m.User.ImagePath
                    }).FirstAsync();//取出该条数据
                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    var cates = await articleToCategoryService
                        .GetAllAsync()
                        .Include(m => m.BlogCategory)
                        .Where(m => m.ArticleId == data.Id)
                        .ToListAsync();
                    data.CategoryIds = cates.Select(m => m.BlogCategoryId).ToArray();
                    data.CategoryNames = cates.Select(m => m.BlogCategory.CategoryName).ToArray();
                    return data;
                }

            }
        }
        //获取总页码
        public async Task<int> GetDataCount(Guid UserId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                return await articleService.GetAllAsync().CountAsync(m => m.UserId == UserId);
            }
        }
        //点赞
        public async Task GoodCountAdd(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var article = await articleService.GetOneByIdAsync(articleId);
                article.GoodConut++;
                await articleService.EditAsync(article);

            }
        }

        //反对
        public async Task BadCountAdd(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var article = await articleService.GetOneByIdAsync(articleId);
                article.BadCount++;
                await articleService.EditAsync(article);

            }
        }
        //评论
        public async Task CreateComment(Guid userId, Guid articleId, string content)
        {
            using (IDAL.ICommentService commentService = new CommentService())
            {
                await commentService.CreateAsync(new Comment()
                {
                    UserId = userId,
                    ArticleId = articleId,
                    Content = content
                });
            }
        }
        //查询评论
        public async Task<List<Dto.CommentDto>> GetCommentsByArticleId(Guid articleId)
        {
            using (IDAL.ICommentService commentService = new CommentService())
            {
                return await commentService.GetAllOrderAsync(asc: false)
                     .Where(m => m.ArticleId == articleId)
                     .Include(m => m.User)
                     .Select(m => new Dto.CommentDto()
                     {
                         Id = m.Id,
                         ArticleId = m.ArticleId,
                         Content = m.Content,
                         CreateTime = m.CreateTime,
                         UserId = m.UserId,
                         Email = m.User.Email
                     }).ToListAsync();
            }
        }
    }
}
