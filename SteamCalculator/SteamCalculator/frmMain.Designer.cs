namespace SteamCalculator
{
    partial class frmMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.lvGames = new System.Windows.Forms.ListView();
            this.chGameName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblSum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Location = new System.Drawing.Point(0, 196);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(290, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // pb1
            // 
            this.pb1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb1.Location = new System.Drawing.Point(0, 143);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(290, 18);
            this.pb1.TabIndex = 1;
            // 
            // lvGames
            // 
            this.lvGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chGameName,
            this.chPrice});
            this.lvGames.Location = new System.Drawing.Point(0, 0);
            this.lvGames.Name = "lvGames";
            this.lvGames.Size = new System.Drawing.Size(290, 137);
            this.lvGames.TabIndex = 2;
            this.lvGames.UseCompatibleStateImageBehavior = false;
            this.lvGames.View = System.Windows.Forms.View.Details;
            // 
            // chGameName
            // 
            this.chGameName.Text = "Game";
            this.chGameName.Width = 190;
            // 
            // chPrice
            // 
            this.chPrice.Text = "Price";
            this.chPrice.Width = 72;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(128, 164);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(30, 13);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "0 / 0";
            // 
            // lblSum
            // 
            this.lblSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSum.AutoSize = true;
            this.lblSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSum.Location = new System.Drawing.Point(128, 177);
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(55, 16);
            this.lblSum.TabIndex = 4;
            this.lblSum.Text = "Sum: $0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 221);
            this.Controls.Add(this.lblSum);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lvGames);
            this.Controls.Add(this.pb1);
            this.Controls.Add(this.btnStart);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SteamCalculator (C#)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.ListView lvGames;
        private System.Windows.Forms.ColumnHeader chGameName;
        private System.Windows.Forms.ColumnHeader chPrice;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblSum;
    }
}

