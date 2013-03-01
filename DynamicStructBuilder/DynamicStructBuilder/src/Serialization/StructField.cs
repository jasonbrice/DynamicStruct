using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicStructBuilder.Serialization
{
    public class StructField
    {
    
        public StructField() { }

        public StructField(string Name, string Type)
        {
            this.Name = Name;
            this.SystemDataType = Type;
        }

        public string Name { get; set; }
        public string SystemDataType { get; set; }

    }
}
