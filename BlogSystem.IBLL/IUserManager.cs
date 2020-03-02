using BlogSystem.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IUserManager
    {
        Task RegisterAsync(string email, string password);
        bool Login(string email, string password, out Guid userId);

        Task ChangePassword(string email, string oldPwd, string newPwd);
        Task ChangeUserInformation(Guid id, string email, string siteName, string imagePath, string passWord, string motto);
        Task<Dto.UserInformationDto> GetUserByEmail(string email);

        Task<Dto.UserInformationDto> GetOneUserById(Guid userId);
        Task<List<UserInformationDto>> GetAllUsers(int pageIndex, int pageSize);
        Task<bool> ExistsUser(Guid Id);
    }
      
}
