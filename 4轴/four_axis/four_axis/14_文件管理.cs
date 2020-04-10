﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace four_axis
{
    public partial class _14_文件管理 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号

        public int pagenum = 0;  //页数
        public int filenum = 1; //文件数
        public int totalfilenum=0;  //文件总数
        public int totalpagenum = 1;  //总页数 

        public int FILENUMMAX = 15;  //文件允许总数
        public int ONEPAGENUM = 5;	//每页文件数

        public int[] filetoflash = new int[15]; //id列表   
        public int[] showidlist = new int[5];	//显示ID	

        public int fileflag;	//读写判断
        public int FILEFLASHFLAG=13941;	 //读写标志
        int asd;

        public string filename;       //文件名

        private _10Start return_10Start = null;
        public _14_文件管理(_10Start F10)
        {
            InitializeComponent();
            this.return_10Start = F10;
        }

        //窗体加载事件
        private void _14_文件管理_Load(object sender, EventArgs e)
        {
            for(int i=0;i<FILENUMMAX;i++)
            {
                filetoflash[i]=1;
            }
            // 行头隐藏
            dataGridView1.RowHeadersVisible = false;
            // 设置用户不能手动给 DataGridView1 添加新行
            dataGridView1.AllowUserToAddRows = false;
            // 让 DataGridView1 的所有列宽自动调整一下。
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            PageSorter();
        }




        DataTable table = new DataTable();

        /// 分页的方法
        private void PageSorter()
        {

            //创建虚拟表
            DataColumn column1 = new DataColumn("文件ID", Type.GetType("System.String"));
            DataColumn column2 = new DataColumn("文件名", Type.GetType("System.String"));

            table.Columns.Add(column1);             //将列添加到table表中
            table.Columns.Add(column2);
            for (int i = 1; i <= 7; i++)
            {
                DataRow dr = table.NewRow();            //table表创建行
                dr["文件ID"] = i.ToString();
                dr["文件名"] = "file" + i.ToString();
                table.Rows.Add(dr);                     //将数据加入到table表中
            }

            totalfilenum = table.Rows.Count;     //记录总行数
            totalpagenum = (totalfilenum / ONEPAGENUM);  //记录总页数
            if ((totalfilenum % ONEPAGENUM) > 0)
            {
                totalpagenum++;
            }

            //默认第一页
            pagenum = 1;

            LoadPage();//调用加载数据的方法
        }

        /// LoadPage方法
        private void LoadPage()
        {
            if (pagenum < 1) pagenum = 1;
            if (pagenum > totalpagenum) pagenum = totalpagenum;

            int beginRecord;    //开始指针
            int endRecord;      //结束指针
            DataTable dtTemp;
            dtTemp = table.Clone();

            beginRecord = ONEPAGENUM * (pagenum - 1);
            if (pagenum == 1)
            {
                beginRecord = 0;
            }
            endRecord = ONEPAGENUM * pagenum;

            if (pagenum == totalpagenum) endRecord = totalfilenum;
            for (int i = beginRecord; i < endRecord; i++)
            {
                dtTemp.ImportRow(table.Rows[i]);
            }

            dataGridView1.Rows.Clear();

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dataGridView1.Rows.Add(new object[] { dtTemp.Rows[i][0], dtTemp.Rows[i][1] });
            }
            deal_fileflash(0);

            labPageIndex.Text = "当前页: " + pagenum.ToString(); //当前页
            // labRecordCount.Text = totalfilenum.ToString() + " 行";//总记录数
            labRecordCount.Text = "总页: " + totalpagenum.ToString();//总页
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

        //上一个
        private void button1_Click(object sender, EventArgs e)
        {

            int row = this.dataGridView1.CurrentRow.Index - 1;
            MessageBox.Show(row.ToString());
            if (row < 0)
            {
                Console.WriteLine("第一个");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "第一个";
                f52.ShowDialog();
            }
            else
            this.dataGridView1.CurrentCell = this.dataGridView1[0, row];

            //if(filenum > 1)
            //{
            //    filenum=filenum-1;
            //    int temppage;
            //    temppage = (filenum - 1) / ONEPAGENUM + 1;
            //    if(pagenum != temppage)
            //    {
            //        pagenum=temppage;
            //        deal_fileflash(0);		
            //    }
            //}
            //else
            //{
            //     Console.WriteLine("第一个");
            //     _52_操作提示 f52 = new _52_操作提示();
            //     f52.V1 = "第一个";
            //     f52.ShowDialog();
            //}            
        }
        
        //下一个
        private void button2_Click(object sender, EventArgs e)
        {
            int row = this.dataGridView1.CurrentRow.Index + 1;
            MessageBox.Show(row.ToString());
            if (row > this.dataGridView1.RowCount - 1)
            {
                Console.WriteLine("最后一个");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "最后一个";
                f52.ShowDialog();    
            }
            else
            this.dataGridView1.CurrentCell = this.dataGridView1[0, row];


            //if((filenum<(totalfilenum+1))&&(filenum < FILENUMMAX))
            //{
            //    filenum=filenum+1;	//上一个ID
            //    int temppage;
            //    temppage = (filenum - 1) / ONEPAGENUM + 1;
            //    if(pagenum!=temppage)
            //    {
            //        pagenum=temppage;
            //        deal_fileflash(0);
            //    }

            //}
            //else
            //{
            //    Console.WriteLine("最后一个");
            //    _52_操作提示 f52 = new _52_操作提示();
            //    f52.V1 = "最后一个";
            //    f52.ShowDialog();

            //}
        }

     

        //上一页
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (pagenum == 1)
            {
                Console.WriteLine("第一页");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "第一页";
                f52.ShowDialog();
            }
            pagenum--;
            filenum = (pagenum - 1) * ONEPAGENUM + 1;
            LoadPage();       
        }

        //下一页
        private void btnNext_Click(object sender, EventArgs e)
        {
            if ((filenum <= totalfilenum) && (totalfilenum % ONEPAGENUM == 0) && (totalfilenum < FILENUMMAX))
            {
                asd = 123;
            }

            if ((pagenum < totalpagenum) || (asd == 123))
            {
                pagenum++;
                filenum = (pagenum - 1) * ONEPAGENUM + 1;
                LoadPage();

            }
            else
            {
                Console.WriteLine("最后一页");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "最后一页";
                f52.ShowDialog();
            }
           




            if ((filenum <= totalfilenum) && (totalfilenum % ONEPAGENUM == 0) && (totalfilenum < FILENUMMAX))
            {
                asd = 123;
            }

            if ((pagenum <= totalpagenum) || (asd == 123))
            {
                pagenum = pagenum + 1;
                filenum = (pagenum - 1) * ONEPAGENUM + 1;
                deal_fileflash(0);
            }
            else
            {
                Console.WriteLine("最后一页");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "最后一页";
                f52.ShowDialog();
            }
        }

        //新建
        private void button3_Click(object sender, EventArgs e)
        {
            if (totalfilenum < FILENUMMAX)
            {
                for (int i = 0; i < FILENUMMAX; i++)
                {
                    fileflag = FILEFLASHFLAG;
                    //flash_read i+1,fileflag
                    if (fileflag != FILEFLASHFLAG)   //未使用
                    {
                        filetoflash[totalfilenum] = i + 1;   //ID表存flash块号
                        totalfilenum = totalfilenum + 1;	//当前总数+1
                        filenum = totalfilenum;			//当前选择ID更新 
                        // filename="New"+tostr(filenum,1,0);	//文件名初始值
                        //   FLASH_WRITE i+1,FILEFLASHFLAG,filename,filelinepara,setzero;	//占用flash块,
                        //标志，   文件名，   指令允许，  指令空间， 指令起始， 数据0
                        pagenum = (filenum - 1) / ONEPAGENUM + 1;
                        totalpagenum = (totalfilenum - 1) / ONEPAGENUM + 1;
                        break;
                    }
                }
                fileflag = 0;
                deal_fileflash(0);
            }
            else
            {
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "文件已满";
                f52.ShowDialog();    
            }
                
        }

        //删除
        private void button4_Click_1(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                _53_删除提示 f53 = new _53_删除提示();
                f53.g_handle = g_handle;
                f53.totalfilenum = totalfilenum;
                f53.totalpagenum = totalpagenum;
                f53.pagenum = pagenum;
                f53.filenum = filenum;
                f53.fileflag = fileflag;
                f53.FILENUMMAX = FILENUMMAX;
                f53.ONEPAGENUM = ONEPAGENUM;
                f53.filetoflash = filetoflash;
                f53.showidlist = showidlist;
                f53.Show();//新窗口显现    
            }    
        }

        //复制
        private void button5_Click(object sender, EventArgs e)
        {
            if (totalfilenum < FILENUMMAX)
            {
                if (filenum > 0)    //判断选择文件
                {
                    Console.WriteLine("选择的flash", filetoflash[filenum - 1]);
                    //flash_read filetoflash(filenum-1),fileflag,filename,tsfcopy	'先将选择块数据赋值到中转数组
                    for (int i = 0; i < FILENUMMAX; i++)
                    {
                        fileflag = FILEFLASHFLAG;				//先等于   
                        //FLASH_READ i+1,fileflag;			//读取判断
                        if (fileflag != FILEFLASHFLAG)
                        {
                            Console.WriteLine("空闲的flash", i + 1);
                            filetoflash[totalfilenum] = i + 1;  //ID表存flash块号
                            totalfilenum = totalfilenum + 1;		//当前总数+1
                            filenum = totalfilenum;				//当前选择ID更新
                            //  filename=filename +"copy"		'文件名初始值
                            //  FLASH_WRITE i+1,FILEFLASHFLAG,filename,tsfcopy		'中转数组数据赋值给块

                            pagenum = (filenum - 1) / ONEPAGENUM + 1;
                            totalpagenum = (totalfilenum - 1) / ONEPAGENUM + 1;
                            break;
                        }

                    }
                    fileflag = 0;			//判断标志置0
                    deal_fileflash(0);
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

        //关闭
        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.Visible = true;
        }

        //选择id
        private void deal_fileslt(int num) 
        {
            if ((num + ONEPAGENUM * (pagenum - 1)) <= (totalfilenum + 1))
            {
                filenum = num + ONEPAGENUM * (pagenum - 1);   //获取文件数
                filename = this.dataGridView1.Rows[num - 1].Cells[1].Value.ToString(); //获取文件名
            }
        }

        //选择
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string buttonText = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            int t = int.Parse(buttonText) % 5;
            if (t == 1)
            {
                deal_fileslt(1);
                MessageBox.Show(buttonText);
            }
            if (t == 2)
            {
                deal_fileslt(2);
                MessageBox.Show(buttonText);
            }
            if (t == 3)
            {
                deal_fileslt(3);
                MessageBox.Show(buttonText);
            }
            if (t == 4)
            {
                deal_fileslt(4);
                MessageBox.Show(buttonText);
            }
            if (t == 0)
            {
                deal_fileslt(5);
                MessageBox.Show(buttonText);
            }
        }


    }
}
