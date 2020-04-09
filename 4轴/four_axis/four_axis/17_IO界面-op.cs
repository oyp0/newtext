using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using cszmcaux;

namespace four_axis
{
    public partial class _17_界面_op : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public float[] vr = new float[500];  //数组

        private _10Start return_10Start = null;
        public _17_界面_op(_10Start F10)
        {
            InitializeComponent();
            this.return_10Start = F10;
        }

        private void _17_界面_op_Load(object sender, EventArgs e)
        {
        }

        //输入口
        private void button41_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _12_IO界面in f12 = new _12_IO界面in(return_10Start);
                f12.g_handle = g_handle;
                f12.vr = vr;
                this.Hide();//隐藏现在这个窗口
                f12.Show();//新窗口显现      
            }
        }

        //返回
        private void button42_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.Visible = true;
        }
    }
}
