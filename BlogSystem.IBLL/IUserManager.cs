using System;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IUserManager
    {
        Task RegisterAsync(string email, string password);
       bool Login(string email, string password,out Guid userId);

        Task ChangePassword(string email, string oldPwd, string newPwd);
        Task ChangeUserInformation(string email, string siteName, string imagePath);
        Task<Dto.UserInformationDto> GetUserByEmail(string email);

    }
}
