using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynamicStructTestImpl
{
    /** 
     * This form tests whether the DynamicStructBuilder.Reflection.StructBuilder class
     * can successfully create a struct based on the XML provided in StructFieldCollection.xml.
     **/
    public partial class DynamicStructViewer : Form
    {
        public DynamicStructViewer()
        {
            InitializeComponent();
        }

        // User has clicked the "Does it work?" button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // This code asks the StructBuilder to create a struct based on the XML supplied. 
                // If you want to create an object instance of the struct instead of just the type,
                // call  DynamicStructBuilder.Reflection.StructBuilder.GetStructInstance() {
                Type t = DynamicStructBuilder.Reflection.StructBuilder.GetStruct();
                this.textBox1.Text = "Yes! You have a new struct:" + Environment.NewLine + Environment.NewLine;
                // Show the values in the struct.
                foreach (System.Reflection.FieldInfo fi in t.GetFields()) {
                    this.textBox1.Text += fi.Name + " is a " + fi.FieldType + Environment.NewLine;
                }
                this.textBox1.Text += Environment.NewLine 
                    + "(Now why don't you try playing with StructFieldCollection.xml and see if you can break it...)";
            }
            // Display any errors. For fun, try intentionally introducing an XML sytax error in 
            // StructFieldCollection.xml, and see if the XML parser can tell you what's wrong...
            catch (Exception ex) {
                this.textBox1.Text = "Nope, there appears to be a problem:"
                    + Environment.NewLine
                    + ex.Message
                    + ex.StackTrace;
            }
        }
    }
}
