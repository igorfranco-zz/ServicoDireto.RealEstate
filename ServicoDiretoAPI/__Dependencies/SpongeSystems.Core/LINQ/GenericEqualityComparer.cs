using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSystems.Core.LINQ
{
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        //string _fieldName;
        //public GenericEqualityComparer(string fieldName)
        //{
        //    this._fieldName = fieldName;
        //}

        //#region IEqualityComparer<T> Members

        //public bool Equals(T x, T y)
        //{
        //    var valX = SpongeSystems.Core.LINQ.Reflection.GetFieldAccessor<T, R>(this._fieldName);
        //    var valY = SpongeSystems.Core.LINQ.Reflection.GetFieldAccessor<T, R>(this._fieldName);
        //    return valX.Equals(valY);
        //}

        //public int GetHashCode(T obj)
        //{
        //    var valX = SpongeSystems.Core.LINQ.Reflection.GetFieldAccessor<T, R>(this._fieldName);
        //    return obj.title.GetHashCode();

        //    throw new NotImplementedException();
        //}
        //#endregion
        public bool Equals(T x, T y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
