namespace eProcBackUp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cboxPrefix = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.lnkOutputPath = new System.Windows.Forms.LinkLabel();
            this.tmrPause = new System.Windows.Forms.Timer(this.components);
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // cboxPrefix
            // 
            this.cboxPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxPrefix.FormattingEnabled = true;
            this.cboxPrefix.Location = new System.Drawing.Point(13, 13);
            this.cboxPrefix.Margin = new System.Windows.Forms.Padding(4);
            this.cboxPrefix.Name = "cboxPrefix";
            this.cboxPrefix.Size = new System.Drawing.Size(216, 28);
            this.cboxPrefix.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.PaleGreen;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(13, 140);
            this.btnGo.Margin = new System.Windows.Forms.Padding(4);
            this.btnGo.Name = "btnGo";
            this.btnGo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGo.Size = new System.Drawing.Size(103, 38);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.MistyRose;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Location = new System.Drawing.Point(126, 140);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(4);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(103, 38);
            this.btnQuit.TabIndex = 2;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // lnkOutputPath
            // 
            this.lnkOutputPath.AutoSize = true;
            this.lnkOutputPath.Location = new System.Drawing.Point(23, 193);
            this.lnkOutputPath.Name = "lnkOutputPath";
            this.lnkOutputPath.Size = new System.Drawing.Size(199, 20);
            this.lnkOutputPath.TabIndex = 3;
            this.lnkOutputPath.TabStop = true;
            this.lnkOutputPath.Text = "Open Output Directory";
            this.lnkOutputPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOutputPath_LinkClicked);
            // 
            // tmrPause
            // 
            this.tmrPause.Interval = 2000;
            this.tmrPause.Tick += new System.EventHandler(this.tmrPause_Tick);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(111, 98);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(26, 27);
            this.pictureBox3.TabIndex = 54;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 222);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.lnkOutputPath);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.cboxPrefix);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "eProcessing BackUp";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboxPrefix;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.LinkLabel lnkOutputPath;
        private System.Windows.Forms.Timer tmrPause;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}

