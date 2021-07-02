namespace TaskManager
{
    partial class FNews
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
            this.b_Close = new System.Windows.Forms.Button();
            this.rtb_News = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // b_Close
            // 
            this.b_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Close.Location = new System.Drawing.Point(97, 406);
            this.b_Close.Name = "b_Close";
            this.b_Close.Size = new System.Drawing.Size(110, 23);
            this.b_Close.TabIndex = 1;
            this.b_Close.Text = "Закрыть и забыть";
            this.b_Close.UseVisualStyleBackColor = true;
            this.b_Close.Click += new System.EventHandler(this.b_Close_Click);
            // 
            // rtb_News
            // 
            this.rtb_News.DetectUrls = false;
            this.rtb_News.Location = new System.Drawing.Point(12, 12);
            this.rtb_News.Name = "rtb_News";
            this.rtb_News.ReadOnly = true;
            this.rtb_News.Size = new System.Drawing.Size(280, 388);
            this.rtb_News.TabIndex = 0;
            this.rtb_News.Text = "";
            // 
            // FNews
            // 
            this.AcceptButton = this.b_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Close;
            this.ClientSize = new System.Drawing.Size(304, 441);
            this.Controls.Add(this.rtb_News);
            this.Controls.Add(this.b_Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FNews";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Что нового";
            this.Load += new System.EventHandler(this.FNews_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button b_Close;
        private System.Windows.Forms.RichTextBox rtb_News;
    }
}