namespace TaskManager
{
    partial class FSetStoragePath
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_StoragePath = new System.Windows.Forms.TextBox();
            this.b_Browse = new System.Windows.Forms.Button();
            this.b_Accept = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.b_Browse);
            this.groupBox1.Controls.Add(this.tb_StoragePath);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Расположение хранища";
            // 
            // tb_StoragePath
            // 
            this.tb_StoragePath.Location = new System.Drawing.Point(6, 21);
            this.tb_StoragePath.Name = "tb_StoragePath";
            this.tb_StoragePath.Size = new System.Drawing.Size(187, 20);
            this.tb_StoragePath.TabIndex = 0;
            // 
            // b_Browse
            // 
            this.b_Browse.Location = new System.Drawing.Point(199, 19);
            this.b_Browse.Name = "b_Browse";
            this.b_Browse.Size = new System.Drawing.Size(75, 23);
            this.b_Browse.TabIndex = 1;
            this.b_Browse.Text = "Обзор...";
            this.b_Browse.UseVisualStyleBackColor = true;
            this.b_Browse.Click += new System.EventHandler(this.b_Browse_Click);
            // 
            // b_Accept
            // 
            this.b_Accept.Location = new System.Drawing.Point(136, 66);
            this.b_Accept.Name = "b_Accept";
            this.b_Accept.Size = new System.Drawing.Size(75, 23);
            this.b_Accept.TabIndex = 1;
            this.b_Accept.Text = "Применить";
            this.b_Accept.UseVisualStyleBackColor = true;
            this.b_Accept.Click += new System.EventHandler(this.b_Accept_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.Location = new System.Drawing.Point(217, 66);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 2;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            // 
            // FSetStoragePath
            // 
            this.AcceptButton = this.b_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(304, 101);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Accept);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSetStoragePath";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Расположение хранилища";
            this.Load += new System.EventHandler(this.FSetStoragePath_Load);
            this.Shown += new System.EventHandler(this.FSetStoragePath_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button b_Browse;
        private System.Windows.Forms.TextBox tb_StoragePath;
        private System.Windows.Forms.Button b_Accept;
        private System.Windows.Forms.Button b_Cancel;
    }
}