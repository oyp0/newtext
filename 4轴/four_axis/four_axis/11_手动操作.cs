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
    public partial class _11_手动操作 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public int AXISMAX = 4;		//轴数
        public int AXISSPACE = 25;	    //每轴参数空间
        public float f1;   //中间变量
        public float[] vr = new float[500];  //数组

        public int flag_Initialization = 1; //初始化标志 只初始化一次

        public int[] manulmode = new int[4];  //模式
        public float[] manulpos = new float[4];   //位置

	    public int manulradio;		//初始速度比

        public int iresult;      //返回值

        private _10Start return_10Start = null;
        public _11_手动操作(_10Start F10)
        {
            InitializeComponent();
            this.return_10Start = F10;
        }

        private void _11_手动操作_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                manulmode[0] = 0;   //   相对/绝对
                manulpos[0] = 0;
            }
            manulradio = int.Parse(textBox9.Text);

            timer1.Enabled = true;    //定时器使能
            timer1.Interval = 100;    //定时器定时100ms
        }

        //选择处理
        public String Choice(ref int temp, String str)
        {
            if (temp% 2 == 0)
            {
                temp = 1;
                return str = "绝对";
                
            }
            else
            {
                temp = 0;
                return  str = "相对";
                
            }
        }


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

        public void deal_manulrun(int axisnum)  //轴号
        {
            if (g_handle != (IntPtr)0)
            {
                f1 = vr[axisnum * AXISSPACE + 7] * (manulradio / 100);
                zmcaux.ZAux_Direct_SetSpeed(g_handle, axisnum, f1);   //示教速度
                if (manulmode[axisnum] == 0)
                {
                    zmcaux.ZAux_Direct_Single_Move(g_handle, axisnum, manulpos[axisnum]);  //相对运动
                }
                else
                {
                    zmcaux.ZAux_Direct_Single_MoveAbs(g_handle, axisnum, manulpos[axisnum]); //绝对运动       
                }
                //MessageBox.Show("运行");
            }
        }

        public void deal_limit(object sender,KeyPressEventArgs e)
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

        //X-
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(0, -1);  
        }
        private void button1_MouseUp(object sender, MouseEventArgs e) 
        {
            deal_manulstop(0);
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

        //Y-
        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(1, -1);  
        }
        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(1);
        }

        //Y+
        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(1, 1);  
        }
        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(1);
        }

        //Z-
        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(2, -1);  
        }
        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(2);
        }

        //Z+
        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(2, 1);  
        }
        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(2);
        }

        //U-
        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(3, -1);  
        }
        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(3);
        }

        //U+
        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            deal_manul(3, 1);  
        }
        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            deal_manulstop(3);
        }


        //相对和绝对的选择
        private void button9_Click(object sender, EventArgs e)
        {            
            button9.Text = Choice(ref manulmode[0], button9.Text);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            button10.Text = Choice(ref manulmode[1], button10.Text);     
        }
        private void button11_Click(object sender, EventArgs e)
        {
            button11.Text = Choice(ref manulmode[2], button11.Text);     
        }
        private void button12_Click(object sender, EventArgs e)
        {
            button12.Text = Choice(ref manulmode[3], button12.Text);     
        }

        //运行
        private void button13_Click(object sender, EventArgs e)
        {
            deal_manulrun(0);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            deal_manulrun(1);
        }
        private void button15_Click(object sender, EventArgs e)
        {
            deal_manulrun(2);
        }
        private void button16_Click(object sender, EventArgs e)
        {
            deal_manulrun(3);
        }

        //位置改变
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            manulpos[0] = float.Parse(textBox1.Text);
            //MessageBox.Show(manulpos[0].ToString());   //测试textbox控件值改变
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            manulpos[1] = float.Parse(textBox2.Text);
            //MessageBox.Show(manulpos[1].ToString());   //测试textbox控件值改变
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            manulpos[2] = float.Parse(textBox3.Text);
            //MessageBox.Show(manulpos[2].ToString());   //测试textbox控件值改变
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            manulpos[3] = float.Parse(textBox4.Text);
            //MessageBox.Show(manulpos[3].ToString());   //测试textbox控件值改变
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

        //速度比例改变
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //只输入0~100之间的数
            try
            {
                string strNum = textBox9.Text;
                if ("" == strNum || null == strNum)
                {
                    return;
                }
                int num = int.Parse(textBox9.Text);
               
                textBox9.Text = num.ToString();
                if (num <= 100)
                {
                    manulradio = int.Parse(textBox9.Text);
                    //MessageBox.Show(manulradio.ToString());
                    return;
                }
                else
                {
                    textBox9.Text = textBox9.Text.Remove(2);
                    textBox9.SelectionStart = textBox9.Text.Length;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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

        //设为原点
        private void button17_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                for (int i = 0; i < AXISMAX; i++)
                {
                    zmcaux.ZAux_Direct_SetDpos(g_handle, i, 0);
                    zmcaux.ZAux_Direct_SetMpos(g_handle, i, 0);
                }
                //MessageBox.Show("设为原点");
            }
        }

        //停止
        private void button18_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                iresult = zmcaux.ZAux_Direct_Rapidstop(g_handle, 2);
                if (iresult != 0)
                {
                    iresult = zmcaux.ZAux_Direct_Rapidstop(g_handle, 2);
                }	
                //MessageBox.Show("停止");
            }
        }

        //返回
        private void button19_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.manulradio = manulradio;
            this.return_10Start.flag_Initialization = flag_Initialization;
            this.return_10Start.vr = vr;
            this.return_10Start.Visible = true;
        }

     

        
    }
}
