using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DynamicStructBuilder.Serialization
{

    //Provide a Type for GenericSerializable to seriialize/deserialize
    [Serializable()]
    public class StructFieldCollection : GenericSerializable<StructFieldCollection>
    {
        public StructFieldCollection() { }

        [XmlArray("DataSet")]
        [XmlArrayItem("StructField", typeof(StructField))]
        public StructField[] StructFields { get; set; }

        // if you're not using Linq but you want to iterate
        // through the fields using a foreach loop... 
        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < StructFields.Length; i++)
            {
                yield return StructFields[i];
            }
        }

    }
}
