using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace SpongeSolutions.Core.Helpers.Reflection
{
    /// <summary> 
    /// Helps to get and set property values on objects through reflection. 
    /// Properties of underlying objects can be accessed directly by separating 
    /// the levels in the hierarchy by dots. 
    /// To get/set the name of an Ancestor, for objects that have a Parent property, 
    /// you could use "Parent.Parent.Parent.Name". 
    /// </summary> 
    public class ReflectionHelper
    {
        private const char PropertyNameSeparator = '.';
        private static readonly object[] NoParams = new object[0];
        private static readonly Type[] NoTypeParams = new Type[0];
        private IDictionary<Type, PropertyInfoCache> propertyCache = new Dictionary<Type, PropertyInfoCache>();
        private IDictionary<Type, ConstructorInfo> constructorCache = new Dictionary<Type, ConstructorInfo>();

        /// <summary> 
        /// Gets the Type of the given property of the given targetType. 
        /// The targetType and propertyName parameters can't be null. 
        /// </summary> 
        /// <param name="targetType">the target type which contains the property</param> 
        /// <param name="propertyName">the property to get, can be a property on a nested object (eg. "Child.Name")</param>
        public Type GetType(Type targetType, string propertyName)
        {
            if (propertyName.IndexOf(PropertyNameSeparator) > -1)
            {
                string[] propertyList = propertyName.Split(PropertyNameSeparator);
                for (int i = 0; i < propertyList.Length; i++)
                {
                    string currentProperty = propertyList[i];
                    targetType = GetTypeImpl(targetType, currentProperty);
                }
                return targetType;
            }
            else
            {
                return GetTypeImpl(targetType, propertyName);
            }
        }

        /// <summary> 
        /// Gets the value of the given property of the given target. 
        /// If objects within the property hierarchy are null references, null will be returned. 
        /// The target and propertyName parameters can't be null. 
        /// </summary> 
        /// <param name="target">the target object to get the value from</param> 
        /// <param name="propertyName">the property to get, can be a property on a nested object (eg. "Child.Name")</param>
        public object GetValue(object target, string propertyName)
        {
            if (propertyName.IndexOf(PropertyNameSeparator) > -1)
            {
                string[] propertyList = propertyName.Split(PropertyNameSeparator);
                for (int i = 0; i < propertyList.Length; i++)
                {
                    string currentProperty = propertyList[i];
                    target = GetValueImpl(target, currentProperty);
                    if (target == null)
                    {
                        return null;
                    }
                }
                return target;
            }
            else
            {
                return GetValueImpl(target, propertyName);
            }
        }

        /// <summary> 
        /// Sets the value of the given property on the given target to the given value. 
        /// If objects within the property hierarchy are null references, an attempt will be 
        /// made to construct a new instance through a parameterless constructor. 
        /// The target and propertyName parameters can't be null. 
        /// </summary> 
        /// <param name="target">the target object to set the value on</param> 
        /// <param name="propertyName">the property to set, can be a property on a nested object (eg. "Child.Name")</param>

        /// <param name="value">the new value of the property</param> 
        public void SetValue(object target, string propertyName, object value)
        {
            if (propertyName.IndexOf(PropertyNameSeparator) > -1)
            {
                object originalTarget = target;
                string[] propertyList = propertyName.Split(PropertyNameSeparator);
                for (int i = 0; i < propertyList.Length - 1; i++)
                {
                    propertyName = propertyList[i];
                    target = GetValueImpl(target, propertyName);
                    if (target == null)
                    {
                        string currentFullPropertyNameString = GetPropertyNameString(propertyList, i);
                        target = Construct(GetType(originalTarget.GetType(), currentFullPropertyNameString));
                        SetValue(originalTarget, currentFullPropertyNameString, target);
                    }
                }
                propertyName = propertyList[propertyList.Length - 1];
            }
            SetValueImpl(target, propertyName, value);
        }

        private TimeSpan GetTimeSpanDefinition(string value)
        {
            string[] timeDefinition = value.Split(':');
            return new TimeSpan(Convert.ToInt16(timeDefinition[0]),
                                Convert.ToInt16(timeDefinition[1]),
                                Convert.ToInt16(timeDefinition[2]));

        }

        private object DynamicCoverter(System.Reflection.PropertyInfo propertyInfo, object value)
        {
            if (value == null)
                return null;
            else if (propertyInfo.PropertyType == typeof(System.TimeSpan))
                return this.GetTimeSpanDefinition(value.ToString());
            else if (propertyInfo.PropertyType.BaseType == typeof(System.Enum))
                return Enum.Parse(propertyInfo.PropertyType, value.ToString());
            else if (propertyInfo.PropertyType == typeof(System.Boolean) && (value.ToString() == "1" || value.ToString() == "0"))
                return Convert.ToBoolean(Convert.ToInt16(value));
            else
                return value;//Convert.ChangeType(value, propertyInfo.PropertyType);
        }

        /// <summary> 
        /// Returns a string containing the properties in the propertyList up to the given 
        /// level, separated by dots. 
        /// For the propertyList { "Zero", "One", "Two" } and level 1, the string 
        /// "Zero.One" will be returned. 
        /// </summary> 
        /// <param name="propertyList">the array containing the properties in the corect order</param> 
        /// <param name="level">the level up to wich to include the properties in the returned string</param> 
        /// <returns>a dot-separated string containing the properties up to the given level</returns> 
        private static string GetPropertyNameString(string[] propertyList, int level)
        {
            StringBuilder currentFullPropertyName = new StringBuilder();
            for (int j = 0; j <= level; j++)
            {
                if (j > 0)
                {
                    currentFullPropertyName.Append(PropertyNameSeparator);
                }
                currentFullPropertyName.Append(propertyList[j]);
            }
            return currentFullPropertyName.ToString();
        }

        /// <summary> 
        /// Returns the type of the given property on the target instance. 
        /// The type and propertyName parameters can't be null. 
        /// </summary> 
        /// <param name="targetType">the type of the target instance</param> 
        /// <param name="propertyName">the property to retrieve the type for</param> 
        /// <returns>the typr of the given property on the target type</returns> 
        private Type GetTypeImpl(Type targetType, string propertyName)
        {
            return GetPropertyInfo(targetType, propertyName).PropertyType;
        }

        /// <summary> 
        /// Returns the value of the given property on the target instance. 
        /// The target instance and propertyName parameters can't be null. 
        /// </summary> 
        /// <param name="target">the instance on which to get the value</param> 
        /// <param name="propertyName">the property for which to get the value</param> 
        /// <returns>the value of the given property on the target instance</returns> 
        private object GetValueImpl(object target, string propertyName)
        {
            return GetPropertyInfo(target.GetType(), propertyName).GetValue(target, NoParams);
        }

        /// <summary> 
        /// Sets the given property of the target instance to the given value. 
        /// Type mismatches in the parameters of these methods will result in an exception. 
        /// Also, the target instance and propertyName parameters can't be null. 
        /// </summary> 
        /// <param name="target">the instance to set the value on</param> 
        /// <param name="propertyName">the property to set the value on</param> 
        /// <param name="value">the value to set on the target</param> 
        private void SetValueImpl(object target, string propertyName, object value)
        {
            PropertyInfo propInfo = GetPropertyInfo(target.GetType(), propertyName);
            propInfo.SetValue(target, this.DynamicCoverter(propInfo, value), NoParams);
            //propInfo.SetValue(target, value, NoParams); 

        }

        /// <summary> 
        /// Obtains the PropertyInfo for the given propertyName of the given type from the cache. 
        /// If it is not already in the cache, the PropertyInfo will be looked up and added to 
        /// the cache. 
        /// </summary> 
        /// <param name="type">the type to resolve the property on</param> 
        /// <param name="propertyName">the name of the property to return the PropertyInfo for</param> 
        /// <returns></returns> 
        public PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfoCache propertyInfoCache = GetPropertyInfoCache(type);
            if (!propertyInfoCache.ContainsKey(propertyName))
            {
                PropertyInfo propertyInfo = GetBestMatchingProperty(propertyName, type);
                if (propertyInfo == null)
                {
                    throw new ArgumentException(String.Format("Unable to find public property named {0} on type {1}", propertyName, type.FullName), propertyName);

                }
                propertyInfoCache.Add(propertyName, propertyInfo);
            }
            return propertyInfoCache[propertyName];
        }

        /// <summary> 
        /// Gets the best matching property info for the given name on the given type if the same property is defined on 
        /// multiple levels in the object hierarchy. 
        /// </summary> 
        public PropertyInfo GetBestMatchingProperty(string propertyName, Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            PropertyInfo bestMatch = null;
            int bestMatchDistance = int.MaxValue;
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo info = propertyInfos[i];
                if (info.Name == propertyName)
                {
                    int distance = CalculateDistance(type, info.DeclaringType);
                    if (distance == 0)
                    {
                        // as close as we're gonna get... 
                        return info;
                    }
                    if (distance > 0 && distance < bestMatchDistance)
                    {
                        bestMatch = info;
                        bestMatchDistance = distance;
                    }
                }
            }
            return bestMatch;
        }

        /// <summary> 
        /// Calculates the hierarchy levels between two classes. 
        /// If the targetObjectType is the same as the baseType, the returned distance will be 0. 
        /// If the two types do not belong to the same hierarchy, -1 will be returned. 
        /// </summary> 
        private static int CalculateDistance(Type targetObjectType, Type baseType)
        {
            if (!baseType.IsInterface)
            {
                Type currType = targetObjectType;
                int level = 0;
                while (currType != null)
                {
                    if (baseType == currType)
                    {
                        return level;
                    }
                    currType = currType.BaseType;
                    level++;
                }
            }
            return -1;
        }

        /// <summary> 
        /// Returns the PropertyInfoCache for the given type. 
        /// If there isn't one available already, a new one will be created. 
        /// </summary> 
        /// <param name="type">the type to retrieve the PropertyInfoCache for</param> 
        /// <returns>the PropertyInfoCache for the given type</returns> 
        private PropertyInfoCache GetPropertyInfoCache(Type type)
        {
            if (!propertyCache.ContainsKey(type))
            {
                lock (this)
                {
                    if (!propertyCache.ContainsKey(type))
                    {
                        propertyCache.Add(type, new PropertyInfoCache());
                    }
                }
            }
            return propertyCache[type];
        }

        /// <summary> 
        /// Creates a new object of the given type, provided that the type has a default (parameterless) 
        /// constructor. If it does not have such a constructor, an exception will be thrown. 
        /// </summary> 
        /// <param name="type">the type of the object to construct</param> 
        /// <returns>a new instance of the given type</returns> 
        public object Construct(Type type)
        {
            if (!constructorCache.ContainsKey(type))
            {
                lock (this)
                {
                    if (!constructorCache.ContainsKey(type))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(NoTypeParams);
                        if (constructorInfo == null)
                        {
                            throw new Exception(String.Format("Unable to construct instance, no parameterless constructor found in type {0}", type.FullName));

                        }
                        constructorCache.Add(type, constructorInfo);
                    }
                }
            }
            return constructorCache[type].Invoke(NoParams);
        }

        public T Construct<T>()
        {
            Type type = typeof(T);
            if (!constructorCache.ContainsKey(type))
            {
                lock (this)
                {
                    if (!constructorCache.ContainsKey(type))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(NoTypeParams);
                        if (constructorInfo == null)
                        {
                            throw new Exception(String.Format("Unable to construct instance, no parameterless constructor found in type {0}", type.FullName));

                        }
                        constructorCache.Add(type, constructorInfo);
                    }
                }
            }
            return (T)constructorCache[type].Invoke(NoParams);
        }

        /// <summary>
        /// Efetua a criação de um objeto apenas pelas suas definicoes de assembly
        /// </summary>
        /// <param name="objectTypeName"></param>
        /// <param name="assemblyTypeName"></param>
        /// <returns></returns>
        public static object CreateObjectInstance(string objectTypeName, string assemblyTypeName)
        {
            object objectResult = Activator.CreateInstance(assemblyTypeName, objectTypeName).Unwrap();
            if (objectResult == null)
                throw new Exception(String.Format("Object {0} can't be created.", objectTypeName));
            return objectResult;
        }

        public static object[] CreateObjectCollectionInstance(string objectTypeName, string assemblyTypeName, int length)
        {
            object objectBase = CreateObjectInstance(objectTypeName, assemblyTypeName);
            Array resultArray = System.Array.CreateInstance(objectBase.GetType(), length);
            return ((object[])resultArray);
        }

        public static object[] CreateObjectCollectionInstance(Type type, int length)
        {
            Array resultArray = System.Array.CreateInstance(type, length);
            return ((object[])resultArray);
        }

        public static object DoInvokeMember(object sourceObject, string methodName, string[] parameterNames, object[] parameterValues)
        {

            if (parameterNames != null)
            {
                return sourceObject.GetType().InvokeMember(methodName,
                                                           System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public,
                                                           (Binder)null,
                                                           sourceObject,
                                                           parameterValues,
                                                           (ParameterModifier[])null,
                                                           (CultureInfo)null,
                                                           parameterNames);
            }
            else
            {
                return sourceObject.GetType().InvokeMember(methodName,
                                                           System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public,
                                                           (Binder)null,
                                                           sourceObject,
                                                           parameterValues);
            }
        }
    }

    /// <summary> 
    /// Keeps a mapping between a string and a PropertyInfo instance. 
    /// Simply wraps an IDictionary and exposes the relevant operations. 
    /// Putting all this in a separate class makes the calling code more 
    /// readable. 
    /// </summary> 
    internal class PropertyInfoCache
    {
        private IDictionary<string, PropertyInfo> propertyInfoCache;

        public PropertyInfoCache()
        {
            propertyInfoCache = new Dictionary<string, PropertyInfo>();
        }

        public bool ContainsKey(string key)
        {
            return propertyInfoCache.ContainsKey(key);
        }

        public void Add(string key, PropertyInfo value)
        {
            propertyInfoCache.Add(key, value);
        }

        public PropertyInfo this[string key]
        {
            get { return propertyInfoCache[key]; }
            set { propertyInfoCache[key] = value; }
        }
    }
}


