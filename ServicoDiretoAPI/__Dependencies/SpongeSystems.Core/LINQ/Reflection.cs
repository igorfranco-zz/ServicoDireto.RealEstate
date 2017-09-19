using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace SpongeSystems.Core.LINQ
{
    public class Reflection
    {
        public static Func<T, R> GetFieldAccessor<T, R>(string fieldName)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "arg");
            MemberExpression member = Expression.Field(param, fieldName);

            LambdaExpression lambda = Expression.Lambda(typeof(Func<T, R>), member, param);
            Func<T, R> compiled = (Func<T, R>)lambda.Compile();
            return compiled;
        }
    }
}
