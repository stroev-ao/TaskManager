namespace TaskManager
{
    partial class FFilterByCustomer
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
            this.clb_Customers = new System.Windows.Forms.CheckedListBox();
            this.b_Accept = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_CkechAll = new System.Windows.Forms.ToolStripButton();
            this.tsb_UncheckAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tstb_Filter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clb_Customers
            // 
            this.clb_Customers.FormattingEnabled = true;
            this.clb_Customers.IntegralHeight = false;
            this.clb_Customers.Location = new System.Drawing.Point(12, 41);
            this.clb_Customers.Name = "clb_Customers";
            this.clb_Customers.Size = new System.Drawing.Size(280, 359);
            this.clb_Customers.TabIndex = 1;
            // 
            // b_Accept
            // 
            this.b_Accept.Location = new System.Drawing.Point(136, 406);
            this.b_Accept.Name = "b_Accept";
            this.b_Accept.Size = new System.Drawing.Size(75, 23);
            this.b_Accept.TabIndex = 2;
            this.b_Accept.Text = "Применить";
            this.b_Accept.UseVisualStyleBackColor = true;
            this.b_Accept.Click += new System.EventHandler(this.b_Accept_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(217, 406);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 3;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_CkechAll,
            this.tsb_UncheckAll,
            this.toolStripSeparator1,
            this.tstb_Filter,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(304, 38);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_CkechAll
            // 
            this.tsb_CkechAll.Image = global::TaskManager.Properties.Resources.check_all;
            this.tsb_CkechAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_CkechAll.Name = "tsb_CkechAll";
            this.tsb_CkechAll.Size = new System.Drawing.Size(79, 35);
            this.tsb_CkechAll.Text = "Выбрать все";
            this.tsb_CkechAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_CkechAll.Click += new System.EventHandler(this.tsb_CkechAll_Click);
            // 
            // tsb_UncheckAll
            // 
            this.tsb_UncheckAll.Image = global::TaskManager.Properties.Resources.uncheck_all;
            this.tsb_UncheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_UncheckAll.Name = "tsb_UncheckAll";
            this.tsb_UncheckAll.Size = new System.Drawing.Size(64, 35);
            this.tsb_UncheckAll.Text = "Снять все";
            this.tsb_UncheckAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_UncheckAll.Click += new System.EventHandler(this.tsb_UncheckAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // tstb_Filter
            // 
            this.tstb_Filter.Name = "tstb_Filter";
            this.tstb_Filter.Size = new System.Drawing.Size(120, 38);
            this.tstb_Filter.TextChanged += new System.EventHandler(this.tstb_Filter_TextChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel1.Image = global::TaskManager.Properties.Resources.search;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(16, 35);
            this.toolStripLabel1.Text = "Поиск";
            // 
            // FFilterByCustomer
            // 
            this.AcceptButton = this.b_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(304, 441);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Accept);
            this.Controls.Add(this.clb_Customers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FFilterByCustomer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Фильтр по постановщику";
            this.Load += new System.EventHandler(this.FFilterByCustomer_Load);
            this.Shown += new System.EventHandler(this.FFilterByCustomer_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb_Customers;
        private System.Windows.Forms.Button b_Accept;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_CkechAll;
        private System.Windows.Forms.ToolStripButton tsb_UncheckAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox tstb_Filter;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}