using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DynamicStructBuilder.Serialization
{
    /**
     * This class acts as the "interim" class between 
     */
    [Serializable()]
    public class StructFieldCollection : GenericSerializable<StructFieldCollection>
    {
        public StructFieldCollection() { }

        [XmlArray("DataSet")]
        [XmlArrayItem("StructField", typeof(StructField))]
        public StructField[] Variables { get; set; }

        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Variables.Length; i++)
            {
                yield return Variables[i];
            }
        }

    }
}
