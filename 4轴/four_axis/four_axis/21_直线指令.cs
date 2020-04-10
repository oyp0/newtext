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
    public partial class _21_直线指令 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        public int MAXLINENUM=100;	//允许最大行数
	    public int LINESTART=30	;	//flash指令起始地址
	    public int LINESPACE=20	;	//行空间
        public float[] codespace = new float[2000];		//存放数组
        public int linenum;	//总行数，当前行号

        public _21_直线指令()
        {
            InitializeComponent();
        }


        //设置坐标
        private void deal_linesetpos(int num)
        {
            if (num == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    zmcaux.ZAux_Direct_SetDpos(g_handle, i, codespace[(linenum - 1) * LINESPACE + 5 + i]);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //
            //deal_linesetpos(int num);
        }

       
    }
}
