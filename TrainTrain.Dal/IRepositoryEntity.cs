using System.Collections.Generic;

namespace TrainTrain.Dal
{
    public interface IRepositoryEntity<T>
    {
        T Get(string id);
        List<T> GetAll();
        void Save(T entity);
        void Remove(T entity);
        void RemoveAll();
    }
}