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
    public partial class _50未保存提示 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public float[] vr = new float[500]; //数组
        public int PARANUM;           //轴参数空间
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数
        public int  flag_returnwindow=0;

        private _13参数设置 return_13参数设置 = null;
        private _15_轴参数设置 return_15_轴参数设置 = null;
        public _50未保存提示(_13参数设置 F13,_15_轴参数设置 F15)
        {
            InitializeComponent();
            this.return_13参数设置 = F13;
            this.return_15_轴参数设置 = F15;
        }


        private void _50未保存提示_Load(object sender, EventArgs e)
        {

        }
        

        //是
        private void button1_Click(object sender, EventArgs e)
        {
            if (flag_returnwindow == 13)
            {
                for (int i = 0; i < PARANUM; i++)
                {
                    paratemp[i] = vr[i];	//还原显示
                }
               
                //DMCPY vr(0),paratemp(0),PARANUM  '还原修改,用了vr就不能dmcpy
                //返回参数设置
                //上一级窗口关闭               
                this.return_15_轴参数设置.Close();
                //关闭当前窗口
                this.Close();
                this.return_13参数设置.Visible = true;                
            }
            else if (flag_returnwindow == 14) //文件
            {
                // deal_fileflash(0);
            }
            flag_returnwindow = 0;         
        }

        //否
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


      
    }
}
