using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infracstructure.DataAccess
{
    public class Repositorio : IRepositorio
    {
        #region 'Components'
        protected PTContext _dbContext;
        public Repositorio(PTContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion
        #region 'Methods'
        #region 'CREATE'
        public IEnumerable<T> AddRange<T>(IEnumerable<T> obj) where T : BaseEntity
        {
            _dbContext.Set<T>().AddRange(obj);
            _dbContext.SaveChanges();
            return obj;
        }
        public async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> obj) where T : BaseEntity
        {
            _dbContext.Set<T>().AddRange(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
        public T Create<T>(T obj) where T : BaseEntity
        {
            _dbContext.Set<T>().Add(obj);
            _dbContext.SaveChanges();
            return obj;
        }
        public async Task<T> CreateAsync<T>(T obj) where T : BaseEntity
        {
            _dbContext.Set<T>().Add(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
        #endregion
        #region 'READ'

        public T Find<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return _dbContext.Set<T>().SingleOrDefault(matchitem);
        }
        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(matchitem);
        }
        public T FindInclude<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria) where T : BaseEntity
        {
            return _dbContext.Set<T>().Include(criteria).SingleOrDefault(matchitem);
        }
        public async Task<T> FindIncludeAsync<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria) where T : BaseEntity
        {
            return await _dbContext.Set<T>().Include(criteria).SingleOrDefaultAsync(matchitem);
        }
        public IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return _dbContext.Set<T>().Where(matchitem).ToList();
        }
        public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return await _dbContext.Set<T>().Where(matchitem).ToListAsync();
        }
        public IEnumerable<T> FindAllInclude<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria) where T : BaseEntity
        {
            return _dbContext.Set<T>().Where(matchitem).Include(criteria).ToList();
        }
        public async Task<IEnumerable<T>> FindAllIncludeAsync<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria) where T : BaseEntity
        {
            return await _dbContext.Set<T>().Where(matchitem).Include(criteria).ToListAsync();
        }
        public IEnumerable<T> FindAllTake<T>(int count) where T : BaseEntity
        {
            return _dbContext.Set<T>().Take(count).ToList();
        }
        public async Task<IEnumerable<T>> FindAllTakeAsync<T>(int count) where T : BaseEntity
        {
            return await _dbContext.Set<T>().Take(count).ToListAsync();
        }
        public IEnumerable<T> FindAllWhere<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return _dbContext.Set<T>().Where(matchitem).ToList();
        }
        public async Task<IEnumerable<T>> FindAllWhereAsync<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return await _dbContext.Set<T>().Where(matchitem).ToListAsync();
        }
        public IEnumerable<T> FindAllWhereTake<T>(Expression<Func<T, bool>> matchitem, int count) where T : BaseEntity
        {
            return _dbContext.Set<T>().Where(matchitem).Take(count).ToList();
        }
        public async Task<IEnumerable<T>> FindAllWhereTakeAsync<T>(Expression<Func<T, bool>> matchitem, int count) where T : BaseEntity
        {
            return await _dbContext.Set<T>().Where(matchitem).Take(count).ToListAsync();
        }
        public T Get<T>(int id) where T : BaseEntity
        {
            return _dbContext.Set<T>().Find(id);
        }
        public async Task<T> GetAsync<T>(int id) where T : BaseEntity
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public IEnumerable<T> Read<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().ToList();
        }
        public async Task<IEnumerable<T>> ReadAsync<T>() where T : BaseEntity
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        #endregion
        #region 'UPDATE'
        public T Update<T>(T obj) where T : BaseEntity
        {
            T exist = _dbContext.Set<T>().Find(obj.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(obj); ;
            _dbContext.SaveChanges();
            return obj;
        }
        public async Task<T> UpdateAsync<T>(T obj) where T : BaseEntity
        {
            T exist = _dbContext.Set<T>().Find(obj.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(obj);
            //_dbContext.Entry(obj).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return obj;
        }
        #endregion
        #region 'DELETE'
        public T Delete<T>(T obj) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(obj);
            _dbContext.SaveChanges();
            return obj;
        }
        public async Task<T> DeleteAsync<T>(T obj) where T : BaseEntity
        {
            T exist = _dbContext.Set<T>().Find(obj.Id);
            _dbContext.Set<T>().Remove(exist);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
        public IEnumerable<T> DeleteRange<T>(IEnumerable<T> obj) where T : BaseEntity
        {
            _dbContext.Set<T>().RemoveRange(obj);
            _dbContext.SaveChanges();
            return obj;
        }
        public async Task<IEnumerable<T>> DeleteRangeAsync<T>(IEnumerable<T> obj) where T : BaseEntity
        {
            _dbContext.Set<T>().RemoveRange(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
        #endregion
        #region 'COUNT'
        public int Count<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().Count();
        }
        public async Task<int> CountAsync<T>() where T : BaseEntity
        {
            return await _dbContext.Set<T>().CountAsync();
        }
        #endregion
        #region 'EXISTS'
        public bool Exist<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity /*Using for Generic Repository*/
        {
            return _dbContext.Set<T>().SingleOrDefault(matchitem) != null;
        }
        public async Task<bool> ExistAsync<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(matchitem) != null;
        }
        public bool Exists<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity /*Using for Manage Operations*/
        {
            return _dbContext.Set<T>().SingleOrDefault(matchitem) != null;
        }
        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> matchitem) where T : BaseEntity
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(matchitem) != null;
        }
        #endregion
        #endregion
    }
}
