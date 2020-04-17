using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using cszmcaux;

namespace four_axis
{
    public partial class Form1 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
        //搜索
        private void Search_Click(object sender, EventArgs e)
        {
            int i, num;
            string[] sArray;
            StringBuilder buffer = new StringBuilder(10240);
            string buff = "";
            zmcaux.ZAux_SearchEthlist(buffer, 10230, 200);

            buff += buffer;
            sArray = buff.Split(' ');
            num = buff.Split(' ').Length;

            C_Ip_Address.Items.Clear();

            C_Ip_Address.Items.Insert(0, "127.0.0.1");
            for (i = 0; i < num - 1; i++)
            {
                C_Ip_Address.Items.Insert(i + 1, sArray[i].ToString());
            }
        }

        private void Link_Click(object sender, EventArgs e)
        {

            zmcaux.ZAux_OpenEth(C_Ip_Address.Text, out g_handle);
            if (g_handle != (IntPtr)0)
            {              
                _10Start f10 = new _10Start(this);
                f10.g_handle = g_handle;
                this.Hide();//隐藏现在这个窗口
                f10.Show();//新窗口显现
            }
            else
            {
                zmcaux.ZAux_Close(g_handle);
                g_handle = (IntPtr)0;
            }
        }

        private void C_Ip_Address_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
