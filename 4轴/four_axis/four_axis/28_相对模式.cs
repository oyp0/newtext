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
    public partial class _28_相对模式 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        private _20_运动类型选择 return_20_运动类型选择 = null;
        public _28_相对模式(_20_运动类型选择 F20)
        {
            InitializeComponent();
            this.return_20_运动类型选择 = F20;
        }

        private void _28_相对模式_Load(object sender, EventArgs e)
        {

        }
    }
}
