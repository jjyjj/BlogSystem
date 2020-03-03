using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class BlogCategoryManager : IBlogCategoryManager
    {
     

        public async Task CreateCategory(string name, Guid userId)
        {
           
            using (IDAL.IBlogCategory s = new BlogCategoryService())
            {
                await s.CreateAsync(new BlogCategory()
                {
                    CategoryName = name,
                    UserId = userId
                });
            }
        }

    

        public Task EditCategory(Guid categoryId, string newCategoryName)
        {
            throw new NotImplementedException();
        }

        public Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }


        //查看所有类别
        public async Task<List<BlogCategoryDto>> GetAllCategories()
        {
            using (IDAL.IBlogCategory categoryService = new BlogCategoryService())
            {
                return await categoryService.GetAllAsync(false).Select(m => new BlogCategoryDto()
                {
                    Id = m.Id,
                    CategoryName = m.CategoryName,
                    CreateTime = m.CreateTime

                }).ToListAsync();
            }
        }
        public Task<List<BlogCategoryDto>> GetAllCategories(Guid userId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
        public Task<List<BlogCategoryDto>> GetAllCategories(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
        //这个方法有问题，后来修改
        public  Task<List<ArticleDto>> GetAllCategoriesForAticleByUserId(Guid userId)
        {
            throw new NotImplementedException();

            //    using (IDAL.IArticleService articleService = new ArticleService())
            //    {
            //        var data = await articleService.GetAllAsync(false)
            //            .Include(m => m.User)//联表查询
            //            .Where(m => m.Id == userId)//查询条件
            //            .Select(m => new Dto.ArticleDto()
            //            {
            //                Id = m.Id
            //            }).FirstAsync();//取出该条数据
            //        using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
            //        {
            //            var cates = await articleToCategoryService
            //                .GetAllAsync(false)
            //                .Include(m => m.BlogCategory)
            //                .Where(m => m.ArticleId == data.Id)
            //                .ToListAsync();
            //            data.CategoryIds = cates.Select(m => m.BlogCategoryId).ToArray();
            //            data.CategoryNames = cates.Select(m => m.BlogCategory.CategoryName).ToArray();
            //            return data;
            //        }

            //}
        }

        public async Task RemoveCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
