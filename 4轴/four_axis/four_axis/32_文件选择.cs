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
    public partial class _32_文件选择 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public float[] vr = new float[500];  //数组

        public int pageSize = 5; //每页数
        public int recordCount = 0;  //总记录数
        public int pageCount = 0;  // 总页数
        public int currentPage = 0;     /// 当前页
        public int filenum=1; //
        public int filetempnum;    //文件数

        public int runlinenum = 0;  //运行行号

        private _10Start return_10Start = null; 
        public _32_文件选择(_10Start F10)
        {
            InitializeComponent();
            this.return_10Start = F10;
        }

        private void _32_文件选择_Load(object sender, EventArgs e)
        {
            dgvLoad();
            PageSorter();//分页 
        }

        //初始化配置
        public void dgvLoad()
        {
            dataGridView1.Columns[0].HeaderText = "文件ID";
            dataGridView1.Columns[1].HeaderText = "文件名";

            // 行头隐藏
            dataGridView1.RowHeadersVisible = false;
            // 设置用户不能手动给 DataGridView1 添加新行
            dataGridView1.AllowUserToAddRows = false;
            // 让 DataGridView1 的所有列宽自动调整一下。
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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
            for (int i = 1; i <= 30000; i++)
            {
                DataRow dr = table.NewRow();            //table表创建行
                dr["文件ID"] = "资产编号" + i.ToString();
                dr["文件名"] = "资产名称" + i.ToString();
                table.Rows.Add(dr);                     //将数据加入到table表中
            }

            recordCount = table.Rows.Count;     //记录总行数
            pageCount = (recordCount / pageSize);  //记录总页数
            if ((recordCount % pageSize) > 0)
            {
                pageCount++;
            }

            //默认第一页
            currentPage = 1;

            LoadPage();//调用加载数据的方法
        }

        /// LoadPage方法
        private void LoadPage()
        {
            if (currentPage < 1) currentPage = 1;
            if (currentPage > pageCount) currentPage = pageCount;

            int beginRecord;    //开始指针
            int endRecord;      //结束指针
            DataTable dtTemp;
            dtTemp = table.Clone();

            beginRecord = pageSize * (currentPage - 1);
            if (currentPage == 1) beginRecord = 0;
            endRecord = pageSize * currentPage;

            if (currentPage == pageCount) endRecord = recordCount;
            for (int i = beginRecord; i < endRecord; i++)
            {
                dtTemp.ImportRow(table.Rows[i]);
            }

            dataGridView1.Rows.Clear();

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dataGridView1.Rows.Add(new object[] { dtTemp.Rows[i][0], dtTemp.Rows[i][1]});
            }

            //labPageIndex.Text = "当前页: " + currentPage.ToString() + " / " + pageCount.ToString();//当前页
            //labRecordCount.Text = "总行数: " + recordCount.ToString() + " 行";//总记录数
            labPageIndex.Text = "当前页: " + currentPage.ToString() + " / " + pageCount.ToString();//当前页
            labRecordCount.Text = recordCount.ToString() + " 行";//总记录数
        }


       //首页
        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (currentPage == 1)
            { return; }
            currentPage = 1;
            LoadPage();
        }

        //上一页
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage == 1)
            { return; }
            currentPage--;
            LoadPage();
        }

        //下一页
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage == pageCount)
            { return; }
            currentPage++;
            LoadPage();
        }

        //尾页
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (currentPage == pageCount)
            { return; }
            currentPage = pageCount;
            LoadPage();
        }

        //关闭
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.return_10Start.Visible = true;     
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                                e.RowBounds.Location.Y,
                                                dgv.RowHeadersWidth - 4,
                                                e.RowBounds.Height);


            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                    dgv.RowHeadersDefaultCellStyle.Font,
                                    rectangle,
                                    dgv.RowHeadersDefaultCellStyle.ForeColor,
                                    TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }


        //确认
        private void button2_Click(object sender, EventArgs e)
        {
            if(filenum < recordCount)     
            {
                if (filenum < 0)
                {
                    //flash_read filetoflash(filenum-1),fileflag,filename,filelinepara,codespace(0,MAXLINENUM*LINESPACE)		'读取 
                    //DMCPY codetempspace(0),codespace(0),MAXLINENUM*LINESPACE	'赋值到临时数组
			        //dmcpy filelintempepara(0),filelinepara(0),10
			        //DMCPY filejudname(0),filename(0),FILENAMELENG
                }

                filetempnum = filenum;
				//DMCPY filetempname(0),filename(0),FILENAMELENG	'赋值到临时数组
                runlinenum=0;	//运行行号清0
				this.Close();
                this.return_10Start.Visible = true;        
				
				//?*"当前编辑",filenum,filename
            }
            else
            {
                 _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "文件不存在";
                f52.ShowDialog();        
            }        
        }

     



    

       


    }
}
