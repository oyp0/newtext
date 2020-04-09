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
    public partial class _57_覆盖提示 : Form
    {
        public int uflag = 0;  

        public _57_覆盖提示()
        {
            InitializeComponent();
        }


        public void deal_fileflash()
        {
            //FLASH_WRITE 20,firstflag,filetoflash,totalfilenum	'读文件列表,当前文件数	
        }

        //是
        private void button1_Click(object sender, EventArgs e)
        {
            //uflag=file "COPY_TO",ZINDEX_ARRAY(unameindex((unum-1) mod ONEPAGENUM )),"SD"+tostr(filetoflash(filenum-1),1,0)+".bin"
            //关闭当前窗口
            this.Close();
            if (uflag == -1)
            {
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "下载成功";
                f52.ShowDialog();
                deal_fileflash();
            }
            else
            {
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "下载失败";
                f52.ShowDialog();
            }
        
        }

        //否
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   
    }
}
