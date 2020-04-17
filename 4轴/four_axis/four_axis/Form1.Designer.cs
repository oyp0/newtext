namespace four_axis
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Search = new System.Windows.Forms.Button();
            this.Link = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.C_Ip_Address = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Search
            // 
            this.Search.Location = new System.Drawing.Point(254, 224);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(54, 32);
            this.Search.TabIndex = 8;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // Link
            // 
            this.Link.Location = new System.Drawing.Point(388, 224);
            this.Link.Name = "Link";
            this.Link.Size = new System.Drawing.Size(54, 32);
            this.Link.TabIndex = 9;
            this.Link.Text = "Link";
            this.Link.UseVisualStyleBackColor = true;
            this.Link.Click += new System.EventHandler(this.Link_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(242, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "IP:";
            // 
            // C_Ip_Address
            // 
            this.C_Ip_Address.FormattingEnabled = true;
            this.C_Ip_Address.Location = new System.Drawing.Point(275, 181);
            this.C_Ip_Address.Name = "C_Ip_Address";
            this.C_Ip_Address.Size = new System.Drawing.Size(134, 20);
            this.C_Ip_Address.TabIndex = 11;
            this.C_Ip_Address.SelectedIndexChanged += new System.EventHandler(this.C_Ip_Address_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(785, 494);
            this.Controls.Add(this.C_Ip_Address);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Link);
            this.Controls.Add(this.Search);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.Button Link;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox C_Ip_Address;

    }
}

