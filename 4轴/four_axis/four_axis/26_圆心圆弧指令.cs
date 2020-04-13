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
    public partial class _26_圆心圆弧指令 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        public int iresult; //返回值
        public float f1;   //中间变量
        public float[] vr = new float[500];  //数组     
        public int manulradio;		//初始速度比  
        public int AXISSPACE = 25;	    //每轴参数空间
        public String[] codespace = new String[2000];	//存放数组
        public int linenum;	//总行数，当前行号
        public int LINESPACE = 20;	//行空间

        private _20_运动类型选择 return_20_运动类型选择 = null;
        public _26_圆心圆弧指令(_20_运动类型选择 F20)
        {
            InitializeComponent();
            this.return_20_运动类型选择 = F20;
        }

        private void _26圆心圆弧指令_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;    //定时器使能
            timer1.Interval = 100;    //定时器定时100ms
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
        private void textBox1_TextChanged(object sender, EventArgs e)//X坐标  终点坐标
        {
            codespace[(linenum - 1) * LINESPACE + 5] = textBox1.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)//Y坐标  终点坐标
        {
            codespace[(linenum - 1) * LINESPACE + 6] = textBox2.Text;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)//Z坐标  终点坐标
        {
            codespace[(linenum - 1) * LINESPACE + 7] = textBox3.Text;
        }
        private void textBox10_TextChanged(object sender, EventArgs e)//X坐标  圆心坐标
        {
            codespace[(linenum - 1) * LINESPACE + 8] = textBox10.Text;
        }
        private void textBox11_TextChanged(object sender, EventArgs e)//Y坐标  圆心坐标
        {
            codespace[(linenum - 1) * LINESPACE + 9] = textBox11.Text;
        }
        private void textBox12_TextChanged(object sender, EventArgs e)//Z坐标  圆心坐标
        {
            codespace[(linenum - 1) * LINESPACE + 10] = textBox12.Text;
        }
        private void textBox4_TextChanged(object sender, EventArgs e)//U坐标  
        {
            codespace[(linenum - 1) * LINESPACE + 11] = textBox4.Text;
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
        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }


        //速度改变
        private void textBox9_TextChanged(object sender, EventArgs e)//速度
        {
            codespace[(linenum - 1) * LINESPACE + 4] = textBox9.Text;
        }
        //只能输入数字
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
