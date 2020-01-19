using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class CommentService : BaseService<Models.Comment>, IDAL.ICommentService
    {
        public CommentService() : base(new BlogContext())
        {
        }
    }
}
