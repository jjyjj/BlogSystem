using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IUserManager
    {
        Task RegisterAsync(string email, string password);
        Task<bool> LoginAsync(string email, string password);

        Task ChangePassword(string email, string oldPwd, string newPwd);
        Task ChangeUserInformation(string email, string siteName, string imagePath);
        Task<DTO.UserInformationDto> GetUserByEmail(string email);

    }
}
