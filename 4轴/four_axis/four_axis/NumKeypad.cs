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
    public partial class NumKeypad : Form
    {
        public double num1;
        public int temp = 0;//记录存储计算方式

        public NumKeypad()
        {
            InitializeComponent();
        }

        private void NumKeypad_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";//初始化内容，设置为空
            textBox1.TextAlign = HorizontalAlignment.Right;//用来设置文本框的文字的位置， 
        }

        //0~9的输入
        public void inputNum(int myNum)
        {
            //当输入的数字连续两个都是零时


            textBox1.Text = textBox1.Text + myNum;   
        }



        //小数点
        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "0.";
            }
            //如果再次输入.则返回都输入的字符后边并提示信息
            else if (textBox1.Text.IndexOf(".") >= 0)
            {
                return;
            }
            else
                //前边有数字时，则直接在后边加上.
                textBox1.Text = textBox1.Text + ".";
        }

        //输入1
        private void button1_Click(object sender, EventArgs e)
        {
            inputNum(1);
        }

        //输入2
        private void button2_Click(object sender, EventArgs e)
        {
            inputNum(2);
        }

        //输入3
        private void button3_Click(object sender, EventArgs e)
        {
            inputNum(3);
        }

        //输入4
        private void button5_Click(object sender, EventArgs e)
        {
            inputNum(4);
        }

        //输入5
        private void button6_Click(object sender, EventArgs e)
        {
            inputNum(5);
        }

        //输入6
        private void button7_Click(object sender, EventArgs e)
        {
            inputNum(6);
        }

        //输入7
        private void button9_Click(object sender, EventArgs e)
        {
            inputNum(7);
        }

        //输入8
        private void button10_Click(object sender, EventArgs e)
        {
            inputNum(8);
        }

        //输入9
        private void button11_Click(object sender, EventArgs e)
        {
            inputNum(9);
        }

        //输入0
        private void button14_Click(object sender, EventArgs e)
        {
            inputNum(0);
        }

        //输入-
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "-";
            }
            //如果再次输入.则返回都输入的字符后边并提示信息
            else if (textBox1.Text.IndexOf("-") >= 0)
            {
                return;
            }
        }

        //输入clr
        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        //退出
        private void button12_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide(); 
        }

        //确定
        private void button15_Click(object sender, EventArgs e)
        {

        }    


    }
}
