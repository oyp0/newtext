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
    public partial class _13参数设置 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public float[] vr = new float[500];  //数组
        public int PARANUM;           //轴参数空间

        private _10Start return_10Start = null;
        public _13参数设置(_10Start F10)
        {
            InitializeComponent();
            this.return_10Start = F10;
        }

        private void _13参数设置_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        //轴参数
        private void button1_Click(object sender, EventArgs e)
        {
            _15_轴参数设置 f15 = new _15_轴参数设置(this);
            f15.g_handle = g_handle;
            f15.vr = vr;
            f15.PARANUM = PARANUM;
            this.Hide();//隐藏现在这个窗口
            f15.Show();//新窗口显现     
        }

        //机械参数
        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;


            //新建一个修改密码的窗体
            _16_机械参数设置 frm = new _16_机械参数设置();
            //将窗体的TopLevel属性设为false，即窗体显示不是顶级窗口
            frm.TopLevel = false;
            //清空panel控件的内容
            this.panel1.Controls.Clear();

            frm.g_handle = g_handle;
            frm.vr = vr;
            frm.PARANUM = PARANUM;

            //给Form去边框
            frm.FormBorderStyle = FormBorderStyle.None;
            //向panel控件中添加修改密码的窗体
            this.panel1.Controls.Add(frm);
            frm.WindowState = FormWindowState.Maximized;//如果windowState设置为最大化，添加到tabPage中时，winform不会显示出来
            //panel控件内显示窗体
            frm.Show();
        }

        //返回
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.Visible = true;     
        }

        
    }
}
