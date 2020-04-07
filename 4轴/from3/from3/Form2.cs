using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace from3
{

    public delegate void ChangeHandleText(string str);//定义委托

    public partial class Form2 : Form
    {

        public event ChangeHandleText changetext; //定义事件

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (changetext != null)//判断事件是否为空
            {
                changetext(textBox1.Text);//执行委托实例 
            }      
        }
    }
}
