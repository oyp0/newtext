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
        public int[] filelinepara  = new int [10];   //总行
        public int[] filelintempepara = new int[10];  
        public string codename;  //类型 
        public string linejump = "1";
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

        // 进入运动指令标志 
        public int change_21 = 0;
        public int change_22 = 0;
        public int change_23 = 0;
        public int change_24 = 0;
        public int change_25 = 0;
        public int change_26 = 0;
        public int change_27 = 0;
        public int change_28 = 0; 


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

            textBox6.Text = linejump.ToString();
        }

        //textbox
        public void deal_limit(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键  
            if (e.KeyChar == 0x08) e.KeyChar = (char)0;  //禁止退格键  
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数  
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }
        }


        //指令相关
        private void button7_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(linenum.ToString());
            //MessageBox.Show(filelinepara[0].ToString());
            if(linenum <= filelinepara[0])  //'未新建行时不允许操作
            {
                _20_运动类型选择 f20 = new _20_运动类型选择(this,this.return_14_文件管理);
                f20.g_handle = g_handle;
                f20.vr = vr;  //数组
                f20.codespace = codespace; //数组
                f20.manulradio = manulradio; //速度比例
                f20.linenum = linenum;  //行号
                f20.filelinepara = filelinepara;
                f20.filelintempepara = filelintempepara;
                f20.codename = codename;
                f20.codetempspace = codetempspace;
                f20.linejump = linejump;
                f20.pagenum = pagenum;
                f20.filetoflash = filetoflash;
                f20.showidlist = showidlist;
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
            textBox2.Text = filelinepara[0].ToString();   //总行数
            textBox5.Text = codename;
            button7.Text = codename;
        }

        //刷新显示
        private void show_code(int num)
        {
            if (num == 0)
            {
                codename = "无";
                //进入19界面
                MessageBox.Show("正在这个界面");
            }
            else if (num == 1)
            {
                if(change_21==0)
                {
                    change_21 = 1;
                    codename = "直线";
                    //21进入直线界面
                    _21_直线指令 f21 = new _21_直线指令(null,this,this.return_14_文件管理);
                    f21.g_handle = g_handle;
                    f21.vr = vr;  //数组
                    f21.codespace = codespace; //数组
                    f21.manulradio = manulradio; //速度比例
                    f21.linenum = linenum;  //行号
                    f21.filelinepara = filelinepara;
                    f21.filelintempepara = filelintempepara;
                    f21.codename = codename;
                    f21.codetempspace = codetempspace;
                    f21.linejump = linejump;
                    f21.pagenum = pagenum;
                    f21.filetoflash = filetoflash;
                    f21.showidlist = showidlist;
                    f21.change = change_21;
                    this.Hide();//隐藏现在这个窗口
                    f21.Show();//新窗口显现                       
                }
            }
            else if (num == 2)
            {
                MessageBox.Show(codetempspace[(linenum - 1) * LINESPACE + 5]);
                if (change_22 == 0)
                {
                    change_22 = 1;
                    codename = "三点画弧";
                    //22进入三点画弧
                    _22_三点圆弧指令 f22 = new _22_三点圆弧指令(null, this, this.return_14_文件管理);
                    f22.g_handle = g_handle;
                    f22.vr = vr;  //数组
                    f22.codespace = codespace; //数组
                    f22.manulradio = manulradio; //速度比例
                    f22.linenum = linenum;  //行号
                    f22.filelinepara = filelinepara;
                    f22.filelintempepara = filelintempepara;
                    f22.codename = codename;
                    f22.codetempspace = codetempspace;
                    f22.linejump = linejump;
                    f22.pagenum = pagenum;
                    f22.filetoflash = filetoflash;
                    f22.showidlist = showidlist;
                    f22.change = change_22;
                    this.Hide();//隐藏现在这个窗口
                    f22.Show();//新窗口显现                       
                }             
            }
            else if (num == 3)
            {
                if (change_23 == 0)
                {
                    change_23 = 1;
                    codename = "延时";
                    //23进入延时界面
                    _23_延时指令 f23 = new _23_延时指令(null, this, this.return_14_文件管理);
                    f23.g_handle = g_handle;
                    f23.vr = vr;  //数组
                    f23.codespace = codespace; //数组
                    f23.manulradio = manulradio; //速度比例
                    f23.linenum = linenum;  //行号
                    f23.filelinepara = filelinepara;
                    f23.filelintempepara = filelintempepara;
                    f23.codename = codename;
                    f23.codetempspace = codetempspace;
                    f23.linejump = linejump;
                    f23.pagenum = pagenum;
                    f23.filetoflash = filetoflash;
                    f23.showidlist = showidlist;
                    f23.change = change_23;
                    this.Hide();//隐藏现在这个窗口
                    f23.Show();//新窗口显现                       
                }             
               
            }
            else if (num == 4)
            {
                if (change_24 == 0)
                {
                    change_24 = 1;
                    codename = "多个输出";
                    //24进入多个输出界面
                    _24_输出指令_ f24 = new _24_输出指令_(null, this, this.return_14_文件管理);
                    f24.g_handle = g_handle;
                    f24.vr = vr;  //数组
                    f24.codespace = codespace; //数组
                    f24.manulradio = manulradio; //速度比例
                    f24.linenum = linenum;  //行号
                    f24.filelinepara = filelinepara;
                    f24.filelintempepara = filelintempepara;
                    f24.codename = codename;
                    f24.codetempspace = codetempspace;
                    f24.linejump = linejump;
                    f24.pagenum = pagenum;
                    f24.filetoflash = filetoflash;
                    f24.showidlist = showidlist;
                    f24.change = change_24;
                    this.Hide();//隐藏现在这个窗口
                    f24.Show();//新窗口显现                       
                }       
                
                
            }
            else if (num == 5)
            {
                if (change_25 == 0)
                {
                    change_25 = 1;
                    codename = "输出延时复位";
                    //25进入输出延时复位界面
                    _25_输出复位指令 f25 = new _25_输出复位指令(null, this, this.return_14_文件管理);
                    f25.g_handle = g_handle;
                    f25.vr = vr;  //数组
                    f25.codespace = codespace; //数组
                    f25.manulradio = manulradio; //速度比例
                    f25.linenum = linenum;  //行号
                    f25.filelinepara = filelinepara;
                    f25.filelintempepara = filelintempepara;
                    f25.codename = codename;
                    f25.codetempspace = codetempspace;
                    f25.linejump = linejump;
                    f25.pagenum = pagenum;
                    f25.filetoflash = filetoflash;
                    f25.showidlist = showidlist;
                    f25.change = change_25;
                    this.Hide();//隐藏现在这个窗口
                    f25.Show();//新窗口显现                       
                }       
  
            }
            else if (num == 6)
            {
                if (change_26 == 0)
                {
                    change_26 = 1;
                    codename = "圆心画弧";
                    //26进入圆心画弧界面
                    _26_圆心圆弧指令 f26 = new _26_圆心圆弧指令(null, this, this.return_14_文件管理);
                    f26.g_handle = g_handle;
                    f26.vr = vr;  //数组
                    f26.codespace = codespace; //数组
                    f26.manulradio = manulradio; //速度比例
                    f26.linenum = linenum;  //行号
                    f26.filelinepara = filelinepara;
                    f26.filelintempepara = filelintempepara;
                    f26.codename = codename;
                    f26.codetempspace = codetempspace;
                    f26.linejump = linejump;
                    f26.pagenum = pagenum;
                    f26.filetoflash = filetoflash;
                    f26.showidlist = showidlist;
                    f26.change = change_26;
                    this.Hide();//隐藏现在这个窗口
                    f26.Show();//新窗口显现                       
                }     
       
            }
            else if (num == 7)
            {
                if (change_27 == 0)
                {
                    change_27 = 1;
                    codename = "绝对模式";
                    //27进入绝对模式界面
                    _27_绝对模式 f27 = new _27_绝对模式(null, this, this.return_14_文件管理);
                    f27.g_handle = g_handle;
                    f27.vr = vr;  //数组
                    f27.codespace = codespace; //数组
                    f27.manulradio = manulradio; //速度比例
                    f27.linenum = linenum;  //行号
                    f27.filelinepara = filelinepara;
                    f27.filelintempepara = filelintempepara;
                    f27.codename = codename;
                    f27.codetempspace = codetempspace;
                    f27.linejump = linejump;
                    f27.pagenum = pagenum;
                    f27.filetoflash = filetoflash;
                    f27.showidlist = showidlist;
                    f27.change = change_27;
                    this.Hide();//隐藏现在这个窗口
                    f27.Show();//新窗口显现                       
                }              
            }
            else if (num == 8)
            {
                if (change_28 == 0)
                {
                    change_28 = 1;
                    codename = "相对模式";
                    //28进入相对模式界面
                    _28_相对模式 f28 = new _28_相对模式(null, this, this.return_14_文件管理);
                    f28.g_handle = g_handle;
                    f28.vr = vr;  //数组
                    f28.codespace = codespace; //数组
                    f28.manulradio = manulradio; //速度比例
                    f28.linenum = linenum;  //行号
                    f28.filelinepara = filelinepara;
                    f28.filelintempepara = filelintempepara;
                    f28.codename = codename;
                    f28.codetempspace = codetempspace;
                    f28.linejump = linejump;
                    f28.pagenum = pagenum;
                    f28.filetoflash = filetoflash;
                    f28.showidlist = showidlist;
                    f28.change = change_28;
                    this.Hide();//隐藏现在这个窗口
                    f28.Show();//新窗口显现                       
                }              
               
            }

        }
        
        //加载
        private void deal_lineload(int num)
        {
            codespace[(num - 1) * LINESPACE] = linejump;

            if(num<=filelinepara[0])
            {
                    templine[0] = int.Parse(codespace[(num - 1) * LINESPACE]);
                	//dmcpy templine(0),codespace((num-1)*LINESPACE),LINESPACE;	'复制到临时数组    
                	linenum=num;	//浏览界面跳转用
                    show_code(templine[0]);	//刷新显示
            }
            else if(num>filelinepara[0] && winnum==30)  //只有游览界面才提示
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
            if (int.Parse(linejump) <= filelinepara[0])
            {
                if (int.Parse(linejump) > 0)
                {
                   linenum = int.Parse(linejump);
                }
                else
                {
                    linejump = textBox6.Text;	//还原
                    linenum = 1;
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
            //flash_write filetoflash(filenum-1),fileflag,filename,filelinepara[0],codespace(0,MAXLINENUM*LINESPACE)	'保存
            //DMCPY codetempspace(0),codespace(0),MAXLINENUM*LINESPACE	'赋值到临时数组
            //dmcpy filelintempepara(0),filelinepara[0],10
            //DMCPY filejudname(0),filename(0),FILENAMELENG
            _51_保存成功提示 f51 = new _51_保存成功提示();
            f51.ShowDialog();   
        }

        private void deal_browseflash()
        {
            //flash_read filetoflash(filenum-1),fileflag,filename,filelinepara[0],codespace(0,MAXLINENUM*LINESPACE)		'读取
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
            for (int j = 0; j == 10; j++)
            {
                if (filelinepara[j] != filelintempepara[j])
                {
                    flag_change = 123;
                    break;
                }
                //for (int i = 0; i < FILENAMELENG; i++)  //文件名判断
                //{
                //    flag_change = 123;
                //    break;
                //}

                for(int i=0;i<filelinepara[0]*LINESPACE;i++)
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
                _50未保存提示 f50 = new _50未保存提示(null, null, null, null, null, this.return_14_文件管理,this,null,null,null,null,null,null,null,null,null);
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

        //linejump 的值改变
  
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            linejump = textBox6.Text;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }

    }
}