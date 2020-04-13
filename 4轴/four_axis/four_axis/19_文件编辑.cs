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

        public int linenum=1;	//总行数，当前行号  
        public int filelinepara = 0;   //总行
        public string codename;  //类型 
        public int  linejump = 1;
        public int filenum;  //文件数

        int winnum = 0;    //窗口30要记得传值过来哦  
        public int[] templine = new int[30];	//临时
        public int LINESPACE=20;		//行空间

        public int FILENAMELENG = 20; //文件名长度
        public int   flag_returnwindow=0;	//参数修改标志

        public String[] codespace = new String[2000];	//存放数组
        public String[] codetempspace = new String[2000];	//临时空间

        public int pagenum;  //页数
        public int ONEPAGENUM = 5;	//每页文件数
        public int[] filetoflash = new int[15]; //id列表   
        public int[] showidlist = new int[5];	//显示ID	

        public float[] vr = new float[500];  //数组
        public int PARANUM;           //轴参数空间
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数

        public int manulradio;		//初始速度比

        int flag_change;

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
            //MessageBox.Show(linenum.ToString());
            //MessageBox.Show(filelinepara.ToString());
            if(linenum <= filelinepara)  //'未新建行时不允许操作
            {
                _20_运动类型选择 f20 = new _20_运动类型选择(this);
                f20.g_handle = g_handle;
                f20.codename = codename;
                f20.codespace = codespace;
                f20.linenum = linenum;
                f20.manulradio =manulradio;		//初始速度比
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
                    templine[0] = int.Parse(codespace[(num - 1) * LINESPACE]);
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

        //文件操作
        private void deal_fileflash(int num)
        {
            //FLASH_WRITE 20,firstflag,filetoflash,totalfilenum	'读文件列表,当前文件数	
            int t0, t1;
            t0 = (pagenum - 1) * ONEPAGENUM;
            t1 = pagenum * ONEPAGENUM;
            for (int i = t0; i < t1; i++)
            {
                if (filetoflash[i] != -1)
                {
                    // flash_read filetoflash(i),fileflag,filename			'读取标志判断是否有存储				
                    // DMCPY ZINDEX_ARRAY(shownamelist(i mod ONEPAGENUM ))(0),filename(0),FILENAMELENG	

                    showidlist[i % ONEPAGENUM] = i + 1;	//显示ID
                    //MessageBox.Show(showidlist[i%ONEPAGENUM].ToString());
                }
                else
                {
                    //DMCPY ZINDEX_ARRAY(shownamelist(i mod ONEPAGENUM ))(0),zeroname(0),FILENAMELENG
                    showidlist[i % ONEPAGENUM] = 0;	//显示ID
                    //MessageBox.Show(showidlist[i%ONEPAGENUM].ToString());
                }
            }
        }

        //退出
        private void button10_Click(object sender, EventArgs e)
        {
            flag_returnwindow=14;	//文件配方界面
            for (int j = 0; j == 0; j++)
            { 
                //if(filelinepara[j] !=filelintempepara[j])
                //{
                //     flag_change=123;
                //        break;
                //}
                for (int i = 0; i < FILENAMELENG;i++)  //文件名判断
                {
                    flag_change = 123;
                    break;
                }

                for(int i=0;i<filelinepara*LINESPACE;i++)
                {
                    if(codespace[i]!=codetempspace[i])  //参数不同
                    {
                        flag_change = 123;
                        break;
                    }
                }                      
            }

            if (flag_change == 123)   //有改动
            {
                _50未保存提示 f50 = new _50未保存提示(null, null, null, null, null, this.return_14_文件管理,this);
                f50.g_handle = g_handle;   //句柄
                //f50.vr = vr;               //存放数组
                //f50.paratemp = paratemp;   //临时数组             
                //f50.PARANUM = PARANUM;  //轴参数空间
                f50.pagenum = pagenum;
                f50.filetoflash = filetoflash;
                f50.showidlist = showidlist;
                f50.flag_returnwindow = flag_returnwindow;  //窗口选择
                f50.Show();//新窗口显现     
            }
            else if (flag_returnwindow == 14)
            {
                this.Close();
                this.return_14_文件管理.filetoflash = filetoflash;
                this.return_14_文件管理.showidlist = showidlist;
                this.return_14_文件管理.Visible = true;
            }

        }

        //上一行
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
