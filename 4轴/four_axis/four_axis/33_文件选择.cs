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
    public partial class _33_文件选择 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public int filetempnum = 0;   

        public int pagenum = 0;  //页数
        public int filenum = 1; //文件数
        public int totalfilenum=0;  //文件总数
        public int totalpagenum = 1;  //总页数 

        public int FILENUMMAX = 15;  //文件允许总数
        public int ONEPAGENUM = 5;	//每页文件数
        public int TOTALPANGE;  	//页数
                                       
        public int runlinenum = 0;  //运行行号
        public int fileid,sltid;	//文件id,选中id;
	   
        public int[] filetoflash = new int[15]; //id列表   
        public int[] showidlist  = new int[5];	//显示ID	

        public string filename;       //文件名

        public int asd;

        private _10Start return_10Start = null;
        public _33_文件选择(_10Start F10)
        {
            InitializeComponent();
            this.return_10Start = F10;
        }

        private void _33_Load(object sender, EventArgs e)
        {
            TOTALPANGE=FILENUMMAX/ONEPAGENUM;   //页数
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
            for (int i = 1; i <= FILENUMMAX; i++)
            {
                DataRow dr = table.NewRow();            //table表创建行
                dr["文件ID"] =  i.ToString();
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
            int t0,t1;
            t0=(pagenum-1)*ONEPAGENUM;
            t1=pagenum*ONEPAGENUM;
            for(int i=t0;i<t1;i++)
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

      

        //上一页
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (pagenum == 1)
            {
                Console.WriteLine("第一页");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "第一页";
                f52.ShowDialog();
                return; 
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

            if ((pagenum <totalpagenum) || (asd == 123))
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
        }


        //确认
        private void button2_Click(object sender, EventArgs e)
        {
            if (filenum <= totalfilenum)
            {
                if (filenum != 0)
                {
                    //flash_read filetoflash(filenum-1),fileflag,filename,filelinepara,codespace(0,MAXLINENUM*LINESPACE)		'读取
                    //DMCPY codetempspace(0),codespace(0),MAXLINENUM*LINESPACE	'赋值到临时数组
                    //dmcpy filelintempepara(0),filelinepara(0),10
                    //DMCPY filejudname(0),filename(0),FILENAMELENG

                    filetempnum = filenum;
                    //DMCPY filetempname(0),filename(0),FILENAMELENG;	//赋值到临时数组				   
                    runlinenum = 0;	//运行行号清0
                    this.Close();
                    this.return_10Start.filetempnum = filetempnum;
                    this.return_10Start.Visible = true;

                    Console.WriteLine("当前编辑 filenum={0}\filename={1}", filenum, filename);
                }
                else
                {
                    Console.WriteLine("未选择ID");
                    _52_操作提示 f52 = new _52_操作提示();
                    f52.V1 = "未选择文件ID";
                    f52.ShowDialog();
                }
            }
            else
            {
                Console.WriteLine("文件不存在");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "文件不存在";
                f52.ShowDialog();   
            }
        }

        //关闭
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.Visible = true;
        }


        private void deal_fileslt(int num) //选择id
        {
            if ((num + ONEPAGENUM * (pagenum - 1)) <= (totalfilenum + 1))
            {
                filenum = num + ONEPAGENUM * (pagenum - 1);   //获取文件数
                filename = this.dataGridView1.Rows[num-1].Cells[1].Value.ToString(); //获取文件名
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string buttonText = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
  
            int t = int.Parse(buttonText)%5;
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
