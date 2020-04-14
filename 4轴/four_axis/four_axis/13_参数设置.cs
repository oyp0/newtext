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
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数
        public int flag_Initialization = 1; //初始化标志 只初始化一次

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
            if (g_handle != (IntPtr)0)
            {
                _15_轴参数设置 f15 = new _15_轴参数设置(this);
                f15.g_handle = g_handle;
                f15.vr = vr;
                f15.PARANUM = PARANUM;
                this.Hide();//隐藏现在这个窗口
                f15.Show();//新窗口显现    
            }
        }

        //机械参数
        private void button2_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _16_机械参数设置 f16 = new _16_机械参数设置(this);
                f16.g_handle = g_handle;
                f16.vr = vr;
                f16.PARANUM = PARANUM;
                this.Hide();//隐藏现在这个窗口
                f16.Show();//新窗口显现    
            }
        }
        
        //IO映射
        private void button3_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _18_IO映射 f18 = new _18_IO映射(this);
                f18.g_handle = g_handle;
                f18.vr = vr;
                f18.PARANUM = PARANUM;
                this.Hide();//隐藏现在这个窗口
                f18.Show();//新窗口显现    
            }

        }

        //回零设置
        private void button4_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _31_复位设置 f31 = new _31_复位设置(this);
                f31.g_handle = g_handle;
                f31.vr = vr;
                f31.PARANUM = PARANUM;
                this.Hide();//隐藏现在这个窗口
                f31.Show();//新窗口显现    
            }
        }

        //上传下载
        private void button5_Click(object sender, EventArgs e)
        {

        }

        //返回
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.flag_Initialization = flag_Initialization;
            this.return_10Start.paratemp = paratemp;
            this.return_10Start.Visible = true;     
        }

       
     
    }
}
