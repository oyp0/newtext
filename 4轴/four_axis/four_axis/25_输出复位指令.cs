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
    public partial class _25_输出复位指令 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public String[] codespace = new String[2000];	//存放数组
        public int linenum;	//总行数，当前行号
        public int LINESPACE = 20;	//行空间

        private _20_运动类型选择 return_20_运动类型选择 = null;
        public _25_输出复位指令(_20_运动类型选择 F20)
        {
            InitializeComponent();
            this.return_20_运动类型选择 = F20;
        }

        private void _25_输出复位指令_Load(object sender, EventArgs e)
        {

        }

        //textbox
        public void deal_limit(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键  
            if (e.KeyChar == 0x08) e.KeyChar = (char)0;  //禁止退格键  
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数  
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }
        }

        //op口编号
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 5] = textBox1.Text;
        }
        //状态
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 6] = textBox2.Text;
        }
        //延时改变ms
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 7] = textBox3.Text;
        }

        //只能输入数字
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }


    }

}
