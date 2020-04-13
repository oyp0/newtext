using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using cszmcaux;

namespace four_axis
{
    public partial class _10Start : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public int AXISMAX = 4;		//轴数
        public int AXISSPACE = 25;	    //每轴参数空间
        public int PARANUM;           //轴参数空间
        public float[] vr = new float[500];  //数组
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数
        public float f1;   //中间变量

        public int reg; //判断
        public int piValue;


        public int[] flag_error = new int[100];
        public int[] flag_erroshow = new int[100];
        public int[] zeroerror = new int[100];
        public int flag_erroryes = 0;
        public string errortemptext = "";



        public int stepmode = 0;   //步模式
        public int cyclemode = 0;  //周期模式
        public int steprun = 0;    //步执行
        public int yield = 0;          //产量
        public int cyclenum = 1;       //循环次数
   
        public int flag_state = 0;    //运动状态
        public int flag_home  = 0;     //回零状态
        public int filetempnum = 0;    //文件数
        public int runlinenum = 0;  //运行行号

        public int filelinepara = 0;   //总行

        public float[] table = new float[4];  //xyzu的坐标 

        public int manulradio = 100;		//初始速度比

        private Form1 returnForm1 = null;

        public _10Start(Form1 F1)
        {
            InitializeComponent();
            // 接受Form1对象
            this.returnForm1 = F1;
        }



        Thread td_run; 
        Thread td_home;
        public int f_run=0; //第一个进程的标志位    
        public int f_home=0;//第二个进程的标志位

        private void _10Start_Load(object sender, EventArgs e)
        {        
            timer1.Enabled = true;
            timer1.Interval = 100;
            timer1.Start();

            label12.Text = DateTime.Now.ToString("yyyy-MM-dd");        // 2008-09-04
           
            deal_setparainit();
            Initialization();
        }

        //textbox处理
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

        public void deal_setparainit()  //初始化参数
        {
            for (int i = 0; i < AXISMAX; i++)
            {
                vr[i * AXISSPACE + 0] = 0;                //起始速度
                vr[i * AXISSPACE + 1] = 100;              //运行速度
		        vr[i * AXISSPACE + 2] = 1000;             //加速度
                vr[i * AXISSPACE + 3] = 1000;             //减速度
                vr[i * AXISSPACE + 4] = 0;                //S曲线
                vr[i * AXISSPACE + 5] = 100;              //复位速度 
                vr[i * AXISSPACE + 6] = 10;               //反找速度
                vr[i * AXISSPACE + 7] = 80;               //示教速度
                vr[i * AXISSPACE + 8] = 3;                //回零方式
                vr[i * AXISSPACE + 9] = 100;              //反找等待
                vr[i * AXISSPACE + 10] = 100;             //每圈脉冲数 
                vr[i * AXISSPACE + 11] = 1;               //螺距

                vr[i * AXISSPACE + 12] = -1000;          //负向软限位 
                vr[i * AXISSPACE + 13] = 1000;           //正向软限位

                vr[i * AXISSPACE + 14] = -1;             //原点IN
                vr[i * AXISSPACE + 15] = 0;              //反转

                vr[i * AXISSPACE + 16] = -1;             //正限位IN  
                vr[i * AXISSPACE + 17] = 0;              //反转

                vr[i * AXISSPACE + 18] = -1;             //负限位IN 
                vr[i * AXISSPACE + 19] = 0;              //反转

                vr[i * AXISSPACE + 20] = -1;             //报警IN
                vr[i * AXISSPACE + 21] = 0;              //反转 

                vr[i * AXISSPACE + 22] = 0;              //使能OP 
                vr[i * AXISSPACE + 23] = 0;              //报警清除op
                vr[i * AXISSPACE + 24] = 0;              //预留
            }
        }

        private void Initialization()
        {
            PARANUM = (AXISMAX + 2) * AXISSPACE;  //轴参数空间
            textBox12.Text = cyclenum.ToString();
        }
        //循环次数改变
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            cyclenum = int.Parse(textBox12.Text);
        }
        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        } 
       

        private void button9_Click(object sender, EventArgs e)
        {
            if (cyclemode % 2 == 0)
            {
                button9.Text = "无限循环";
                cyclemode = 1;
            }
            else
            {
                button9.Text = "无限循环";
                cyclemode = 0;
            }         
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (stepmode % 2 == 0)
            {
                button10.Text = "单步";
                stepmode = 1;
            }
            else
            {
                button10.Text = "持续";
                stepmode = 0;
            }         
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (steprun % 2 == 0)
            {
                button11.Text = "执行";
                steprun = 1;
            }
            else
            {
                button11.Text = "执行";
                steprun = 0;
            }         
        }

        //启动
        private void button5_Click(object sender, EventArgs e)
        {
            if (flag_state == 0)              //启动
            {
                if (flag_home == 1)          //回零完成
                {
                    if (filetempnum != 0)
                    {
                        for (int i = 0; i < AXISMAX; i++)
                        {
                            f1 = vr[i * AXISSPACE + 1];
                            zmcaux.ZAux_Direct_SetSpeed(g_handle, i, f1);   //还原速度                                                
                        }

                        //执行任务1
                        //td_run = new Thread(task_run);
                        //if ((td_run.ThreadState & ThreadState.Unstarted) != 0 && f_run ==0)
                        //{
                        //    td_run.Start(); //启动线程
                        //    f_run = 1;  //标志启动了线程1
                        //}
                        timer2.Enabled = true;
                        timer2.Interval = 100;
                        timer2.Start();
                    }
                    else
                    {
                        _52_操作提示 f52 = new _52_操作提示();
                        f52.V1 = "未选择文件";
                        f52.ShowDialog();
                    }
                }
                else
                {
                    _52_操作提示 f52 = new _52_操作提示();
                    f52.V1 = "机台复位未完成";
                    f52.ShowDialog();                   
                }
            }
            else if (flag_state == 2)       //恢复
            {
                //恢复任务1   RESUMETASK 1
                //if ((td_run.ThreadState & ThreadState.Suspended) != 0 && f_run == 1)
                //{
                //    td_run.Resume(); //恢复线程  
                //}
                //轴选择      base(0)
                //运动恢复	  MOVE_RESUME            
		        statename.Text ="运行中：执行";  //状态显示
                flag_state = 1;   //运行
            }
            else
            {
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "已在运行中";
                f52.ShowDialog();             
            }
        }

        //暂停
        private void button6_Click(object sender, EventArgs e)
        {
            if (flag_state != 0)
            {
                //暂停任务1  PAUSETASK 1
                //if ((td_run.ThreadState & ThreadState.WaitSleepJoin) != 0 && f_run == 1)
                //{
                //    td_run.Suspend(); //休眠线程
                //}
                //轴选择      base(0)
                //运动暂停	  move_pause(0) 
                statename.Text = "运行中：暂停";  //状态显示
                flag_state=2;	 //暂停
            }
        }

        //停止运动
        private void deal_stop()
        {
            // 停止任务1   stoptask 1
            //if ((td_run.ThreadState & ThreadState.WaitSleepJoin) != 0 && f_run == 1)
            //{
            //    td_run.Abort(); //终止线程  
            //    f_run = 0; //标志终止了线程1
            //} 
            timer2.Enabled = false;
            timer2.Stop();
            // 停止任务2   stoptask 2	
            //if ((td_home.ThreadState & ThreadState.WaitSleepJoin) != 0 && f_home == 1)
            //{
            //    td_home.Abort(); //终止线程  
            //    f_home = 0;  //标志终止了线程2
            //} 
            timer3.Enabled = false;
            timer3.Stop();
            zmcaux.ZAux_Direct_Rapidstop(g_handle, 2);  //轴全部停止
            flag_state = 0;	 //停止
            statename.Text = "停止";
            runlinenum = 0;
        }

        //停止
        private void button7_Click(object sender, EventArgs e)
        {
            deal_stop();
        }

        //回零
        private void button8_Click(object sender, EventArgs e)
        {
            if (flag_state == 0)
            {
                //执行任务2
                //td_home = new Thread(task_home);
                //if ((td_home.ThreadState & ThreadState.Unstarted) != 0 && f_home == 0)
                //{
                //    td_home.Start(); //启动线程
                //    f_home = 1; //标志启动了线程2
                //}
                timer3.Enabled = true;
                timer3.Interval = 100;
                timer3.Start();
            }
            else
            {
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "运行中无法操作";
                f52.ShowDialog();                  
            }
        }

        //手动操作
        private void button1_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _11_手动操作 f11 = new _11_手动操作(this);
                f11.g_handle = g_handle;
                f11.vr = vr;
                this.Hide();//隐藏现在这个窗口
                f11.Show();//新窗口显现         
            }
        }

        //I/O测试
        private void button2_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _12_IO界面in f12 = new _12_IO界面in(this);
                f12.g_handle = g_handle;
                f12.vr = vr;
                this.Hide();//隐藏现在这个窗口
                f12.Show();//新窗口显现  
            }
        }

        //参数设置
        private void button3_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _13参数设置 f13 = new _13参数设置(this);
                f13.g_handle = g_handle;
                f13.vr = vr;
                f13.PARANUM = PARANUM;
                this.Hide();//隐藏现在这个窗口
                f13.Show();//新窗口显现    
            }
        }

        //文件管理
        private void button4_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _14_文件管理 f14 = new _14_文件管理(this);
                f14.g_handle = g_handle;
                f14.filetempnum = filetempnum;   
                f14.filelinepara = filetempnum;   //总行
                f14.manulradio = manulradio;
                f14.vr = vr;
                this.Hide();//隐藏现在这个窗口
                f14.Show();//新窗口显现    
            }
        }

        //文件选择
        private void button13_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _33_文件选择 f33 = new _33_文件选择(this);
                f33.g_handle = g_handle;
                f33.runlinenum = runlinenum;
                f33.filetempnum = filetempnum;
                this.Hide();//隐藏现在这个窗口
                f33.Show();//新窗口显现    
            }
        }

        //清除
        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AXISMAX; i++)
            {
                //move_op2(vr(i*AXISSPACE+23),on,1000)axis(6)	//清除报警
                zmcaux.ZAux_Direct_Single_Datum(g_handle, i, 0);
            }
        }
        
        //任务1
        private void task_run()
        { 
            flag_state=1;      //运行
            //Move_Mark指令
            zmcaux.ZAux_Direct_SetMovemark(g_handle, 0, 0);
            zmcaux.ZAux_Direct_SetMovemark(g_handle, 1, 0);
            zmcaux.ZAux_Direct_SetMovemark(g_handle, 2, 0);
            zmcaux.ZAux_Direct_SetMovemark(g_handle, 3, 0);

            while (cyclenum >= 0 || cyclenum == -1)  //重复循环
            {
                runlinenum = 1;	 //首行
                while (runlinenum <= filelinepara)        //单次程序运行
                {
                    //ZINDEX_CALL(runindex(codespace((runlinenum-1)*LINESPACE+0)))(runlinenum)	'文件类型跳转
                    runlinenum = runlinenum + 1;
                }
                //等待轴停止  还没写
                yield = yield + 1;   //产量加一
		        cyclenum=cyclenum-1; //循环次数减一
                if (cyclenum==1)
                {
                    cyclenum = -1;   //无限循环
                }
            }
            flag_state=0;	//停止
	        statename.Text="运行完成";
	        cyclenum=1;          //循环次数
        }

        //任务2
        private void task_home()
        {
            statename.Text = "复位中";
            for (int i = 0; i < AXISMAX; i++)
            {
                f1 = vr[i * AXISSPACE + 5];
                zmcaux.ZAux_Direct_SetSpeed(g_handle, i, f1);   //回零速度   
                //f1 = vr[i * AXISSPACE + 9];
                //zmcaux.ZAux_Direct_SetHomeWait(g_handle, i, f1);  //反找等待
                //f1 = vr[i * AXISSPACE + 8];
                //zmcaux.ZAux_Direct_Single_Datum(g_handle, i, f1); //回零方式
            }
            //等待轴停止  还没写
            flag_home=1;	 //回零完成
	        statename.Text ="复位完成";
        }

        //任务3
        private void task_error()
        {
            while (true)
            {
                //轴0
                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 0, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 21).ToString());  //22位           
               // if (read_bit2(22, axisstatus(0)))
                if (reg == 1)
                {
                    flag_error[0] = 1;		//报警标志
                    deal_stop();  //停止运动
                    if (flag_erroshow[0] != 231) //只加一次
                    {
                        errortemptext = errortemptext + "," + "轴0伺服报警";
                        flag_erroshow[0] = 231;
                        Console.WriteLine("轴0伺服报警");
                    }
                }
                else
                { 
                    flag_erroshow[0]=0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0伺服报警")		'报警取消时删除
                    num = errortemptext.indexof(",轴0伺服报警");
                    if (num != -1)
                    { 
                       // DMDEL  errortemptext(num,num+11)	'只删除一次
                    }
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 0, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 3).ToString());  //4位
                if (reg == 1)
                {
                    flag_error[1] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[1] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0正限位报警";
                        flag_erroshow[1] = 231;
                        Console.WriteLine("轴0正限位报警");
                    }

                }
                else
                {
                    flag_erroshow[1] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0正限位报警")		
                    num = errortemptext.indexof(",轴0正限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }   
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 0, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 4).ToString());  //5位
                if (reg == 1)
                {
                    flag_error[2] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[2] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0负限位报警";
                        flag_erroshow[2] = 231;
                        Console.WriteLine("轴0负限位报警");
                    }

                }
                else
                {
                    flag_erroshow[2] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0负限位报警")		
                    num = errortemptext.indexof(",轴0负限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }
                }

                

                //轴1
                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 1, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 21).ToString());  //22位
                // if (read_bit2(22, axisstatus(0)))
                if (reg == 1)
                {
                    flag_error[5] = 1;		//报警标志
                    deal_stop();  //停止运动
                    if (flag_erroshow[5] != 231) //只加一次
                    {
                        errortemptext = errortemptext + "," + "轴0伺服报警";
                        flag_erroshow[5] = 231;
                        Console.WriteLine("轴0伺服报警");
                    }
                }
                else
                {
                    flag_erroshow[5] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0伺服报警")		'报警取消时删除
                    num = errortemptext.indexof(",轴0伺服报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+11)	'只删除一次
                    }
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 1, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 3).ToString());  //4位
                if (reg == 1)
                {
                    flag_error[6] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[6] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0正限位报警";
                        flag_erroshow[6] = 231;
                        Console.WriteLine("轴0正限位报警");
                    }

                }
                else
                {
                    flag_erroshow[6] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0正限位报警")		
                    num = errortemptext.indexof(",轴0正限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 1, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 4).ToString());  //5位
                if (reg == 1)
                {
                    flag_error[7] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[7] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0负限位报警";
                        flag_erroshow[7] = 231;
                        Console.WriteLine("轴0负限位报警");
                    }

                }
                else
                {
                    flag_erroshow[7] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0负限位报警")		
                    num = errortemptext.indexof(",轴0负限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }
                }


                //轴2
                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 2, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 21).ToString());  //22位
                // if (read_bit2(22, axisstatus(0)))
                if (reg == 1)
                {
                    flag_error[10] = 1;		//报警标志
                    deal_stop();  //停止运动
                    if (flag_erroshow[10] != 231) //只加一次
                    {
                        errortemptext = errortemptext + "," + "轴0伺服报警";
                        flag_erroshow[10] = 231;
                        Console.WriteLine("轴0伺服报警");
                    }
                }
                else
                {
                    flag_erroshow[10] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0伺服报警")		'报警取消时删除
                    num = errortemptext.indexof(",轴0伺服报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+11)	'只删除一次
                    }
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 2, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 3).ToString());  //4位
                if (reg == 1)
                {
                    flag_error[11] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[11] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0正限位报警";
                        flag_erroshow[11] = 231;
                        Console.WriteLine("轴0正限位报警");
                    }

                }
                else
                {
                    flag_erroshow[11] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0正限位报警")		
                    num = errortemptext.indexof(",轴0正限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 2, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 4).ToString());  //5位
                if (reg == 1)
                {
                    flag_error[12] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[12] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0负限位报警";
                        flag_erroshow[12] = 231;
                        Console.WriteLine("轴0负限位报警");
                    }

                }
                else
                {
                    flag_erroshow[12] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0负限位报警")		
                    num = errortemptext.indexof(",轴0负限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }
                }


                //轴3
                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 3, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 21).ToString());  //22位
                // if (read_bit2(22, axisstatus(0)))
                if (reg == 1)
                {
                    flag_error[15] = 1;		//报警标志
                    deal_stop();  //停止运动
                    if (flag_erroshow[15] != 231) //只加一次
                    {
                        errortemptext = errortemptext + "," + "轴0伺服报警";
                        flag_erroshow[15] = 231;
                        Console.WriteLine("轴0伺服报警");
                    }
                }
                else
                {
                    flag_erroshow[15] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0伺服报警")		'报警取消时删除
                    num = errortemptext.indexof(",轴0伺服报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+11)	'只删除一次
                    }
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 3, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 3).ToString());  //4位
                if (reg == 1)
                {
                    flag_error[16] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[16] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0正限位报警";
                        flag_erroshow[16] = 231;
                        Console.WriteLine("轴0正限位报警");
                    }

                }
                else
                {
                    flag_erroshow[16] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0正限位报警")		
                    num = errortemptext.indexof(",轴0正限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }
                }

                zmcaux.ZAux_Direct_GetAxisStatus(g_handle, 3, ref piValue);
                reg = piValue & int.Parse(Math.Pow(2, 4).ToString());  //5位
                if (reg == 1)
                {
                    flag_error[17] = 1;
                    deal_stop();  //停止运动
                    if (flag_erroshow[17] != 231)
                    {
                        errortemptext = errortemptext + "," + "轴0负限位报警";
                        flag_erroshow[17] = 231;
                        Console.WriteLine("轴0负限位报警");
                    }

                }
                else
                {
                    flag_erroshow[17] = 0;	//清除显示标志
                    int num;
                    //num = STRFIND(errortemptext,",轴0负限位报警")		
                    num = errortemptext.indexof(",轴0负限位报警");
                    if (num != -1)
                    {
                        // DMDEL  errortemptext(num,num+13)	'只刷新一次
                    }
                }

            }
        }

        //定时器1  
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                label11.Text = DateTime.Now.ToString("hh:mm:ss");  

                textBox3.Text = filetempnum.ToString();
                button13.Text = filetempnum.ToString();
                textBox5.Text = (runlinenum - 1).ToString();
                textBox6.Text = filelinepara.ToString();
                textBox7.Text = yield.ToString();

                int iret = 0;
                float[] fDpos = new float[4];
                iret = zmcaux.ZAux_Direct_GetAllAxisPara(g_handle, "dpos", 4, table);

                textBox8.Text = table[0].ToString();
                textBox9.Text = table[1].ToString();
                textBox10.Text = table[2].ToString();
                textBox11.Text = table[3].ToString();
            }
        }

        //定时器2 
        private void timer2_Tick(object sender, EventArgs e)
        {
            task_run();
        }

        //定时器3
        private void timer3_Tick(object sender, EventArgs e)
        {
            task_home();
        }

       

    }
}
