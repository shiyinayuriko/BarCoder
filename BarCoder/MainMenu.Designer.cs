namespace BarCoder
{
    partial class MainMenu
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
            this.check = new System.Windows.Forms.Button();
            this.generation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // check
            // 
            this.check.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check.Location = new System.Drawing.Point(12, 12);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(178, 65);
            this.check.TabIndex = 0;
            this.check.Text = "验票";
            this.check.UseVisualStyleBackColor = true;
            this.check.Click += new System.EventHandler(this.StartChecker);
            // 
            // generation
            // 
            this.generation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generation.Enabled = false;
            this.generation.Location = new System.Drawing.Point(12, 88);
            this.generation.Name = "generation";
            this.generation.Size = new System.Drawing.Size(178, 65);
            this.generation.TabIndex = 1;
            this.generation.Text = "生成";
            this.generation.UseVisualStyleBackColor = true;
            this.generation.Click += new System.EventHandler(this.GenerateCode);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 162);
            this.Controls.Add(this.generation);
            this.Controls.Add(this.check);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BarCoder";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button check;
        private System.Windows.Forms.Button generation;
    }
}

