﻿using System;
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
    public partial class _12_IO界面in : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public float[] vr = new float[500];  //数组
        public int flag_Initialization = 1; //初始化标志 只初始化一次

        private _10Start return_10Start = null;
        public _12_IO界面in(_10Start F10)
        {
            InitializeComponent();
            this.return_10Start = F10;
        }

        
        private void _12_IO界面in_Load(object sender, EventArgs e)
        {
        }

        //输出口
        private void button41_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _17_界面_op f17 = new _17_界面_op(return_10Start);
                f17.g_handle = g_handle;
                f17.vr = vr;
                this.Hide();//隐藏现在这个窗口
                f17.Show();//新窗口显现  
            }
        }

        //返回
        private void button42_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.flag_Initialization = flag_Initialization;
            this.return_10Start.Visible = true;
        }
    }
}
