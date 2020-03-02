using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity, new()
    {
        private readonly BlogContext _db;

        public BaseService(Models.BlogContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(T model, bool saved = true)
        {
            _db.Set<T>().Add(model);
            if (saved)
            {
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(Guid id, bool saved = true)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            var t = new T() { Id = id };
            _db.Entry(t).State = EntityState.Unchanged;
            t.IsRemoved = true;
            if (saved)
            {
                await _db.SaveChangesAsync();
                _db.Configuration.ValidateOnSaveEnabled = true;
            }
        }

        public async Task RemoveAsync(T model, bool saved = true)
        {
            await RemoveAsync(model.Id, saved);
        }
        public async Task EditAsync(T model, bool saved = true)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.Entry(model).State = EntityState.Modified;
            if (saved)
            {
                await _db.SaveChangesAsync();
                _db.Configuration.ValidateOnSaveEnabled = true;
            }
        }
        /// <summary>
        /// 返回所有未被删除数据（没有真的执行）
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAllAsync(bool all)
        {
            if (all)
            {
                return _db.Set<T>().AsNoTracking();
            }
            else
            {
                return _db.Set<T>().Where(m => !m.IsRemoved).AsNoTracking();
            }
           
        }
        public IQueryable<T> GetAllsAsync()
        {

            return _db.Set<T>().AsNoTracking();
        }
        public async Task<T> GetOneByIdAsync(bool all,Guid id)
        {
            return await GetAllAsync( all).FirstAsync(m => m.Id == id);
        }

        public IQueryable<T> GetAllByPageAsync(bool all,int pageSize = 10, int pageIndex = 0)
        {
            return GetAllAsync(all).Skip(pageSize * pageIndex).Take(pageSize);
        }


        public IQueryable<T> GetAllOrderAsync(bool all,bool asc = true)
        {
            var datas = GetAllAsync(all);
            
            datas = asc ? datas.OrderBy(m => m.CreateTime) : datas.OrderByDescending(m => m.CreateTime);
            return datas;
        }
        public IQueryable<T> GetAllByPageOrderAsync(bool all,int pageSize = 10, int pageIndex = 0, bool asc = true)
        {
            return GetAllOrderAsync(all,asc).Skip(pageSize * pageIndex).Take(pageSize);
        }
                     
        public async Task Save()
        {
            
            await _db.SaveChangesAsync();
            _db.Configuration.ValidateOnSaveEnabled = true;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
