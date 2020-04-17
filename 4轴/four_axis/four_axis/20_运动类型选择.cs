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

        public int manulradio;		//初始速度比    还没传过来
        public float[] vr = new float[500];  //数组     

        public int linenum;	//总行数，当前行号  
        public int linetype;

        public int[] filelinepara = new int[10];   //总行
        public int[] filelintempepara = new int[10];
        public string codename;  //类型 
        public String[] codespace = new String[2000];	//存放数组
        public String[] codetempspace = new String[2000];	//临时空间

        public string linejump;
        public int[] templine = new int[30];	//临时
        int winnum = 0;    //窗口30要记得传值过来哦  

        public int pagenum;  //页数
        public int[] filetoflash = new int[15]; //id列表   
        public int[] showidlist = new int[5];	//显示ID

        // 进入运动指令标志 
        public int change21 = 0;
        public int change22 = 0;
        public int change23 = 0;
        public int change24 = 0;
        public int change25 = 0;
        public int change26 = 0;
        public int change27 = 0;
        public int change28 = 0; 

        private _19_文件编辑 return_19_文件编辑 = null;
        private _14_文件管理 return_14_文件管理 = null;
        public _20_运动类型选择(_19_文件编辑 F19,_14_文件管理 F14)
        {
            InitializeComponent();
            this.return_19_文件编辑 = F19;
            this.return_14_文件管理 = F14;
        }

        private void _20_运动类型选择_Load(object sender, EventArgs e)
        {

        }

        //private void show_code(int num)
        //{
        //    if (num == 0)
        //    {
        //        codename = "无";

        //        //进入19界面
        //    }
        //    else if (num == 1)
        //    {
        //        codename = "直线";

        //        //21进入直线界面          
        //    }
        //    else if (num == 2)
        //    {
        //        codename = "三点画弧";
        //        //22进入三点画弧
        //    }
        //    else if (num == 3)
        //    {
        //        codename = "延时";
        //        //23进入延时界面
        //    }
        //    else if (num == 4)
        //    {
        //        codename = "多个输出";
        //        //24进入多个输出界面
        //    }
        //    else if (num == 5)
        //    {
        //        codename = "输出延时复位";
        //        //25进入输出延时复位界面
        //    }
        //    else if (num == 6)
        //    {
        //        codename = "圆心画弧";
        //        //26进入圆心画弧界面
        //    }
        //    else if (num == 7)
        //    {
        //        codename = "绝对模式";
        //        //27进入绝对模式界面
        //    }
        //    else if (num == 8)
        //    {
        //        codename = "相对模式";
        //        //28进入相对模式界面
        //    }          
        //}


        //private void deal_lineload(int num)
        //{   
        //    //winnum=HMI_BASEWINDOW
        //    codespace[(num - 1) * LINESPACE] = linejump;
        //    if (num <= filelinepara[0])
        //    {
        //        templine[0] = int.Parse(codespace[(num - 1) * LINESPACE]);
        //        //dmcpy templine(0),codespace((num-1)*LINESPACE),LINESPACE;	//复制到临时数组   
        //        linenum = num; //游览界面跳转用
        //        show_code(templine[0]);  //刷新显示
        //    }
        //    else if ((num > filelinepara[0]) && winnum ==30)
        //    {
        //        _52_操作提示 f52 = new _52_操作提示();
        //        f52.V1 = "超过文件总行数";
        //        f52.ShowDialog();
        //    }
        //}

        //直线类型
        private void button1_Click(object sender, EventArgs e)
        {
            codespace[(linenum-1)*LINESPACE]="1";	 //直线类型
            codespace[(linenum-1)*LINESPACE+4]="80";	 //速度默认

            //直线窗口
            if (g_handle != (IntPtr)0)
            {
                if (change21 == 0)
                {
                    linenum = 1;
                    codename = "直线";
                    change21 = 1;
                    
                        _21_直线指令 f21 = new _21_直线指令(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                        f21.change = change21;
                        if (this.return_19_文件编辑 != null)
                        {
                            this.return_19_文件编辑.Close(); //关闭上一级窗口     
                        }                                        
                        this.Hide();//关闭这个窗口
                        f21.Show();//新窗口显现                    
                }
                else
                {
                    this.Close();
                }

             //   deal_lineload(linenum);
            }    
        }

        //三点圆弧
        private void button2_Click(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE] = "2";	 //圆弧类型
            codespace[(linenum - 1) * LINESPACE + 4] = "80";	 //速度默认

            //三点圆弧窗口
            if (change22 == 0)
            {
                linenum = 2;
                codename = "三点圆弧";
                change22 = 1;
               
                    _22_三点圆弧指令 f22 = new _22_三点圆弧指令(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f22.change = change22;
                    if (this.return_19_文件编辑 != null)
                    {
                        this.return_19_文件编辑.Close(); //关闭上一级窗口     
                    }                
                    this.Hide();//关闭这个窗口
                    f22.Show();//新窗口显现   
               
            }
            else
            {
                this.Close();
            }  

      //          deal_lineload(linenum);    
                 
        }
  
        //延时
        private void button6_Click(object sender, EventArgs e)
        {
               codespace[(linenum - 1) * LINESPACE] = "3";	 //延时
            //延时窗口
               if (g_handle != (IntPtr)0)
               {
                   if (change23 == 0)
                   {
                       linenum = 3;
                       codename = "延时";
                       change23 = 1;
                      
                           _23_延时指令 f23 = new _23_延时指令(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                           f23.change = change23;
                           if (this.return_19_文件编辑 != null)
                           {
                               this.return_19_文件编辑.Close(); //关闭上一级窗口     
                           }              
                           this.Hide();//关闭这个窗口
                           f23.Show();//新窗口显现    
                    
                   }
                   else
                   {
                       this.Close();
                   }

                   //           deal_lineload(linenum);
               }
        }

        //输出类型
        private void button7_Click(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE] = "4";	 //输出
            //输出窗口
            if (g_handle != (IntPtr)0)
            {
                if (change24 == 0)
                {
                    linenum = 4;
                    codename = "输出";
                    change24 = 1;
                   
                        _24_输出指令_ f24 = new _24_输出指令_(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                        f24.change = change24;
                        if (this.return_19_文件编辑 != null)
                        {
                            this.return_19_文件编辑.Close(); //关闭上一级窗口     
                        }                 
                        this.Hide();//关闭这个窗口
                        f24.Show();//新窗口显现    
                 
                }
                else
                {
                    this.Close();
                }

                //           deal_lineload(linenum);
            }
        }

        //输出复位类型
        private void button8_Click(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE] = "5";	 //输出复位
            //输出复位窗口
            if (g_handle != (IntPtr)0)
            {
                if (change25 == 0)
                {
                    linenum = 5;
                    codename = "输出复位";
                    change25 = 1;
                   
                        _25_输出复位指令 f25 = new _25_输出复位指令(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                        f25.change = change25;
                        if (this.return_19_文件编辑 != null)
                        {
                            this.return_19_文件编辑.Close(); //关闭上一级窗口     
                        }                   
                        this.Hide();//关闭这个窗口
                        f25.Show();//新窗口显现    
                 

                }
                else
                {
                    this.Close();
                }

                //           deal_lineload(linenum);
            }
        }

        //圆心圆弧
        private void button3_Click(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE] = "6";	 //圆弧类型
            codespace[(linenum - 1) * LINESPACE + 4] = "80";	 //速度默认
            //圆心圆弧窗口
            if (g_handle != (IntPtr)0)
            {
                if (change26 == 0)
                {
                    linenum = 6;
                    codename = "圆心圆弧";
                    change26 = 1;
                   
                        _26_圆心圆弧指令 f26 = new _26_圆心圆弧指令(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                        f26.change = change26;
                        if (this.return_19_文件编辑 != null)
                        {
                            this.return_19_文件编辑.Close(); //关闭上一级窗口     
                        }             
                        this.Hide();//关闭这个窗口
                        f26.Show();//新窗口显现    
                  

                }
                else
                {
                    this.Close();
                }

                // deal_lineload(linenum);
            }
        }

        //绝对模式
        private void button4_Click(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE] = "7";	 //绝对
            //绝对
            if (g_handle != (IntPtr)0)
            {
                if (change27 == 0)
                {
                    linenum = 7;
                    codename = "绝对模式";
                    change27 = 1;
                   
                        _27_绝对模式 f27 = new _27_绝对模式(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                        f27.change = change27;
                        if (this.return_19_文件编辑 != null)
                        {
                            this.return_19_文件编辑.Close(); //关闭上一级窗口     
                        }                 
                        this.Hide();//关闭这个窗口
                        f27.Show();//新窗口显现    
                   
                }
                else
                {
                    this.Close();
                }
                //deal_lineload(linenum);
            }
        }
    
        //相对模式
        private void button5_Click(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE] = "8";	 //绝对
            //相对
            if (g_handle != (IntPtr)0)
            {
                if (change28 == 0)
                {
                    linenum = 8;
                    codename = "相对模式";
                    change28 = 1;
                   
                        _28_相对模式 f28 = new _28_相对模式(this, this.return_19_文件编辑, this.return_14_文件管理);
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
                        f28.change = change28;
                        if (this.return_19_文件编辑 != null)
                        {
                            this.return_19_文件编辑.Close(); //关闭上一级窗口     
                        }
                        this.Hide();//关闭这个窗口
                        f28.Show();//新窗口显现    
                  
                }
                else
                {
                    this.Close();
                }
                //deal_lineload(linenum);
            }
        }

        //返回
        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }
   
    }
}
