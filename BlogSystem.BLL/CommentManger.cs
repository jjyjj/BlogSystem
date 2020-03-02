using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class CommentManger : ICommentManager
    {

        public Task<List<CommentDto>> GetCommentsByUsersId(Guid userId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<CommentDto>> GetAllComments(int pageIndex, int pageSize)
        {
            using (var commentService = new CommentService())
            {
                
                var list = await commentService
                    .GetAllByPageOrderAsync(false,pageSize, pageIndex, false)
                    .Select(m => new CommentDto()
                    {
                        Id=m.Id,
                        Content=m.Content,
                        CreateTime=m.CreateTime,
                        UserId=m.User.Id,
                        User =m.User,
                        ArticleId=m.ArticleId
                    }).ToListAsync();
                using (IArticleService articleService = new ArticleService())
                {
                    foreach (var commentDto in list)
                    {
                        var cates = await articleService
                            .GetAllAsync(false)
                            .Where(m => m.Id == commentDto.ArticleId)
                            .ToListAsync();

                        commentDto.ArticleTitles = cates.Select(m => m.Title).ToArray();
                    }
                    return list;
                }
              
            }
        }
        public async Task<int> GetDataCount()
        {
            using (IDAL.ICommentService  commentService = new CommentService())
            {
                return await commentService.GetAllAsync(false).CountAsync();
            }
        }
    }
}
