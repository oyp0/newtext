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
    public partial class _19_文件编辑 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        public int linenum=0;	//总行数，当前行号  
        public int filelinepara = 0;   //总行
        public string codename;  //类型 
        public int  linejump = 1;
        public int filenum;  //文件数

        int winnum = 0;    //窗口30要记得传值过来哦  
        public int[] templine = new int[30];	//临时


        public int   flag_returnwindow=0;	//参数修改标志


        //////////////游览操作///
         public int BROWSEPAGEGNUM=5;
	     public int[] browseshowid = new int[5];
         public int  browsepage=1;



        private _14_文件管理 return_14_文件管理 = null;
        public _19_文件编辑(_14_文件管理 F14)
        {
            InitializeComponent();
            this.return_14_文件管理 = F14;
        }

        private void _19_文件编辑_Load(object sender, EventArgs e)
        {
            //开启定时器
            timer1.Enabled = true;
            timer1.Interval = 100;
            timer1.Start();
        }

        //指令相关
        private void button7_Click(object sender, EventArgs e)
        {
            if(linenum <= filelinepara)  //'未新建行时不允许操作
            {
                _20_运动类型选择 f20 = new _20_运动类型选择(this);
                f20.g_handle = g_handle;
                f20.codename = codename;
                this.Hide();//隐藏现在这个窗口
                f20.Show();//新窗口显现        
            }
            else
            {
                 Console.WriteLine("新建行");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "先插入行";
                f52.ShowDialog();
            }
        }

        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = linenum.ToString();    //当前行 
            textBox2.Text = filelinepara.ToString();   //总行数
            textBox6.Text = linejump.ToString();
        }

        //刷新显示
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
        
        //加载
        private void deal_lineload(int num)
        {
            if(num<=filelinepara)
            {
                	//dmcpy templine(0),codespace((num-1)*LINESPACE),LINESPACE;	'复制到临时数组    
                	linenum=num;	//浏览界面跳转用
                    show_code(templine[0]);	//刷新显示
            }
            else if(num>filelinepara && winnum==30)  //只有游览界面才提示
            {
                Console.WriteLine("超过文件总行数");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "超过文件总行数";
                f52.ShowDialog();
            }
          
        }

        //跳转
        private void button13_Click(object sender, EventArgs e)
        {
            if (linejump <= filelinepara)
            {
                if(linejump>0)
                {
                    linenum  =linejump;
                }
                else
                {
                    linejump=1;	//还原
			        linenum=1;
                }
                //wa(10)
                deal_lineload(linenum);
            }
            else
            {
                Console.WriteLine("超出范围");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "超出范围";
                f52.ShowDialog();   
            }

        }

        //保存
        private void button5_Click(object sender, EventArgs e)
        {
            //flash_write filetoflash(filenum-1),fileflag,filename,filelinepara,codespace(0,MAXLINENUM*LINESPACE)	'保存
            //DMCPY codetempspace(0),codespace(0),MAXLINENUM*LINESPACE	'赋值到临时数组
            //dmcpy filelintempepara(0),filelinepara(0),10
            //DMCPY filejudname(0),filename(0),FILENAMELENG
            _51_保存成功提示 f51 = new _51_保存成功提示();
            f51.ShowDialog();   
        }

        private void deal_browseflash()
        {
            //flash_read filetoflash(filenum-1),fileflag,filename,filelinepara,codespace(0,MAXLINENUM*LINESPACE)		'读取
	        //'DMCPY codetempspace(0),codespace(0),MAXLINENUM*LINESPACE	'赋值到临时数组
        }

        //游览
        private void button6_Click(object sender, EventArgs e)
        {
            if(filenum > 0)
            {
                browsepage=1;
                deal_browseflash();
                //HMI_SHOWWINDOW(30,0)
            }
            else
            {
                 Console.WriteLine("未选择ID");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "未选择文件ID";
                f52.ShowDialog();     
            }
            
        }
   

        private void button10_Click(object sender, EventArgs e)
        {
            int flag_change;
            flag_returnwindow=14;	//文件配方界面

        }
    }
}
