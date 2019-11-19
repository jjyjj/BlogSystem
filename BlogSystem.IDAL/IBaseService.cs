using BlogSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.IDAL
{
    public interface IBaseService<T>:IDisposable where T : BaseEntity
    {
        Task CreateAsync(T model, bool saved = true);
        Task EditAsync(T mdoel, bool saved = true);
        Task RemoveAsync(Guid id, bool saved = true);
        Task RemoveAsync(T model, bool saved = true);
        Task Save();
        Task<T> GetOneByIdAsync(Guid id);
        IQueryable<T> GetAllAsync();
        IQueryable<T> GetAllByPageAsync(int pageSize = 10, int pageIndex = 0);
        IQueryable<T> GetAllOrderAsync(bool asc = true);
        IQueryable<T> GetAllByPageOrderAsync(int pageSize = 10, int pageIndex = 0, bool asc = true);

    }
}
