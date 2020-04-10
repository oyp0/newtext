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
    public partial class _20_运动类型选择 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        public int MAXLINENUM=100;	//允许最大行数
        public int LINESTART=30;		//flash指令起始地址
        public int LINESPACE=20;		//行空间
	
        
        public int[] codespace = new int[2000];		//存放数组
        public int[] codetempspace= new int[2000];	//临时空间
        public string  codename = "无" ;  //类型
        public int[] templine= new int[20];	//临时

        public int linenum;	//总行数，当前行号  
        public int linetype,linejump;

        public int filelinepara = 0;   //总行


        private _19_文件编辑 return_19_文件编辑 = null;
        public _20_运动类型选择(_19_文件编辑 F19)
        {
            InitializeComponent();
            this.return_19_文件编辑 = F19;
        }

        private void _20_运动类型选择_Load(object sender, EventArgs e)
        {

        }


        private void show_code(int num)
        {
            if (num == 0)
            {
                codename = "无";
                //进入19界面
            }
            else if (num == 1)
            {
                codename = "直线";
                //21进入直线界面
            }
            else if (num == 2)
            {
                codename = "三点画弧";
                //22进入三点画弧
            }
            else if (num == 3)
            {
                codename = "延时";
                //23进入延时界面
            }
            else if (num == 4)
            {
                codename = "多个输出";
                //24进入多个输出界面
            }
            else if (num == 5)
            {
                codename = "输出延时复位";
                //25进入输出延时复位界面
            }
            else if (num == 6)
            {
                codename = "圆心画弧";
                //26进入圆心画弧界面
            }
            else if (num == 7)
            {
                codename = "绝对模式";
                //27进入绝对模式界面
            }
            else if (num == 8)
            {
                codename = "相对模式";
                //28进入相对模式界面
            }
            
        }


        private void deal_lineload(int num)
        {
        //    int winnum = 0; //表示是哪个窗口跳转过来的
            //winnum=HMI_BASEWINDOW
            if (num <= filelinepara)
            { 
                //dmcpy templine(0),codespace((num-1)*LINESPACE),LINESPACE;	//复制到临时数组   
                linenum = num; //游览界面跳转用
                show_code(templine[0]);  //刷新显示
            }
            else if ((num > filelinepara))
            {
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "超过文件总行数";
                f52.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            codespace[(linenum-1)*LINESPACE]=1;	 //直线类型
            codespace[(linenum-1)*LINESPACE+4]=80;	 //速度默认

            //直线窗口

            deal_lineload(linenum);
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

      
    }
}
