using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IBlogCategoryManager
    {
     
        //添加文章类别
        Task CreateCategory(string name, Guid userId);
        //查看当前用户的所有类别
        Task<List<Dto.BlogCategoryDto>> GetAllCategories(Guid userId, int pageIndex, int pageSize);
        //根据类别查找文章
        Task<List<Dto.ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId);

        //删除类别
        Task RemoveCategory(Guid categoryId);
        //编辑类别
        Task EditCategory(Guid categoryId, string newCategoryName);
    }
}
