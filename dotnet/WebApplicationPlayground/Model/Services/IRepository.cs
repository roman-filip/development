using System.Collections.Generic;

namespace Model.Services
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetItemById(int id);

        IEnumerable<TEntity> GetAllItems();

        void Save(TEntity entity);
    }
}
