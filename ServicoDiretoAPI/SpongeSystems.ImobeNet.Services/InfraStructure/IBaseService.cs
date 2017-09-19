using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.ServicoDireto.Services.InfraStructure
{
    public interface IBaseService<T, C>
    {
        void Insert(T entity);
        void Delete(T entity);
        void Delete(C id);
        void Update(T entity);
        T GetById(C id);
        IList<T> GetAll();
    }

    public interface IBaseService<T> : IBaseService<T, int>
    { 
    }
}
