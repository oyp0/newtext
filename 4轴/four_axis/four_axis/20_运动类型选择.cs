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

        public int manulradio = 100;		//初始速度比    还没传过来
        public float[] codespace = new float[2000];		//存放数组
        public int[] codetempspace= new int[2000];	//临时空间
        public string  codename = "无" ;  //类型
        public int[] templine= new int[20];	//临时
        public float[] vr = new float[500];  //数组     

        public int linenum;	//总行数，当前行号    还没传过来 文件编辑界面
        public int linetype,linejump;

        public int winnum = 0; //窗口30的进来标志

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

                _21_直线指令 f21 = new _21_直线指令(this);
                f21.g_handle = g_handle;  //句柄
                f21.vr = vr;  //数组
                f21.codespace = codespace; //数组
                f21.manulradio = manulradio; //速度比例
                f21.linenum = linenum;  //行号
                this.Hide();//隐藏现在这个窗口
                f21.Show();//新窗口显现       
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
            //winnum=HMI_BASEWINDOW
            if (num <= filelinepara)
            { 
                //dmcpy templine(0),codespace((num-1)*LINESPACE),LINESPACE;	//复制到临时数组   
                linenum = num; //游览界面跳转用
                show_code(templine[0]);  //刷新显示
            }
            else if ((num > filelinepara) && winnum ==30)
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
            if (g_handle != (IntPtr)0)
            {
                _21_直线指令 f21 = new _21_直线指令(this);
                f21.g_handle = g_handle;  //句柄
                f21.vr = vr;  //数组
                f21.codespace = codespace; //数组
                f21.manulradio = manulradio; //速度比例
                f21.linenum = linenum;  //行号
                this.Hide();//隐藏现在这个窗口
                f21.Show();//新窗口显现     

                deal_lineload(linenum);
            }

           
        }   
    }
}
