using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace four_axis
{
   
    public partial class _52_操作提示 : Form
    {
        public String V1;

        public _52_操作提示()
        {
            InitializeComponent();
        }

        private void _52_操作提示_Load(object sender, EventArgs e)
        {
            textBox1.Text=V1;
        }

        //确认
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
