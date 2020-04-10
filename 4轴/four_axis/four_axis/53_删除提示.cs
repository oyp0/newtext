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
    public partial class _53_删除提示 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public int totalfilenum = 0;  //文件总数
        public int totalpagenum = 1;  //总页数 
        public int pagenum = 0;  //页数
        public int filenum = 1; //文件数

        public int fileflag;	//读写判断
 
        public int FILENUMMAX = 15;  //文件允许总数s
        public int ONEPAGENUM = 5;	//每页文件数

        public int[] filetoflash = new int[15]; //id列表   
        public int[] showidlist = new int[5];	//显示ID	


        public _53_删除提示()
        {
            InitializeComponent();
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
                    // flash_read filetoflash(i),fileflag,filename			'读取标志判断是否有存储				
                    // DMCPY ZINDEX_ARRAY(shownamelist(i mod ONEPAGENUM ))(0),filename(0),FILENAMELENG	

                    showidlist[i % ONEPAGENUM] = i + 1;	//显示ID
                    //MessageBox.Show(showidlist[i%ONEPAGENUM].ToString());
                }
                else
                {
                    //DMCPY ZINDEX_ARRAY(shownamelist(i mod ONEPAGENUM ))(0),zeroname(0),FILENAMELENG
                    showidlist[i % ONEPAGENUM] = 0;	//显示ID
                    //MessageBox.Show(showidlist[i%ONEPAGENUM].ToString());
                }
            }
        }

        //是
        private void button1_Click(object sender, EventArgs e)
        {
            if (totalfilenum > 0)
            {
                if (filenum != 0)
                {
                    if (filenum <= totalfilenum)
                    {
                        fileflag = 0;
                        Console.WriteLine("清除flash：", filetoflash[filenum - 1]);
                        //FLASH_WRITE filetoflash(filenum-1),fileflag,zeroname,filelinepara,setzero				'释放flash块		
                        //DMDEL filetoflash(filenum-1)		'删除块号
                        filetoflash[FILENUMMAX - 1] = -1;				//ID列表刷新
                        if (filenum == totalfilenum)
                        {
                            filenum = totalfilenum - 1;		//最后一个上移
                        }
                        totalfilenum = totalfilenum - 1;			//文件数-1
                        pagenum = (filenum - 1) / ONEPAGENUM + 1;	//页数刷新,启用每次删除跳转到最后
                        totalpagenum = (totalfilenum - 1) / ONEPAGENUM + 1;
                        deal_fileflash(0);
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                        Console.WriteLine("文件不存在");
                        _52_操作提示 f52 = new _52_操作提示();
                        f52.V1 = "文件不存在";
                        f52.ShowDialog();
                    }

                }
                else
                {
                    Console.WriteLine("未选择文件ID");
                    _52_操作提示 f52 = new _52_操作提示();
                    f52.V1 = "未选择文件ID";
                    f52.ShowDialog();
                }
            }
            else
            {
                Console.WriteLine("没有文件");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "没有文件";
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
