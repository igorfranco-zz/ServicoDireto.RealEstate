using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using SpongeSystems.Core.Helpers.Reflection;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpongeSystems.Core.Helpers.Serialization
{
    public class SerializationHelper
    {
        public Object[] DeserializeObject(SerializeEntity[] serializedEntities)
        {
            Object[] objects = new Object[serializedEntities.Length];
            int index = 0;
            foreach (SerializeEntity serializedEntity in serializedEntities)
            {
                objects[index] = this.DeserializeObject(serializedEntity);
                index++;
            }
            return objects;
        }

        public Object DeserializeObject(SerializeEntity serializedEntity)
        {
            object result = ReflectionHelper.CreateObjectInstance(serializedEntity.ObjectTypeName,
                                                                  serializedEntity.AssemblyTypeName);
            if (result != null)
                return this.DeserializeObject(serializedEntity.XmlSource, result.GetType());
            else
                return null;
        }

        public Object DeserializeObject(XmlDocument xmlContext, System.Type type)
        {
            if (xmlContext != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                xmlContext.Save(memoryStream);
                XmlSerializer xs = new XmlSerializer(type);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                memoryStream.Position = 0;
                return xs.Deserialize(memoryStream);
            }
            else
                return null;

        }

        private String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private Byte[] StringToUTF8ByteArray(String xmlSource)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(xmlSource);
            return byteArray;
        }

        public XmlDocument[] SerializeObject(Object[] objects)
        {
            XmlDocument[] xmldocs = new XmlDocument[objects.Length];
            int index = 0;
            foreach (object actualObj in objects)
            {
                xmldocs[index] = this.SerializeObject(actualObj);
                index++;
            }

            return xmldocs;
        }

        //public static long SizeOf<T>(T _object)
        //{
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    MemoryStream stream = new MemoryStream();
        //    formatter.Serialize(stream, _object);
        //    return stream.Length;
        //}

        public static long SizeOf<T>()
        {
            return System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
        }

        public static int SizeOf(object obj)
        {
            return System.Runtime.InteropServices.Marshal.SizeOf(obj);
        }

        public XmlDocument SerializeObject(Object pObject)
        {
            StringBuilder stbResult = new StringBuilder();
            XmlDocument xml = new XmlDocument();
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                XmlSerializer xs = new XmlSerializer(pObject.GetType());
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, pObject);

                xmlTextWriter.BaseStream.Position = 0;
                xml.Load(xmlTextWriter.BaseStream);

                return xml;
            }
            catch (Exception e) { System.Console.WriteLine(e); return null; }
        }

        /// <summary>
        /// Serialize object to default entity
        /// </summary>
        /// <param name="pObject"></param>
        /// <returns></returns>
        public SerializeEntity SerializeObjectToDefaultEntity(Object pObject)
        {
            SerializeEntity serializeEntity = new SerializeEntity(this.SerializeObject(pObject),
                                                                 pObject.GetType().FullName,
                                                                 pObject.GetType().Assembly.FullName);
            return serializeEntity;
        }

        /// <summary>
        /// Serialize objects to default entities
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public SerializeEntity[] SerializeObjectToDefaultEntity(Object[] objects)
        {
            SerializeEntity[] serializedEntities = new SerializeEntity[objects.Length];
            int index = 0;
            foreach (object entity in objects)
            {
                serializedEntities[index] = this.SerializeObjectToDefaultEntity(entity);
                index++;
            }
            return serializedEntities;
        }

        public static string SerializeJSON<T>(T source)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(stream, source);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        }

        public static T DeSerializeJSON<T>(string source)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(source)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                stream.Position = 0;
                return (T)ser.ReadObject(stream);
            }
        }

        public static XDocument Serialize<T>(T source)
        {
            XDocument target = new XDocument();
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (System.Xml.XmlWriter writer = target.CreateWriter())
            {
                s.Serialize(writer, source);
            };
            return target;
        }

        public static XmlDocument SerializeAsDocument<T>(T source)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = Serialize<T>(source).CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;

        }


        public static T DeSerialize<T>(string path)
        {
            lock (typeof(T))
            {
                if (File.Exists(path))
                {
                    return DeSerialize<T>(File.Open(path, FileMode.Open));
                }
                else
                    throw new FileNotFoundException("Informe o caminho correto do arquivo");
            }
        }

        public static T DeSerialize<T>(Stream stream)
        {
            using (stream)
            {
                stream.Position = 0;
                XmlSerializer s = new XmlSerializer(typeof(T));
                return (T)s.Deserialize(stream);
            }
        }

        public static T DeSerialize<T>(XElement source)
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", null), source);
            using (MemoryStream stream = new MemoryStream())
            {
                using (var xmlReader = document.CreateReader())
                {
                    using (var xmlWriter = XmlWriter.Create(stream))
                    {
                        xmlWriter.WriteNode(xmlReader, true);
                    }
                    return DeSerialize<T>(stream);
                }
            }
        }

        public static T DeSerializeFromString<T>(string source)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(source)))
            {
                return DeSerialize<T>(stream);
            }
        }
    }
}
