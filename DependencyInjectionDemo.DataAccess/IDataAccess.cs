using System.Collections.Generic;

namespace DependencyInjectionDemo.DataAccess
{
    public interface IDataAccess
    {
        List<T> LoadData<T>(string sql);
        void SaveData<T>(T model, string sql);
        void UpdateData<T>(T model, string sql);
        void DeleteData<T>(T model, string sql);

    }
}