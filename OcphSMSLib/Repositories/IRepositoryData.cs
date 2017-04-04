using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcphSMSLib.Repositories
{
    public interface IRepositoryData<T>
    {
        int InsertWithGetID(T t);
        bool Insert(T t);
        bool Delete(T t);
        bool Update(T t);
        T Select(int index);
        List<T> Select();
    }
}
