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
        public int PARANUM;           //轴参数空间
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数
        public int  flag_returnwindow=0;
        public String[] codetempspace = new String[2000];	//临时空间

        public int linenum = 1;	//总行数，当前行号

        public int pagenum;  //页数
        public int ONEPAGENUM = 5;	//每页文件数
        public int[] filetoflash = new int[15]; //id列表   
        public int[] showidlist = new int[5];	//显示ID	

        private _13参数设置 return_13参数设置 = null;
        private _15_轴参数设置 return_15_轴参数设置 = null;
        private _16_机械参数设置 return_16_机械参数设置 = null;
        private _18_IO映射 return_18_IO映射 = null;
        private _31_复位设置 return_31_复位设置 = null;
      
        private _14_文件管理 return_14_文件管理 = null;
        private _19_文件编辑 return_19_文件编辑 = null;
        private _20_运动类型选择 return_20_运动类型选择 = null;

        private _21_直线指令 return_21_直线指令 = null;
        private _22_三点圆弧指令 return_22_三点圆弧指令 = null;
        private _23_延时指令 return_23_延时指令 = null;
        private _24_输出指令_ return_24_输出指令_ = null;
        private _25_输出复位指令 return_25_输出复位指令 = null;
        private _26_圆心圆弧指令 return_26_圆心圆弧指令 = null;
        private _27_绝对模式 return_27_绝对模式 = null;
        private _28_相对模式 return_28_相对模式 = null;

        public _50未保存提示(_13参数设置 F13,_15_轴参数设置 F15,_16_机械参数设置 F16,_18_IO映射 F18,_31_复位设置 F31,_14_文件管理 F14,_19_文件编辑 F19,_20_运动类型选择 F20,_21_直线指令 F21,_22_三点圆弧指令 F22,_23_延时指令 F23,_24_输出指令_ F24,_25_输出复位指令 F25,_26_圆心圆弧指令 F26,_27_绝对模式 F27,_28_相对模式 F28)
        {
            InitializeComponent();
            this.return_13参数设置 = F13;
            this.return_15_轴参数设置 = F15;
            this.return_16_机械参数设置 = F16;
            this.return_18_IO映射 = F18;
            this.return_31_复位设置 = F31;
            this.return_14_文件管理 = F14;
            this.return_19_文件编辑 = F19;
            this.return_20_运动类型选择 = F20;
            this.return_21_直线指令 = F21;
            this.return_22_三点圆弧指令 = F22;
            this.return_23_延时指令 = F23;
            this.return_24_输出指令_ = F24;
            this.return_25_输出复位指令 = F25;
            this.return_26_圆心圆弧指令 = F26;
            this.return_27_绝对模式 = F27;
            this.return_28_相对模式 = F28;
        }

        private void _50未保存提示_Load(object sender, EventArgs e)
        {

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

                if (this.return_15_轴参数设置 != null && this.return_16_机械参数设置 == null && this.return_18_IO映射 == null && this.return_31_复位设置 == null)
                {
                    //上一级窗口关闭  
                    this.return_15_轴参数设置.Close();
                    //关闭当前窗口
                    this.Close();
                    this.return_13参数设置.paratemp = paratemp;
                    this.return_13参数设置.Visible = true;    
                }
                if (this.return_15_轴参数设置 == null && this.return_16_机械参数设置 != null && this.return_18_IO映射 == null && this.return_31_复位设置 == null)
                {
                    //上一级窗口关闭  
                    this.return_16_机械参数设置.Close();
                    //关闭当前窗口
                    this.Close();
                    this.return_13参数设置.paratemp = paratemp;
                    this.return_13参数设置.Visible = true;
                }
                if (this.return_15_轴参数设置 == null && this.return_16_机械参数设置 == null && this.return_18_IO映射 != null && this.return_31_复位设置 == null)
                {
                    //上一级窗口关闭  
                    this.return_18_IO映射.Close();
                    //关闭当前窗口
                    this.Close();
                    this.return_13参数设置.paratemp = paratemp;
                    this.return_13参数设置.Visible = true;
                }
                if (this.return_15_轴参数设置 == null && this.return_16_机械参数设置 == null && this.return_18_IO映射 == null && this.return_31_复位设置 != null)
                {
                    //上一级窗口关闭  
                    this.return_31_复位设置.Close();
                    //关闭当前窗口
                    this.Close();
                    this.return_13参数设置.paratemp = paratemp;
                    this.return_13参数设置.Visible = true;
                }


                             
            }
            else if (flag_returnwindow == 14) //文件
            {
                deal_fileflash(0);


                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_21_直线指令 == null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_21_直线指令 == null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_21_直线指令 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_21_直线指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_21_直线指令 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_21_直线指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }

                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_22_三点圆弧指令 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_22_三点圆弧指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_22_三点圆弧指令 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_22_三点圆弧指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }

                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_23_延时指令 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_23_延时指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_23_延时指令 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_23_延时指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }

                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_24_输出指令_ != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_24_输出指令_.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_24_输出指令_ != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_24_输出指令_.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }

                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_25_输出复位指令 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_25_输出复位指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_25_输出复位指令 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_25_输出复位指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }

                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_26_圆心圆弧指令 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_26_圆心圆弧指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_26_圆心圆弧指令 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_26_圆心圆弧指令.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }

                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_27_绝对模式 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_27_绝对模式.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_27_绝对模式 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_27_绝对模式.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }

                if (this.return_19_文件编辑 == null && this.return_20_运动类型选择 != null && this.return_28_相对模式 != null)
                {
                    this.Close();
                    this.return_20_运动类型选择.Close();
                    this.return_28_相对模式.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }
                if (this.return_19_文件编辑 != null && this.return_20_运动类型选择 == null && this.return_28_相对模式 != null)
                {
                    this.Close();
                    this.return_19_文件编辑.Close();
                    this.return_28_相对模式.Close();
                    this.return_14_文件管理.linenum = linenum;
                    this.return_14_文件管理.codetempspace = codetempspace;
                    this.return_14_文件管理.pagenum = pagenum;
                    this.return_14_文件管理.filetoflash = filetoflash;
                    this.return_14_文件管理.showidlist = showidlist;
                    this.return_14_文件管理.Visible = true;
                }


            }
            flag_returnwindow = 0;         
        }

        //否
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


      
    }
}
