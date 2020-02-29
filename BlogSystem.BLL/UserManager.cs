using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class UserManager : IUserManager
    {


        public async Task RegisterAsync(string email, string password)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                await userSvc.CreateAsync(new User()
                {
                    Email = email,
                    Password = password,
                    SiteName = "小码农",
                    ImagePath = "../Image/defult.png",
                    State=0,//0未冻结 1冻结
                    type=0,//0普通用户 1管理员
                    Motto="野蛮而生长，自由而向上"
                });
            }
        }

        public bool Login(string email, string password, out Guid userid)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                var user = userSvc.GetAllAsync().FirstOrDefaultAsync(m => m.Email == email && m.Password == password);
                user.Wait();
                var data = user.Result;
                if (data == null)
                {
                    userid = new Guid();
                    return false;
                }
                else
                {
                    userid = data.Id;
                    return true;
                }

            }
        }

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

        public async Task ChangeUserInformation(Guid id, string email, string siteName, string imagePath, string passWord,string motto)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {

                var user = await userSvc.GetAllAsync().FirstAsync(m => m.Id == id);
                user.SiteName = siteName;
                user.ImagePath = imagePath;
                user.Password = passWord;
                user.Motto = motto;
                await userSvc.EditAsync(user);

            }
        }

        public async Task<UserInformationDto> GetUserByEmail(string email)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                if (await userSvc.GetAllAsync().AnyAsync(m => m.Email == email))
                {
                    return await userSvc.GetAllAsync().Where(m => m.Email == email).Select(m =>
                        new Dto.UserInformationDto()
                        {
                            Id = m.Id,
                            Email = m.Email,
                            FansCount = m.FansCount,
                            ImagePath = m.ImagePath,
                            SiteName = m.SiteName,
                            FocusCount = m.FocusCount
                        }).FirstAsync();
                }
                else
                {
                    throw new ArgumentException("邮箱地址不存在");
                }
            }
        }

        public async Task<UserInformationDto> GetOneUserById(Guid userId)
        {
            using (IDAL.IUserService userService = new UserService())
            {
                return await userService.GetAllAsync()
                    .Where(m => m.Id == userId)
                    .Select(m => new Dto.UserInformationDto()
                    {
                        Id = m.Id,
                        Email = m.Email,
                        ImagePath = m.ImagePath,
                        SiteName = m.SiteName,
                        PassWord = m.Password,
                        Motto=m.Motto
                        
                    }).FirstAsync();//取出该条数据;

            }
        }

        public async Task<int> GetDataCount()
        {
            using (IDAL.IUserService userService = new UserService())
            {
                return await userService.GetAllAsync().CountAsync();
            }
        }
        public async Task<List<UserInformationDto>> GetAllUsers(int pageIndex, int pageSize)
        {
            using (var userService = new UserService())
            {
                var list = await userService
                    .GetAllByPageOrderAsync(pageSize, pageIndex, false)

                    .Select(m => new UserInformationDto()
                    {
                        Id=m.Id,
                        SiteName = m.SiteName,
                        Email = m.Email,
                        ImagePath = m.ImagePath,
                        PassWord = m.Password,
                        FansCount = m.FansCount,
                        FocusCount = m.FocusCount,
                        CreateTime=m.CreateTime
                        
                    }).ToListAsync();
                return list;
            }
        }
    }
}
