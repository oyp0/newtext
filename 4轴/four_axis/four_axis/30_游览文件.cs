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
    public partial class _30_游览文件 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public int pagenum ;  //页数
        public int[] filetoflash = new int[15]; //id列表   
        public int[] showidlist = new int[5];	//显示ID	

        public int ONEPAGENUM = 5;	//每页文件数
   

        private _14_文件管理 return_14_文件管理 = null;
        public _30_游览文件(_14_文件管理 F14)
        {
            InitializeComponent();
            this.return_14_文件管理 = F14;
        }

        private void _30_游览文件_Load(object sender, EventArgs e)
        {
            
        }

        //文件操作
        private void deal_fileflash(int num)
        {
            //FLASH_WRITE 20,firstflag,filetoflash,totalfilenum	'读文件列表,当前文件数	
            int t0, t1;
            t0 = (pagenum - 1) * ONEPAGENUM;
            t1 = pagenum * ONEPAGENUM;
            for (int i = t0; i < t1; i++)
            {
                if (filetoflash[i] != -1)
                {
                    //filetoflash[i],fileflag,filename			'读取标志判断是否有存储				
                    // DMCPY ZINDEX_ARRAY(shownamelist(i mod ONEPAGENUM ))(0),filename[0],FILENAMELENG	

                    //   char [] chArr = filename[0].ToCharArray();
                    //  shownamelist[i % ONEPAGENUM].CopyTo(0,chArr,0,FILENAMELENG);
                    showidlist[i % ONEPAGENUM] = i + 1;	//显示ID
                }
                else
                {
                    //DMCPY ZINDEX_ARRAY(shownamelist(i mod ONEPAGENUM ))(0),zeroname(0),FILENAMELENG
                    //  char[] chArr = zeroname[0].ToCharArray();
                    //   shownamelist[i % ONEPAGENUM].CopyTo(0, chArr, 0, FILENAMELENG);
                    showidlist[i % ONEPAGENUM] = 0;	//显示ID
                }
            }
        }


        //退出
        private void button10_Click(object sender, EventArgs e)
        {
            deal_fileflash(0);
            this.Close();
            this.return_14_文件管理.Visible = true;
        }
    }
}
