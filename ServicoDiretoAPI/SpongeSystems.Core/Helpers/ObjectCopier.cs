using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using SpongeSolutions.Core.Helpers.Reflection;

namespace SpongeSolutions.Core.Helpers
{
    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// 
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>

    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source) 
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static void Copy<T, F>(T source, F result)
            where T : class
            where F : class
        {
            Copy<T, F>(source, result, null);
        }

        public static void Copy<T>(T source, T result)
            where T : class
        {
            Copy<T, T>(source, result, null);
        }

        public static void Copy<T, F>(T source, F result, bool ignoreEntitySets)
            where T : class
            where F : class
        {
            if (ignoreEntitySets)
                Copy<T, F>(source, result, new string[] { "System.Data.Linq", "SpongeSolutions.ServicoDireto.BusinessEntities" });
            else
                Copy<T, F>(source, result, null);
        }

        public static void Copy<T, F>(T source, F result, string[] ignoreAssembliesType)
            where T : class
            where F : class
        {
            ReflectionHelper reflectionHelper = new ReflectionHelper();
            foreach (var item in source.GetType().GetProperties())
            {
                try
                {
                    PropertyInfo property = result.GetType().GetProperty(item.Name);
                    if (property != null)
                    {
                        if (ignoreAssembliesType != null)
                        {
                            int count = (from assembly in ignoreAssembliesType
                                         where property.PropertyType.FullName.IndexOf(assembly) > -1
                                         select assembly).Count();
                            if (count == 0)
                                reflectionHelper.SetValue(result, item.Name, reflectionHelper.GetValue(source, item.Name));

                        }
                        else
                            reflectionHelper.SetValue(result, item.Name, reflectionHelper.GetValue(source, item.Name));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Erro ao setar o valor na propriedade <b>{0}</b>", item.Name), ex);
                }
            }

        }
    }

}
