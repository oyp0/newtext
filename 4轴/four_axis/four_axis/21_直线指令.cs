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
        public int AXISSPACE = 25;	    //每轴参数空间
        public float f1;   //中间变量
        public int iresult; //返回值

        public float[] codespace = new float[2000];		//存放数组
        public int linenum;	//总行数，当前行号
        public float[] vr = new float[500];  //数组     
        public int manulradio = 100;		//初始速度比  
   

        
        private _20_运动类型选择 return_20_运动类型选择 = null;        
        public _21_直线指令(_20_运动类型选择 F20)
        {
            InitializeComponent();
            this.return_20_运动类型选择 = F20;
        }

        private void _21_直线指令_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;    //定时器使能
            timer1.Interval = 100;    //定时器定时100ms
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

        //设置坐标
        private void button9_Click(object sender, EventArgs e)
        {

        }


        //运动
        public void deal_manul(int axisnum, int dir)  //轴号，方向
        {
            if (g_handle != (IntPtr)0)
            {
                //如果任务1和任务2停止
                //if PROC_STATUS(1)=0 and PROC_STATUS(2)=0 ;  
                f1 = vr[axisnum * AXISSPACE + 7] * (manulradio / 100);
                zmcaux.ZAux_Direct_SetSpeed(g_handle, axisnum, f1);   //示教速度
                zmcaux.ZAux_Direct_Single_Vmove(g_handle, axisnum, dir);
            }
        }

        //停止
        public void deal_manulstop(int axisnum)  //轴号
        {
            if (g_handle != (IntPtr)0)
            {
                //如果任务1和任务2停止
                //if PROC_STATUS(1)=0 and PROC_STATUS(2)=0 ;  
                iresult = zmcaux.ZAux_Direct_Single_Cancel(g_handle, axisnum, 2);
                if (iresult != 0)
                {
                    iresult = zmcaux.ZAux_Direct_Single_Cancel(g_handle, axisnum, 2);
                }
            }
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

        //X+
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(0, 1);  
        }
        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0);
        }

        //X-
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(0, -1);  
        }
        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0); 
        }

        //Y+
        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(1, 1);  
        }
        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0); 
        }

        //Y-
        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(1, -1);       
        }
        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0); 
        }

        //Z+
        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(2, 1); 
        }
        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0); 
        }

        //Z-
        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(2, -1); 
        }
        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0); 
        }

        //U+
        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(3, 1); 
        }
        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0); 
        }

        //U-
        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(3, -1); 
        }
        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(0); 
        }

        //位置改变
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 5] = float.Parse(textBox1.Text);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 6] = float.Parse(textBox2.Text);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 7] = float.Parse(textBox3.Text);
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 8] = float.Parse(textBox4.Text);
        }

        //只能输入数字
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }


        //速度改变
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            codespace[(linenum - 1) * LINESPACE + 4] = float.Parse(textBox9.Text);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }

        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                int iret = 0;
                float[] fDpos = new float[4];
                iret = zmcaux.ZAux_Direct_GetAllAxisPara(g_handle, "dpos", 4, fDpos);
                //当前坐标  
                textBox5.Text = fDpos[0].ToString();
                textBox6.Text = fDpos[1].ToString();
                textBox7.Text = fDpos[2].ToString();
                textBox8.Text = fDpos[3].ToString();
            }
        }

       
  


  


       
    }
}
