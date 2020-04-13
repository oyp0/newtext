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
    public partial class _16_机械参数设置 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public float[] vr = new float[500]; //数组
        public int PARANUM;           //轴参数空间
        public int AXISMAX = 4;		//轴数
        public int AXISSPACE = 25;	    //每轴参数空间
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数

        public int fileflag;        //读写判断
        public int  firstflag=0;

        public int flag_setchange=0;     //参数修改标志
        public int  flag_returnwindow=0;
        public int flag_change;


        private _13参数设置 return_13参数设置 = null;
        public _16_机械参数设置(_13参数设置 F13)
        {
            InitializeComponent();
            this.return_13参数设置 = F13;
        }

        private void _16_机械参数设置_Load(object sender, EventArgs e)
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

            textBox1.Text = vr[10].ToString();
            textBox2.Text = vr[11].ToString();
            textBox3.Text = vr[12].ToString();
            textBox4.Text = vr[13].ToString();

            textBox5.Text = vr[35].ToString();
            textBox6.Text = vr[36].ToString();
            textBox7.Text = vr[37].ToString();
            textBox8.Text = vr[38].ToString();

            textBox9.Text = vr[60].ToString();
            textBox10.Text = vr[61].ToString();
            textBox11.Text = vr[62].ToString();
            textBox12.Text = vr[63].ToString();

            textBox13.Text = vr[85].ToString();
            textBox14.Text = vr[86].ToString();
            textBox15.Text = vr[87].ToString();
            textBox16.Text = vr[88].ToString();

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

                if(vr[i*AXISSPACE+14]<0)  //原点IN 
                {
                    // datum_in=-1    
                }
                else
                {
                    //原点设置及反转
                    // datum_in=vr(i*AXISSPACE+14);		
                    // INVERT_IN(vr(i*AXISSPACE+14),vr(i*AXISSPACE+15))  
                }
                
                
                if(vr[i*AXISSPACE+16]<0)  //正限位
                {
                    //fwd_in=-1
                }
                else
                {
                    //'正限位设置及反转
                    //fwd_in=vr(i*AXISSPACE+16);		
			        //INVERT_IN(vr(i*AXISSPACE+16),vr(i*AXISSPACE+17));
                }

                 if(vr[i*AXISSPACE+18]<0)  //负限位
                 {
                   // rev_in=-1
                 }
                 else
                 {
                     //负限位设置及反转
                    //rev_in=vr(i*AXISSPACE+18)		
			        //INVERT_IN(vr(i*AXISSPACE+18),vr(i*AXISSPACE+19))
                 }
                
                 if(vr[i*AXISSPACE+20]<0)  //报警
                 {
                    //alm_in=-1
                 }
                 else
                 {
                     //报警设置及反转
                    //alm_in=vr(i*AXISSPACE+20)		
			        //INVERT_IN(vr(i*AXISSPACE+20),vr(i*AXISSPACE+21))
                 }

                if(vr[i*AXISSPACE+22]>=0)
                {
                    zmcaux.ZAux_Direct_SetOp(g_handle, i, Convert.ToUInt32(vr[i * AXISSPACE + 22]));
                    //op(vr(i*AXISSPACE+22),on)  //打开使能
                }
          
            }
        }

        public void initset()
        { 
            if(fileflag !=1419)
            {
                    firstflag=1419;  //上电保存标志
                    //FLASH_WRITE 20,firstflag,filetoflash,totalfilenum
            }
        }


        //X轴 每圈脉冲数
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            paratemp[10] = float.Parse(textBox1.Text);
            //MessageBox.Show(paratemp[10].ToString());   //测试textbox控件值改变
        }
        //X轴 螺距/角度
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            paratemp[11] = float.Parse(textBox2.Text);
            //MessageBox.Show(paratemp[11].ToString());   //测试textbox控件值改变
        }
        //X轴 负向行程限制
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            paratemp[12] = float.Parse(textBox3.Text);
            //MessageBox.Show(paratemp[12].ToString());   //测试textbox控件值改变
        }
        //X轴 正向行程限制
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            paratemp[13] = float.Parse(textBox4.Text);
            //MessageBox.Show(paratemp[13].ToString());   //测试textbox控件值改变
        }

        //Y轴 每圈脉冲数
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            paratemp[35] = float.Parse(textBox5.Text);
            //MessageBox.Show(paratemp[35].ToString());   //测试textbox控件值改变
        }
        //Y轴 螺距/角度
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            paratemp[36] = float.Parse(textBox6.Text);
            //MessageBox.Show(paratemp[36].ToString());   //测试textbox控件值改变
        }
        //Y轴 负向行程限制
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            paratemp[37] = float.Parse(textBox7.Text);
            //MessageBox.Show(paratemp[37].ToString());   //测试textbox控件值改变
        }
        //Y轴 正向行程限制
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            paratemp[38] = float.Parse(textBox8.Text);
            //MessageBox.Show(paratemp[38].ToString());   //测试textbox控件值改变
        }

        //Z轴 每圈脉冲数
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            paratemp[60] = float.Parse(textBox9.Text);
            //MessageBox.Show(paratemp[60].ToString());   //测试textbox控件值改变
        }
        //Z轴 螺距/角度
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            paratemp[61] = float.Parse(textBox10.Text);
            //MessageBox.Show(paratemp[61].ToString());   //测试textbox控件值改变
        }
        //Z轴 负向行程限制
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            paratemp[62] = float.Parse(textBox11.Text);
            //MessageBox.Show(paratemp[62].ToString());   //测试textbox控件值改变
        }
        //Z轴 正向行程限制
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            paratemp[63] = float.Parse(textBox12.Text);
            //MessageBox.Show(paratemp[63].ToString());   //测试textbox控件值改变
        }

        //U轴 每圈脉冲数
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            paratemp[85] = float.Parse(textBox13.Text);
            //MessageBox.Show(paratemp[85].ToString());   //测试textbox控件值改变
        }
        //U轴 螺距/角度
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            paratemp[86] = float.Parse(textBox14.Text);
            //MessageBox.Show(paratemp[86].ToString());   //测试textbox控件值改变
        }
        //U轴 负向行程限制
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            paratemp[87] = float.Parse(textBox15.Text);
            //MessageBox.Show(paratemp[87].ToString());   //测试textbox控件值改变
        }
        //U轴 正向行程限制
        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            paratemp[88] = float.Parse(textBox16.Text);
            //MessageBox.Show(paratemp[88].ToString());   //测试textbox控件值改变
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

        //保存
        private void button1_Click(object sender, EventArgs e)
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
        private void button2_Click(object sender, EventArgs e)
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
                //进入提示窗口
                _50未保存提示 f50 = new _50未保存提示(return_13参数设置,null,this,null,null,null,null);
                f50.g_handle = g_handle;   //句柄
                f50.vr = vr;               //存放数组
                f50.paratemp = paratemp;   //临时数组
                f50.flag_returnwindow = flag_returnwindow;  //窗口选择
                f50.PARANUM = PARANUM;  //轴参数空间
                f50.ShowDialog();
            }
            else
            {
                this.Close();
                this.return_13参数设置.Visible = true;
            }
        }






    }
}
