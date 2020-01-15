using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
  public   interface IArticleManager
    {
        //添加文章
        Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId);
        //添加文章类别
        Task CreateCategory(string name, Guid userId);
        //查看当前用户的所有类别
        Task<List<Dto.BlogCategoryDto>> GetAllCategories(Guid userId);
        //根据用户Id去查找文章
        Task<List<Dto.ArticleDto>> GetAllArticlesByUserId(Guid userId);
        //根据用户name去查找文章
        Task<List<Dto.ArticleDto>> GetAllArticlesByEmail(string email);
        //根据类别查找文章
        Task<List<Dto.ArticleDto>> GetAllArticlesByCategoryId(Guid  categoryId);

        //删除类别
        Task RemoveCategory(string name);
        //编辑类别
        Task EditCategory(Guid categoryId, string newCategoryName);
        //删除文章
        Task RemoveArticle(Guid articleId);
        Task EditArticle(Guid articleId,string title,string content,Guid[] categoryIds);
        //判断文章是否存在
        Task<bool> ExistsArticle(Guid articleId);
        //获取文章详情
        Task<Dto.ArticleDto> GetOneArticleById(Guid articleId);
    }
}
