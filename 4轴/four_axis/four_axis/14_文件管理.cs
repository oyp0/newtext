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
    public partial class _14_文件管理 : Form
    {
        public IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public int filetempnum = 0;
      
        public string codename = "无";  //类型  20界面

        public float[] vr = new float[500];  //数组

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

        public int linenum=1;	//总行数，当前行号
        public int manulradio;		//初始速度比

        public int flag_Initialization = 1; //初始化标志 只初始化一次

        public int FILENAMELENG = 20;  //文件名长度      
        public float[] paratemp = new float[150];   //临时存储，用于不保存时还原参数
        public string[] filename = new string[20];   //文件名
        public string[] filejudname = new string[20];  
        public string[] shownamelist = new string[5];
        public string[] zeroname = new string[20];   //空文件数组

        public int t;
        

        int winnum=0;    //窗口30要记得传值过来哦  


        ////////////行编辑////////////////////
	
        public int MAXLINENUM=100;	//允许最大行数
        public int LINESTART=30;	//flash指令起始地址
        public int LINESPACE=20;	//行空间

        public int[]  filelintempepara = new int[10];  
        public int[]  filelinepara = new int[10];   //总行
    


        public String[] codespace = new String[2000];	//存放数组
        public String[] codetempspace = new String[2000];	//临时空间

        public int[] templine= new int[30];	//临时

        public int nCurrent = 0;      //当前记录行

        public int browsepage = 1;

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
            for (int i = 1; i <= 10; i++)
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

        //上一个
        private void button1_Click(object sender, EventArgs e)
        {
            //int row = this.dataGridView1.CurrentRow.Index - 1;
            //if (row < 0)
            //{
            //    Console.WriteLine("第一个");
            //    _52_操作提示 f52 = new _52_操作提示();
            //    f52.V1 = "第一个";
            //    f52.ShowDialog();
            //}
            //else
            //{
            //    deal_fileflash(0);
            //    filenum = (pagenum - 1) * ONEPAGENUM + row + 1;
            //    this.dataGridView1.CurrentCell = this.dataGridView1[0, row];
            //}

            if (filenum > 1)
            {
                filenum = filenum - 1;
                int temppage;
                temppage = (filenum - 1) / ONEPAGENUM + 1;
                if (pagenum != temppage)
                {
                    pagenum = temppage;
                    deal_fileflash(0);
                }
            }
            else
            {
                Console.WriteLine("第一个");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "第一个";
                f52.ShowDialog();
            }            
        }
        
        //下一个
        private void button2_Click(object sender, EventArgs e)
        {
            int row = this.dataGridView1.CurrentRow.Index + 1;
            if (row > this.dataGridView1.RowCount - 1)
            {
                Console.WriteLine("最后一个");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "最后一个";
                f52.ShowDialog();
            }
            else
            {
                deal_fileflash(0);
                filenum = (pagenum - 1) * ONEPAGENUM + row + 1;
                this.dataGridView1.CurrentCell = this.dataGridView1[0, row];    
            }
         
            
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
                return;
            }
        }

     

        //新建
        private void button3_Click(object sender, EventArgs e)
        {
            if (totalfilenum < FILENUMMAX)
            {
                if (pagenum == totalpagenum)
                {
                    filetoflash[totalfilenum] = totalfilenum + 1;   //ID表存flash块号
                    totalfilenum = totalfilenum + 1;	//当前总数+1    
                    filenum = totalfilenum;			//当前选择ID更新 
                    filename[0] = "New";	//文件名初始值  
                    pagenum = (filenum - 1) / ONEPAGENUM + 1;
                    totalpagenum = (totalfilenum - 1) / ONEPAGENUM + 1;
                    int index = this.dataGridView1.Rows.Add();
                    if (index%5 < ONEPAGENUM)
                    {
                        this.dataGridView1.Rows[index].Cells[0].Value = (pagenum - 1) * ONEPAGENUM + index + 1;
                        this.dataGridView1.Rows[index].Cells[1].Value = filename[0];
                    }                           
                    deal_fileflash(0);
                    return;                
                }
                pagenum = totalpagenum;
                LoadPage();          
            }
            else
            {
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "文件已满";
                f52.ShowDialog();      
            }

            //if (totalfilenum < FILENUMMAX)
            //{
            //    for (int i = 0; i < FILENUMMAX; i++)
            //    {
            //        fileflag = FILEFLASHFLAG;
            //        //flash_read i+1,fileflag
            //        if (fileflag == FILEFLASHFLAG)   //未使用
            //        {
            //            filetoflash[totalfilenum] = i + 1;   //ID表存flash块号
            //            totalfilenum = totalfilenum + 1;	//当前总数+1
            //            filenum = totalfilenum;			//当前选择ID更新 
            //            filename="New";	//文件名初始值
            //            //   FLASH_WRITE i+1,FILEFLASHFLAG,filename,filelinepara,setzero;	//占用flash块,
            //            //标志，   文件名，   指令允许，  指令空间， 指令起始， 数据0
            //            pagenum = (filenum - 1) / ONEPAGENUM + 1;
            //            totalpagenum = (totalfilenum - 1) / ONEPAGENUM + 1;
            //            break;
            //        }
            //    }
            //    fileflag = 0;
             //   deal_fileflash(0);
            //}
            //else
            //{
            //    _52_操作提示 f52 = new _52_操作提示();
            //    f52.V1 = "文件已满";
            //    f52.ShowDialog();    
            //}
                
        }

        //删除
        private void button4_Click_1(object sender, EventArgs e)
        {
            if (t < 5)
            {
                if (t == 0)
                {
                    dataGridView1.Rows.RemoveAt(t + 4);
                }
                else
                {
                    dataGridView1.Rows.RemoveAt(t - 1);
                }              
            } 

            //if (g_handle != (IntPtr)0)
            //{
            //    _53_删除提示 f53 = new _53_删除提示();
            //    f53.g_handle = g_handle;
            //    f53.totalfilenum = totalfilenum;
            //    f53.totalpagenum = totalpagenum;
            //    f53.pagenum = pagenum;
            //    f53.filenum = filenum;
            //    f53.fileflag = fileflag;
            //    f53.FILENUMMAX = FILENUMMAX;
            //    f53.ONEPAGENUM = ONEPAGENUM;
            //    f53.filetoflash = filetoflash;
            //    f53.showidlist = showidlist;
            //this.Hide();
            //    f53.Show();//新窗口显现    
            //}    
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
            this.return_10Start.flag_Initialization = flag_Initialization;
            this.return_10Start.codetempspace = codetempspace;
            this.return_10Start.linenum = linenum;
            this.return_10Start.Visible = true;
        }

        //选择id
        private void deal_fileslt(int num) 
        {
            if ((num + ONEPAGENUM * (pagenum - 1)) <= (totalfilenum + 1))
            {
                filenum = num + ONEPAGENUM * (pagenum - 1);   //获取文件数
                filename[0] = this.dataGridView1.Rows[num - 1].Cells[1].Value.ToString(); //获取文件名
            }
        }

        //选择
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string buttonText = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            t = int.Parse(buttonText) % 5;
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

        ////刷新显示
        //private void show_code(int num)
        //{
        //    if (num == 0)
        //    {
        //        codename = "无";

        //        //进入19界面     
         
        //    }
        //    else if (num == 1)
        //    {
        //        codename = "直线";
        //        //21进入直线
        //    }
        //    else if (num == 2)
        //    {
        //        codename = "三点画弧";
        //        //22进入三点画弧
        //    }
        //    else if (num == 3)
        //    {
        //        codename = "延时";
        //        //23进入延时界面
        //    }
        //    else if (num == 4)
        //    {
        //        codename = "多个输出";
        //        //24进入多个输出界面
        //    }
        //    else if (num == 5)
        //    {
        //        codename = "输出延时复位";
        //        //25进入输出延时复位界面
        //    }
        //    else if (num == 6)
        //    {
        //        codename = "圆心画弧";
        //        //26进入圆心画弧界面
        //    }
        //    else if (num == 7)
        //    {
        //        codename = "绝对模式";
        //        //27进入绝对模式界面
        //    }
        //    else if (num == 8)
        //    {
        //        codename = "相对模式";
        //        //28进入相对模式界面
        //    }

        //}


        ////加载
        //private void deal_lineload(int num)
        //{
        //    MessageBox.Show(num.ToString());
        //    MessageBox.Show(filelinepara[0].ToString());
        //    if(num<=filelinepara[0])
        //    {
        //        templine[0] = int.Parse(codespace[(num - 1) * LINESPACE]);
        //        //dmcpy templine(0),codespace((num-1)*LINESPACE),LINESPACE;	'复制到临时数组    
        //        linenum=num;	//浏览界面跳转用
        //        show_code(templine[0]);	//刷新显示
        //    }
        //    else if(num>filelinepara[0] && winnum==30)  //只有游览界面才提示
        //    {
        //        Console.WriteLine("超过文件总行数");
        //        _52_操作提示 f52 = new _52_操作提示();
        //        f52.V1 = "超过文件总行数";
        //        f52.ShowDialog();
        //    }
          
        //}

        private void deal_browseflash()
        { 
            
        }

        //游览
        private void button9_Click(object sender, EventArgs e)
        {
            if (filenum>0)
            {
                browsepage=1;
		        deal_browseflash();

                _30_游览文件 f30 = new _30_游览文件(this);
                f30.g_handle = g_handle;
                f30.pagenum = pagenum;
                f30.filetoflash = filetoflash;
                f30.showidlist = showidlist;
                this.Hide();//隐藏现在这个窗口
                f30.Show();//新窗口显现    
 
            }
            else
            {
                 Console.WriteLine("未选择ID");
                _52_操作提示 f52 = new _52_操作提示();
                f52.V1 = "未选择ID";
                f52.ShowDialog();   
            }   
        }

        //编辑
        private void button8_Click(object sender, EventArgs e)
        {
            if (filenum <= totalfilenum)
            {
                if (filenum != 0)
                {
                    //flash_read filetoflash(filenum-1),fileflag,filename,filelinepara,codespace(0,MAXLINENUM*LINESPACE)		'读取
 
                    //   char [] chArr = filename[0].ToCharArray();
                    //  shownamelist[i % ONEPAGENUM].CopyTo(0,chArr,0,FILENAMELENG);

                    //codetempspace[0].CopyTo(0,codespace[0].ToCharArray(),0,MAXLINENUM*LINESPACE);
                    //String str = filelintempepara[0].ToString();
                    //str.CopyTo(0, filelinepara[0].ToString().ToCharArray(), 0, 10);
                    //filelintempepara[0] = int.Parse(str);
                    //filejudname[0].CopyTo(0, filename[0].ToCharArray(), 0, FILENAMELENG);
                    
                    //DMCPY codetempspace(0),codespace(0),MAXLINENUM*LINESPACE	'赋值到临时数组
                    //dmcpy filelintempepara(0),filelinepara(0),10
                    //DMCPY filejudname(0),filename(0),FILENAMELENG


                    MessageBox.Show(linenum.ToString());
               //     linenum = 1;
                    _19_文件编辑 f19 = new _19_文件编辑(this);
                    f19.g_handle = g_handle;
                    f19.codename = codename;
                    f19.filenum = filenum;
                    filelinepara[0] = totalfilenum;      
                    f19.filelinepara = filelinepara;
                    f19.filelintempepara = filelintempepara;
                    f19.codespace = codespace;
                    f19.codetempspace = codetempspace;
                    f19.browsepage = browsepage;
                    f19.manulradio = manulradio;	//初始速度比
                   // f19.paratemp = paratemp;
                    f19.vr = vr;
                    f19.pagenum = pagenum;
                    f19.filetoflash = filetoflash;
                    f19.showidlist = showidlist;
                    f19.linenum = linenum;
                    this.Hide();//隐藏现在这个窗口
                    f19.Show();//新窗口显现    
              //    deal_lineload(linenum);	//加载第一行
               //     filename[0] = this.dataGridView1.Rows[linenum - 1].Cells[1].Value.ToString(); //获取文件名
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


    }
}
