using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
  public interface ICommentManager
    {
        //根据用户id获取所发表的评论
        Task<List<Dto.CommentDto>> GetCommentsByUsersId(Guid userId);
        Task<int> GetDataCount();
        //获取所有评论
        Task<List<Dto.CommentDto>> GetAllComments(int pageIndex, int pageSize);
    }
}
