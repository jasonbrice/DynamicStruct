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
    public partial class DynamicStructViewer : Form
    {
        public DynamicStructViewer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Type t = DynamicStructBuilder.Reflection.StructBuilder.GetStruct();
                this.textBox1.Text = "Yes! You have a new struct:" + Environment.NewLine + Environment.NewLine;
                foreach (System.Reflection.FieldInfo fi in t.GetFields()) {
                    this.textBox1.Text += fi.Name + " is a " + fi.FieldType + Environment.NewLine;
                }
            }
            catch (Exception ex) {
                this.textBox1.Text = "Nope, there appears to be a problem:"
                    + Environment.NewLine
                    + ex.Message
                    + ex.StackTrace;
            }
        }
    }
}
