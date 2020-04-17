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
    public partial class _27_绝对模式 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        public int LINESPACE = 20;	//行空间
        public int linenum;	//总行数，当前行号
        public float[] vr = new float[500];  //数组     
        public int manulradio;		//初始速度比  

        public int flag_returnwindow = 0;	//参数修改标志
        int flag_change;
        public int FILENAMELENG = 20; //文件名长度


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

        public int change; // 接收运动指令标志 

        private _20_运动类型选择 return_20_运动类型选择 = null;
        private _19_文件编辑 return_19_文件编辑 = null;
        private _14_文件管理 return_14_文件管理 = null;
        public _27_绝对模式(_20_运动类型选择 F20, _19_文件编辑 F19, _14_文件管理 F14)
        {
            InitializeComponent();
            this.return_20_运动类型选择 = F20;
            this.return_19_文件编辑 = F19;
            this.return_14_文件管理 = F14;
        }
 
        private void _27_绝对模式_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;    //定时器使能
            timer1.Interval = 100;    //定时器定时100ms
            timer1.Start();

            textBox6.Text = linejump;     
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
            if (linenum <= filelinepara[0])  //'未新建行时不允许操作
            {
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _20_运动类型选择 f20 = new _20_运动类型选择(this.return_19_文件编辑, this.return_14_文件管理,null,null,null,null,null,null,this,null);
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
                    f20.change27 = change;
                    f20.Show();//新窗口显现        
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {
                    this.return_20_运动类型选择.Close();
                    _20_运动类型选择 f20 = new _20_运动类型选择(null, this.return_14_文件管理,null,null,null,null,null,null,this,null);
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
                    f20.change27 = change;
                    f20.Show();//新窗口显现
                } 
            }
            else
            {
                Console.WriteLine("新建行");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "先插入行";
                f52.ShowDialog();
            }
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
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _21_直线指令 f21 = new _21_直线指令(null, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f21.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f21.Show();//新窗口显现       
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {
                    _21_直线指令 f21 = new _21_直线指令(this.return_20_运动类型选择, null, this.return_14_文件管理);
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
                    f21.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f21.Show();//新窗口显现       
                }
            }
            else if (num == 2)
            {
                codename = "三点画弧";
                //22进入三点画弧
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _22_三点圆弧指令 f22 = new _22_三点圆弧指令(null, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f22.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f22.Show();//新窗口显现     
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {
                    _22_三点圆弧指令 f22 = new _22_三点圆弧指令(this.return_20_运动类型选择, null, this.return_14_文件管理);
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
                    f22.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f22.Show();//新窗口显现     
                }
            }
            else if (num == 3)
            {
                codename = "延时";
                //23进入延时界面
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _23_延时指令 f23 = new _23_延时指令(null, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f23.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f23.Show();//新窗口显现    
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {

                    _23_延时指令 f23 = new _23_延时指令(this.return_20_运动类型选择, null, this.return_14_文件管理);
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
                    f23.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f23.Show();//新窗口显现          
                }             
            }
            else if (num == 4)
            {
                codename = "多个输出";
                //24进入多个输出界面
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _24_输出指令_ f24 = new _24_输出指令_(null, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f24.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f24.Show();//新窗口显现        
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {
                    _24_输出指令_ f24 = new _24_输出指令_(this.return_20_运动类型选择, null, this.return_14_文件管理);
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
                    f24.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f24.Show();//新窗口显现        
                }
            }
            else if (num == 5)
            {
                codename = "输出延时复位";
                //25进入输出延时复位界面
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _25_输出复位指令 f25 = new _25_输出复位指令(null, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f25.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f25.Show();//新窗口显现      
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {
                    _25_输出复位指令 f25 = new _25_输出复位指令(this.return_20_运动类型选择, null, this.return_14_文件管理);
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
                    f25.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f25.Show();//新窗口显现      
                }
            }
            else if (num == 6)
            {
                codename = "圆心画弧";
                //26进入圆心画弧界面
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _26_圆心圆弧指令 f26 = new _26_圆心圆弧指令(null, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f26.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f26.Show();//新窗口显现      
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {
                    _26_圆心圆弧指令 f26 = new _26_圆心圆弧指令(this.return_20_运动类型选择, null, this.return_14_文件管理);
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
                    f26.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f26.Show();//新窗口显现     
                }
            }
            else if (num == 7)
            {
                codename = "绝对模式";
                //27进入绝对模式界面
                MessageBox.Show("正在这个界面");
            }
            else if (num == 8)
            {
                codename = "相对模式";
                //28进入相对模式界面
                if (this.return_20_运动类型选择 == null && this.return_19_文件编辑 != null && this.return_14_文件管理 != null)
                {
                    _28_相对模式 f28 = new _28_相对模式(null, this.return_19_文件编辑, this.return_14_文件管理);
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
                    f28.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f28.Show();//新窗口显现            
                }
                if (this.return_20_运动类型选择 != null && this.return_19_文件编辑 == null && this.return_14_文件管理 != null)
                {
                    _28_相对模式 f28 = new _28_相对模式(this.return_20_运动类型选择, null, this.return_14_文件管理);
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
                    f28.change = 1;
                    this.Close();//隐藏现在这个窗口
                    f28.Show();//新窗口显现            
                }
            }
        }

        //加载
        private void deal_lineload(int num)
        {
            codespace[(num - 1) * LINESPACE] = linejump;
            if (num <= filelinepara[0])
            {
                templine[0] = int.Parse(codespace[(num - 1) * LINESPACE]);
                //dmcpy templine(0),codespace((num-1)*LINESPACE),LINESPACE;	'复制到临时数组    
                linenum = num;	//浏览界面跳转用
                show_code(templine[0]);	//刷新显示
            }
            else if (num > filelinepara[0] && winnum == 30)  //只有游览界面才提示
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
            linejump = textBox6.Text;
            MessageBox.Show(linejump);
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

        //linejump 的值改变
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            linejump = textBox6.Text;  
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }

        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = linenum.ToString();    //当前行 
            textBox2.Text = filelinepara[0].ToString();   //总行数
            textBox5.Text = codename;
            button7.Text = codename;
        }

        //保存
        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 2000; i++)
            {
                codetempspace[i] = codespace[i];
            }
            _51_保存成功提示 f51 = new _51_保存成功提示();
            f51.ShowDialog();    
        }

        //退出
        private void button10_Click(object sender, EventArgs e)
        {
            flag_returnwindow = 14;	//文件配方界面
            for (int j = 0; j == 10; j++)
            {
                if (filelinepara[j] != filelintempepara[j])
                {
                    flag_change = 123;
                    break;
                }
                for (int i = 0; i < FILENAMELENG; i++)  //文件名判断
                {
                    flag_change = 123;
                    break;
                }

                for (int i = 0; i < 2000; i++)
                {
                    if (codespace[i] != codetempspace[i])  //参数不同
                    {
                        flag_change = 123;
                        break;
                    }
                }
            }

            if (flag_change == 123)   //有改动
            {
                _50未保存提示 f50 = new _50未保存提示(null, null, null, null, null, this.return_14_文件管理, this.return_19_文件编辑, this.return_20_运动类型选择, null, null, null, null, null, null,this,null);
                f50.g_handle = g_handle;   //句柄
                //f50.vr = vr;               //存放数组
                //f50.paratemp = paratemp;   //临时数组             
                //f50.PARANUM = PARANUM;  //轴参数空间
                f50.pagenum = pagenum;
                f50.linenum = linenum;
                f50.codetempspace = codetempspace;
                f50.filetoflash = filetoflash;
                f50.showidlist = showidlist;
                f50.flag_returnwindow = flag_returnwindow;  //窗口选择
                f50.Show();//新窗口显现     
            }
            else if (flag_returnwindow == 14)
            {
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_14_文件管理 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_14_文件管理 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                } 
            }
        }
    }
}
