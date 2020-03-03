using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IArticleManager
    {
        //添加文章
        Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId);
    
        //根据用户Id去查找文章
        Task<List<Dto.ArticleDto>> GetAllArticlesByUserId(Guid userId);
        //根据用户name去查找文章
        Task<List<Dto.ArticleDto>> GetAllArticlesByEmail(string email);

     
        //删除文章
        Task RemoveArticle(Guid articleId,bool isConfirm);
        Task RemoveComment(Guid commentId);
        Task EditArticle(Guid articleId, string title, string content, Guid[] categoryIds);
        //判断文章是否存在
        Task<bool> ExistsArticle(Guid articleId);
        //获取文章详情
        Task<Dto.ArticleDto> GetOneArticleById(Guid articleId);
        //获取总页码
        Task<int> GetDataCount(Guid UserId);
        //点赞
        Task GoodCountAdd(Guid articleId);
        //反对
        Task BadCountAdd(Guid articleId);
        Task BrowseCountAdd(Guid articleId);
        //评论
        Task CreateComment(Guid userId, Guid articleId, string content);
        //查看评论
        Task<List<Dto.CommentDto>> GetCommentsByArticleId(Guid articleId);
        //获取当前用户评论
        Task<List<Dto.CommentDto>> GetCommentsByUsersId(Guid usersId);
        //获取当前用户被最新评论的文章
        Task<List<Article>> GetNewArticleForCommentByUserId(Guid usersId);
        
        //获取当前用户文章中浏览量最高的文章
        Task<List<Dto.ArticleDto>> GetHotArticleByUsersId(Guid usersId);
    }

}
