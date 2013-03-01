using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Data;

namespace DynamicStructBuilder.Serialization
{
    /* Provides generic serialization/deserialization of an object. */
    [Serializable()]
    public class GenericSerializable<T>
    {

        protected string path { get; set; }

        public string Path
        {
            get { if (string.IsNullOrEmpty(path)) { return typeof(T).Name + ".xml"; } else { return path; } }
            set { path = value; }
        }

        // Serialize the object to a local XML file
        public virtual void Serialize()
        {
            // If no path is specified, default to the type's name as the file name.
            // For example, if type Foo is serialized, this method will generate
            // an XML file named Foo.xml in the same directory that the assembly
            // is executing. Useful if there only needs to be one serialized
            // instance of a class (e.g., AppSettings.xml).
            if (string.IsNullOrEmpty(path)) path = typeof(T).Name + ".xml";

            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(Path);
                serializer.Serialize(writer, this);
                writer.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Serialize the object to local file, allow specification
        // of part of the file name.
        protected virtual void Serialize(string UniqueName)
        {
            string typeName = typeof(T).Name;

            if (typeName.Contains("`"))
            {
                typeName = typeName.Replace(typeName.Substring(typeName.LastIndexOf("`")), "");
            }

            // If no path is specified, default to the type's name as the file name.
            // Otherwise, name the file using the unique identifier given.
            // Useful if there will be multiple serialized instances, 
            // e.g., OptionsFoo.xml, OptionsBar.xml, OptionsBat.xml.
            if (string.IsNullOrEmpty(path)) path = typeName + UniqueName + ".xml";

            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(Path);
                serializer.Serialize(writer, this);
                writer.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Returns an instance of the type to be deserialized with a unique name.
        // To be paired with public static void Serialize(UniqueName);
        protected static T Deserialize(string UniqueName)
        {

            string path = null;

            if (File.Exists(UniqueName))
            {
                path = UniqueName;
            }
            else
            {
                path = typeof(T).Name + UniqueName + ".xml";
            }


            if (!File.Exists(path)) throw new System.IO.FileNotFoundException(path);
            T serializedType = default(T);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                int tries = 0;
                while (IsFileLocked(path))
                {

                    System.Threading.Thread.Sleep(200);
                    if (tries++ > 10) throw new Exception("Cannot unlock " + path);
                }

                StreamReader reader = new StreamReader(path);
                serializedType = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return serializedType;

        }

        // Returns an instance of the type to be deserialized. 
        // To be paired with public static void Serialize();
        public static T Deserialize()
        {
            T serializedType = default(T);

            string path = typeof(T).Name + ".xml";

            if (!File.Exists(path))
            {
                return serializedType;
            }

            try
            {

                XmlSerializer serializer = new XmlSerializer(typeof(T));

                int tries = 0;

                while (IsFileLocked(path))
                {
                    System.Threading.Thread.Sleep(200);
                    if (tries++ > 10) throw new Exception("Cannot unlock " + path);
                }

                StreamReader reader = new StreamReader(path);
                serializedType = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return serializedType;

        }

        public static bool IsFileLocked(string FileName)
        {
            try
            {
                using (FileStream fs = File.Open(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    fs.Close();
                }

                return false;
                // The file is not locked
            }
            catch (Exception)
            {
                return true;
            }

        }

    }
}
