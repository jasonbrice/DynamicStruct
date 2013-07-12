using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

using DynamicStructBuilder.Serialization;

namespace DynamicStructBuilder.Reflection
{
    public class StructBuilder
    {
        private static Type structType;

        // Returns an object instance of the struct.
        public static object GetStructInstance() {
            object ptInstance = Activator.CreateInstance(GetStruct(), new object[] { });
            return ptInstance;
        }

        /**
         * The following method uses code adapted (ahem, borrowed) from Micrsoft's MSDN library: 
         * http://msdn.microsoft.com/en-us/library/system.reflection.emit.assemblybuilder.addresourcefile(v=vs.71).aspx
         */
        public static Type GetStruct() {

            // Unless the serialized instance is expected to change during runtime,
            // only deserialize & build the struct once. Otherwise, if the struct 
            // is expected to change during runtime, commment the next line out.
            if (structType != null) return structType;

            AppDomain myDomain = AppDomain.CurrentDomain;
            AssemblyName myAsmName = new AssemblyName("MyDynamicAssembly");

            // The RunAndSave option allows you to use the new struct
            // in memory, as well as save it out to a DLL for later
            // inspection/use.
            AssemblyBuilder myAsmBuilder =
               myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder structModule = myAsmBuilder.DefineDynamicModule("StructModule", "MyDynamicAssembly.dll");

            // Serializable is the key here if we ever want to save an instance of the struct off to an XML file.
            // Also, subclassing ValueType is key to it behaving as a struct.
            TypeBuilder structTypeBld = structModule.DefineType("DynamicStructBuilder.Reflection.DyanmicStruct", TypeAttributes.Public |
                TypeAttributes.Sealed | TypeAttributes.SequentialLayout |
                TypeAttributes.Serializable | TypeAttributes.AnsiClass, typeof(ValueType), PackingSize.Size1);


            StructFieldCollection collection = StructFieldCollection.Deserialize();
            foreach (StructField variable in collection.StructFields)
            {
                Console.WriteLine("StructField name: " + variable.Name + " DataType: " + variable.DataType);
                FieldBuilder field = structTypeBld.DefineField(variable.Name, Type.GetType(variable.DataType, true) , FieldAttributes.Public);
            }

            // Base class and base class constructor.
            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[] { });

            Type[] ctorParams = { };

            ConstructorBuilder pointCtor =
               structTypeBld.DefineConstructor(MethodAttributes.Public,
                                              CallingConventions.Standard, ctorParams);
            ILGenerator ctorIL = pointCtor.GetILGenerator();

            // Build the constructor. Begin by invoking the base class
            // constructor. The zero-index parameter of the constructor
            // is the new instance. Store the values of the fields.
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, objCtor);
            ctorIL.Emit(OpCodes.Ret);

           
            // Create the type, and then create an instance of the type 
            // (or not, doesn't hurt to comment next line out...)
            Type ptType = structTypeBld.CreateType();
            object ptInstance = Activator.CreateInstance(ptType, new object[] { });

            StructBuilder.structType = ptType;

            // save the newly created type to a DLL for use later
            // (or not, doesn't hurt to comment the next line out...)
            myAsmBuilder.Save("MyDynamicAssembly.dll");

            return ptType;
        }
    }
}
