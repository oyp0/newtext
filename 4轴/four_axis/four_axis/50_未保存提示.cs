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
        public int PARANUM=4;           //轴参数空间
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数
        public int  flag_returnwindow=0;

        public _50未保存提示()
        {
            InitializeComponent();
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
                //_13参数设置 f13 = new _13参数设置();
                //f13.g_handle = g_handle;  //句柄
                //f13.vr = vr;              //数组
                //f13.PARANUM = PARANUM;    //轴参数空间
                //f13.Show();
            }
            else if (flag_returnwindow == 14) //文件
            {
                // deal_fileflash(0);
            }
            flag_returnwindow = 0;
            this.Hide();           
        }

        //否
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


      
    }
}
