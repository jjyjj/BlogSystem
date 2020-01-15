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
        public async Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId)
        {
            using (var articleService = new ArticleService())
            {
                var list = await articleService.GetAllAsync().Include(m => m.User).Where(m => m.UserId == userId).Select(m => new ArticleDto()
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
