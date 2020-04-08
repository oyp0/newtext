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
            _16_机械参数设置 f16 = new _16_机械参数设置(this);
            f16.g_handle = g_handle;
            f16.vr = vr;
            f16.PARANUM = PARANUM;
            this.Hide();//隐藏现在这个窗口
            f16.Show();//新窗口显现    
        }

        //返回
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.Visible = true;     
        }

        
    }
}
