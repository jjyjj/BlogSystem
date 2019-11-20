using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
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
        public Task EditArticle(Guid articleId, string title, string content, Guid[] categoryIds)
        {
            throw new NotImplementedException();
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
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public Task RemoveArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 删除文章类别
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task RemoveCategory(string name)
        {
            throw new NotImplementedException();
        }
    }
}
