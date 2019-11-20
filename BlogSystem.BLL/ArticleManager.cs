using BlogSystem.DAL;
using BlogSystem.DTO;
using BlogSystem.IBLL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class ArticleManager : IArticleManager
    {
        public async Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId)
        {
            using (var articleSvc=new ArticleService())
            {
                var article = new Article()
                {
                    Title = title,
                    Content = content,
                    UserId = userId
                };
                await articleSvc.CreateAsync(article);

                Guid atriclId = article.Id;
                using (var atricleToCategorySvc=new ArticleToCategoryService())
                {
                    foreach (var categoryId in categoryIds)
                    {
                        await atricleToCategorySvc.CreateAsync(new ArticleToCategory() {
                            ArticleId = atriclId,
                            BlogCategoryId=categoryId
                        },saved:false);
                    }
                    await atricleToCategorySvc.Save();
                }
            }
        }

        public async Task CreateCategory(string name, Guid userId)
        {
            using (var categorySvc=new BlogCategoryService())
            {
                await categorySvc.CreateAsync(new BlogCategory() {
                    CategoryName = name,
                    UserId=userId
                }); 
            }
        }

        public Task EditArticle(Guid articleId, string title, string content, Guid[] categoryIds)
        {
            throw new NotImplementedException();
        }

        public Task EditCategory(Guid categoryId, string newCategoryName)
        {
            throw new NotImplementedException();
        }

        public Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ArticleDto>> GetAllArticlesByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlogCategoryDto>> GetAllBlogCategories(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCategory(string name)
        {
            throw new NotImplementedException();
        }
    }
}
