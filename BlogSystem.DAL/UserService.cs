using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class UserService : BaseService<Models.User>, IUserService
    {
        public UserService() : base(new BlogContext())
        {
        }
    }
}