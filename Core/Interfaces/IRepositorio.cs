using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepositorio
    {
        #region 'CREATE'

        T Create<T>(T obj) where T : BaseEntity;
        Task<T> CreateAsync<T>(T obj)where T : BaseEntity;
        IEnumerable<T> AddRange<T>(IEnumerable<T> obj)where T : BaseEntity;
        Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> obj)where T : BaseEntity;
        #endregion
        #region 'READ'
        IEnumerable<T> Read<T>()where T : BaseEntity;
        Task<IEnumerable<T>> ReadAsync<T>()where T : BaseEntity;
        Boolean Exist<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        Task<Boolean> ExistAsync<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        T Find<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity; /*Single Item*/
        Task<T> FindAsync<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        T FindInclude<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria)where T : BaseEntity;
        Task<T> FindIncludeAsync<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria)where T : BaseEntity;
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity; /*Many Items*/
        IEnumerable<T> FindAllInclude<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria)where T : BaseEntity;/*Include Relationship of T*/
        Task<IEnumerable<T>> FindAllIncludeAsync<T>(Expression<Func<T, bool>> matchitem, Expression<Func<T, object>> criteria)where T : BaseEntity; /*Include Relationship of T*/
        Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        IEnumerable<T> FindAllWhere<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        Task<IEnumerable<T>> FindAllWhereAsync<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        IEnumerable<T> FindAllWhereTake<T>(Expression<Func<T, bool>> matchitem, int count)where T : BaseEntity;
        Task<IEnumerable<T>> FindAllWhereTakeAsync<T>(Expression<Func<T, bool>> matchitem, int count)where T : BaseEntity;
        IEnumerable<T> FindAllTake<T>(int count)where T : BaseEntity;
        Task<IEnumerable<T>> FindAllTakeAsync<T>(int count)where T : BaseEntity;
        T Get<T>(int id)where T : BaseEntity;
        Task<T> GetAsync<T>(int id)where T : BaseEntity;
        #endregion
        #region 'UPDATE'
        T Update<T>(T obj)where T : BaseEntity;
        Task<T> UpdateAsync<T>(T obj)where T : BaseEntity;
        #endregion
        #region 'DELETE'
        T Delete<T>(T obj)where T : BaseEntity;
        Task<T> DeleteAsync<T>(T obj)where T : BaseEntity;
        IEnumerable<T> DeleteRange<T>(IEnumerable<T> obj)where T : BaseEntity;
        Task<IEnumerable<T>> DeleteRangeAsync<T>(IEnumerable<T> obj)where T : BaseEntity;

        void DeletaAllRows<T>() where T : BaseEntity;
        #endregion
            #region 'COUNT'
        int Count<T>()where T : BaseEntity;
        Task<int> CountAsync<T>()where T : BaseEntity;
        #endregion
        #region 'EXISTS'
        Boolean Exists<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        Task<Boolean> ExistsAsync<T>(Expression<Func<T, bool>> matchitem)where T : BaseEntity;
        #endregion
    }
}