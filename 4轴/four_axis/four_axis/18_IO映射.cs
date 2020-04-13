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
    public partial class _18_IO映射 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public float[] vr = new float[500]; //数组
        public int PARANUM;           //轴参数空间
        public int AXISMAX = 4;		//轴数
        public int AXISSPACE = 25;	    //每轴参数空间
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数

        public int fileflag;        //读写判断
        public int firstflag = 0;

        public int flag_setchange = 0;     //参数修改标志
        public int flag_returnwindow = 0;
        public int flag_change;

        private _13参数设置 return_13参数设置 = null;
        public _18_IO映射(_13参数设置 F13)
        {
            InitializeComponent();
            this.return_13参数设置 = F13;
        }

        private void _18_IO映射_Load(object sender, EventArgs e)
        {
            Initialization();
        }

        //初始化参数
        public void Initialization()
        {
            //一进入界面就存放在一个临时的数组中
            for (int i = 0; i < PARANUM; i++)
            {
                paratemp[i] = vr[i];
            }

            textBox1.Text = vr[14].ToString();
            textBox2.Text = vr[16].ToString();
            textBox3.Text = vr[18].ToString();
            textBox4.Text = vr[20].ToString();
            textBox5.Text = vr[22].ToString();
            textBox6.Text = vr[23].ToString();

            textBox7.Text = vr[39].ToString();
            textBox8.Text = vr[41].ToString();
            textBox9.Text = vr[43].ToString();
            textBox10.Text = vr[45].ToString();
            textBox11.Text = vr[47].ToString();
            textBox12.Text = vr[48].ToString();

            textBox13.Text = vr[64].ToString();
            textBox14.Text = vr[66].ToString();
            textBox15.Text = vr[68].ToString();
            textBox16.Text = vr[70].ToString();
            textBox17.Text = vr[72].ToString();
            textBox18.Text = vr[73].ToString();

            textBox19.Text = vr[89].ToString();
            textBox20.Text = vr[91].ToString();
            textBox21.Text = vr[93].ToString();
            textBox22.Text = vr[95].ToString();
            textBox23.Text = vr[97].ToString();
            textBox24.Text = vr[98].ToString();

            paratemp[15] = vr[15];
            paratemp[17] = vr[17];
            paratemp[19] = vr[19];
            paratemp[21] = vr[21];

            paratemp[40] = vr[40];
            paratemp[42] = vr[42];
            paratemp[44] = vr[44];
            paratemp[46] = vr[46];

            paratemp[65] = vr[65];
            paratemp[67] = vr[67];
            paratemp[69] = vr[69];
            paratemp[61] = vr[61];

            paratemp[90] = vr[90];
            paratemp[92] = vr[92];
            paratemp[94] = vr[94];
            paratemp[96] = vr[96];

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

        //选择处理
        public String Choice(ref float temp, String str)
        {
            if (temp % 2 == 0)
            {
               temp = 1;
               return str = "on";
            }
            else
            {
               temp = 0;
               return str = "off";               
            }     
        }
             

        public void deal_setpara()
        {
            for (int i = 0; i < AXISMAX; i++)
            {
                zmcaux.ZAux_Direct_SetAtype(g_handle, i, 1);  //设置轴的类型
                zmcaux.ZAux_Direct_SetUnits(g_handle, i, vr[i * AXISSPACE + 10] / vr[i * AXISSPACE + 11]);   //设置轴的脉冲当量
                zmcaux.ZAux_Direct_SetLspeed(g_handle, i, vr[i * AXISSPACE + 0]); //设置轴的起始速度
                zmcaux.ZAux_Direct_SetSpeed(g_handle, i, vr[i * AXISSPACE + 1]);  //设置轴的运行速度
                zmcaux.ZAux_Direct_SetAccel(g_handle, i, vr[i * AXISSPACE + 2]);  //设置轴的加速度
                zmcaux.ZAux_Direct_SetDecel(g_handle, i, vr[i * AXISSPACE + 3]);  //设置轴的减速度
                zmcaux.ZAux_Direct_SetSramp(g_handle, i, vr[i * AXISSPACE + 4]);  //设置轴的S曲线时间
                zmcaux.ZAux_Direct_SetCreep(g_handle, i, vr[i * AXISSPACE + 6]);  //设置轴的反找速度

                // RS_LIMIT=vr(i*AXISSPACE+12)   //设置轴的负向软限位
                // FS_LIMIT=vr(i*AXISSPACE+13)   //设置轴的正向软限位

                if (vr[i * AXISSPACE + 14] < 0)  //原点IN 
                {
                    // datum_in=-1    
                }
                else
                {
                    //原点设置及反转
                    // datum_in=vr(i*AXISSPACE+14);		
                    // INVERT_IN(vr(i*AXISSPACE+14),vr(i*AXISSPACE+15))  
                }


                if (vr[i * AXISSPACE + 16] < 0)  //正限位
                {
                    //fwd_in=-1
                }
                else
                {
                    //'正限位设置及反转
                    //fwd_in=vr(i*AXISSPACE+16);		
                    //INVERT_IN(vr(i*AXISSPACE+16),vr(i*AXISSPACE+17));
                }

                if (vr[i * AXISSPACE + 18] < 0)  //负限位
                {
                    // rev_in=-1
                }
                else
                {
                    //负限位设置及反转
                    //rev_in=vr(i*AXISSPACE+18)		
                    //INVERT_IN(vr(i*AXISSPACE+18),vr(i*AXISSPACE+19))
                }

                if (vr[i * AXISSPACE + 20] < 0)  //报警
                {
                    //alm_in=-1
                }
                else
                {
                    //报警设置及反转
                    //alm_in=vr(i*AXISSPACE+20)		
                    //INVERT_IN(vr(i*AXISSPACE+20),vr(i*AXISSPACE+21))
                }

                if (vr[i * AXISSPACE + 22] >= 0)
                {
                    zmcaux.ZAux_Direct_SetOp(g_handle, i, Convert.ToUInt32(vr[i * AXISSPACE + 22]));
                    //op(vr(i*AXISSPACE+22),on)  //打开使能
                }

            }
        }

        public void initset()
        {
            if (fileflag != 1419)
            {
                firstflag = 1419;  //上电保存标志
                //FLASH_WRITE 20,firstflag,filetoflash,totalfilenum
            }
        }

        //X轴 原点信号及反转
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            paratemp[14] = float.Parse(textBox1.Text);
            //MessageBox.Show(paratemp[14].ToString());   //测试textbox控件值改变
        }
        //X轴 正限位信号
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            paratemp[16] = float.Parse(textBox2.Text);
            //MessageBox.Show(paratemp[16].ToString());   //测试textbox控件值改变
        }
        //X轴 负限位信号
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            paratemp[18] = float.Parse(textBox3.Text);
            //MessageBox.Show(paratemp[18].ToString());   //测试textbox控件值改变
        }
        //X轴 报警信号
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            paratemp[20] = float.Parse(textBox4.Text);
            //MessageBox.Show(paratemp[20].ToString());   //测试textbox控件值改变
        }
        //X轴 使能信号
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            paratemp[22] = float.Parse(textBox5.Text);
            //MessageBox.Show(paratemp[22].ToString());   //测试textbox控件值改变
        }
        //X轴 报警清除信号
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            paratemp[23] = float.Parse(textBox6.Text);
            //MessageBox.Show(paratemp[23].ToString());   //测试textbox控件值改变
        }

        //Y轴 原点信号及反转
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            paratemp[39] = float.Parse(textBox7.Text);
            //MessageBox.Show(paratemp[39].ToString());   //测试textbox控件值改变
        }
        //Y轴 正限位信号
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            paratemp[41] = float.Parse(textBox8.Text);
            //MessageBox.Show(paratemp[41].ToString());   //测试textbox控件值改变
        }
        //Y轴 负限位信号
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            paratemp[43] = float.Parse(textBox9.Text);
            //MessageBox.Show(paratemp[43].ToString());   //测试textbox控件值改变
        }
        //Y轴 报警信号
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            paratemp[45] = float.Parse(textBox10.Text);
            //MessageBox.Show(paratemp[45].ToString());   //测试textbox控件值改变
        }
        //Y轴 使能信号
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            paratemp[47] = float.Parse(textBox11.Text);
            //MessageBox.Show(paratemp[47].ToString());   //测试textbox控件值改变
        }
        //Y轴 报警清除信号 
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            paratemp[48] = float.Parse(textBox12.Text);
            //MessageBox.Show(paratemp[48].ToString());   //测试textbox控件值改变
        }

        //Z轴 原点信号及反转
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            paratemp[64] = float.Parse(textBox13.Text);
            //MessageBox.Show(paratemp[64].ToString());   //测试textbox控件值改变
        }
        //Z轴 正限位信号
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            paratemp[66] = float.Parse(textBox14.Text);
            //MessageBox.Show(paratemp[66].ToString());   //测试textbox控件值改变
        }
        //Z轴 负限位信号
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            paratemp[68] = float.Parse(textBox15.Text);
            //MessageBox.Show(paratemp[68].ToString());   //测试textbox控件值改变
        }
        //Z轴 报警信号
        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            paratemp[70] = float.Parse(textBox16.Text);
            //MessageBox.Show(paratemp[70].ToString());   //测试textbox控件值改变
        }
        //Z轴 使能信号
        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            paratemp[72] = float.Parse(textBox17.Text);
            //MessageBox.Show(paratemp[72].ToString());   //测试textbox控件值改变
        }
        //Z轴 报警清除信号
        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            paratemp[73] = float.Parse(textBox18.Text);
            //MessageBox.Show(paratemp[73].ToString());   //测试textbox控件值改变
        }

        //U轴 原点信号及反转
        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            paratemp[89] = float.Parse(textBox19.Text);
            //MessageBox.Show(paratemp[89].ToString());   //测试textbox控件值改变
        }
        //U轴 正限位信号
        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            paratemp[91] = float.Parse(textBox20.Text);
            //MessageBox.Show(paratemp[91].ToString());   //测试textbox控件值改变
        }
        //U轴 负限位信号
        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            paratemp[93] = float.Parse(textBox21.Text);
            //MessageBox.Show(paratemp[93].ToString());   //测试textbox控件值改变
        }
        //U轴 报警信号
        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            paratemp[95] = float.Parse(textBox22.Text);
            //MessageBox.Show(paratemp[95].ToString());   //测试textbox控件值改变
        }
        //U轴 使能信号
        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            paratemp[97] = float.Parse(textBox23.Text);
            //MessageBox.Show(paratemp[97].ToString());   //测试textbox控件值改变
        }
        //U轴报警清除信号
        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            paratemp[98] = float.Parse(textBox24.Text);
            //MessageBox.Show(paratemp[98].ToString());   //测试textbox控件值改变
        }


        //只能输入有理数
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
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
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
        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox22_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox23_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }
        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            deal_limit(sender, e);
        }

        //on/off
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text=Choice(ref paratemp[15], button1.Text);   
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = Choice(ref paratemp[17], button2.Text);   
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = Choice(ref paratemp[19], button3.Text);   
        }
        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = Choice(ref paratemp[21], button4.Text);  
        }
        private void button5_Click(object sender, EventArgs e)
        {
            button5.Text = Choice(ref paratemp[40], button5.Text);  
        }
        private void button6_Click(object sender, EventArgs e)
        {
            button6.Text = Choice(ref paratemp[42], button6.Text);  
        }
        private void button7_Click(object sender, EventArgs e)
        {
            button7.Text = Choice(ref paratemp[44], button7.Text);  
        }
        private void button8_Click(object sender, EventArgs e)
        {
            button8.Text = Choice(ref paratemp[46], button8.Text);  
        }
        private void button9_Click(object sender, EventArgs e)
        {
            button9.Text = Choice(ref paratemp[65], button9.Text);  
        }
        private void button10_Click(object sender, EventArgs e)
        {
            button10.Text = Choice(ref paratemp[67], button10.Text);  
        }
        private void button11_Click(object sender, EventArgs e)
        {
            button11.Text = Choice(ref paratemp[69], button11.Text);  
        }
        private void button12_Click(object sender, EventArgs e)
        {
            button12.Text = Choice(ref paratemp[71], button12.Text);  
        }
        private void button13_Click(object sender, EventArgs e)
        {
            button13.Text = Choice(ref paratemp[90], button13.Text);  
        }
        private void button14_Click(object sender, EventArgs e)
        {
            button14.Text = Choice(ref paratemp[92], button14.Text);  
        }
        private void button15_Click(object sender, EventArgs e)
        {
            button15.Text = Choice(ref paratemp[94], button15.Text);  
        }
        private void button16_Click(object sender, EventArgs e)
        {
            button16.Text = Choice(ref paratemp[96], button16.Text);  
        }

        //保存
        private void button25_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < PARANUM; i++)
            {
                vr[i] = paratemp[i];
            }
            deal_setpara();
            initset();
            flag_setchange = 0;  //保存标志
            _51_保存成功提示 f51 = new _51_保存成功提示();
            f51.ShowDialog();       
        }

        //返回
        private void button26_Click(object sender, EventArgs e)
        {
            flag_returnwindow = 13; //参数设置界面
            for (int i = 0; i < PARANUM; i++)
            {
                if (paratemp[i] != vr[i])
                {
                    flag_change = 123;
                    break;
                }
                //DMCPY paraaxis(0),table(0),PARANUM		'赋值	'目前此指令遇到0终止，不能用
                //flag_change=STRCOMP(paraaxis,paratemp)   
            }

            if (flag_change == 123)   //有改动
            {
                _50未保存提示 f50 = new _50未保存提示(return_13参数设置, null, null, this, null, null, null);
                f50.g_handle = g_handle;   //句柄
                f50.vr = vr;               //存放数组
                f50.paratemp = paratemp;   //临时数组
                f50.flag_returnwindow = flag_returnwindow;  //窗口选择
                f50.PARANUM = PARANUM;  //轴参数空间
                f50.Show();//新窗口显现     
            }
            else
            {
                this.Close();
                this.return_13参数设置.Visible = true;
            }
        }

   

     

        

       
    }
}
