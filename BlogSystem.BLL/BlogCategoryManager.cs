using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
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

        public async Task RemoveCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
