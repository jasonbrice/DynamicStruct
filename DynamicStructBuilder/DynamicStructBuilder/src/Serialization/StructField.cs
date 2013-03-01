using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicStructBuilder.Serialization
{
    // a very simple class to represent the bits of a struct field
    // we want to store in XML.
    public class StructField
    {
        // Field Name
        public string Name { get; set; }
        // Name of the type (e.g., System.Double)
        public string DataType { get; set; }

        // zero parameter constructor important for XML serialization!
        public StructField() { }

        public StructField(string Name, string DataType)
        {
            this.Name = Name;
            this.DataType = DataType;
        }

        
    }
}
