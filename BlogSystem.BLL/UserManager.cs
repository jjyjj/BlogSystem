using BlogSystem.DTO;
using BlogSystem.IBLL;
using BlogSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class UserManager : IUserManager
    {


        public async Task ChangePassword(string email, string oldPwd, string newPwd)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                if (await userSvc.GetAllAsync().AnyAsync(m => m.Email == email && m.Password == oldPwd))
                {
                    var user = await userSvc.GetAllAsync().FirstAsync(m => m.Email == email);
                    user.Password = newPwd;
                    await userSvc.EditAsync(user);
                }
            }
        }

        public async Task ChangeUserInformation(string email, string siteName, string imagePath)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
               
                    var user = await userSvc.GetAllAsync().FirstAsync(m => m.Email == email);
                    user.SiteName = siteName;
                    user.ImagePath = imagePath;
                    await userSvc.EditAsync(user);
               
            }
        }

        public async Task<UserInformationDto> GetUserByEmail(string email)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                if (await userSvc.GetAllAsync().AnyAsync(m => m.Email == email))
                {
                    return await userSvc.GetAllAsync().Where(m => m.Email == email).Select(m => new DTO.UserInformationDto() {
                        Id = m.Id,
                        Email = m.Email,
                        ImagePath = m.ImagePath,
                        SiteName=m.SiteName,
                        FansCount=m.FansCount,
                        FocusCount=m.FocusCount
                    }).FirstAsync();
                }
                else
                {
                    throw new ArgumentException("邮箱地址不存在");
                }
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                return await userSvc.GetAllAsync().AnyAsync(m => m.Email == email && m.Password == password);
            }
        }

        public async Task RegisterAsync(string email, string password)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                await userSvc.CreateAsync(new User()
                {
                    Email = email,
                    Password = password,
                    SiteName = "默认的网站",
                    ImagePath = "default.png"
                });
            }
        }
    }
}
