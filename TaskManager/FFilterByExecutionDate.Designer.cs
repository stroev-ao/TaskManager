namespace TaskManager
{
    partial class FFilterByExecutionDate
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
            this.dtp_ExecutionDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.b_Accept = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.l_TaskCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dtp_ExecutionDate
            // 
            this.dtp_ExecutionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_ExecutionDate.Location = new System.Drawing.Point(116, 12);
            this.dtp_ExecutionDate.Name = "dtp_ExecutionDate";
            this.dtp_ExecutionDate.Size = new System.Drawing.Size(100, 20);
            this.dtp_ExecutionDate.TabIndex = 1;
            this.dtp_ExecutionDate.ValueChanged += new System.EventHandler(this.dtp_ExecutionDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Срок исполнения:";
            // 
            // b_Accept
            // 
            this.b_Accept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Accept.Location = new System.Drawing.Point(60, 57);
            this.b_Accept.Name = "b_Accept";
            this.b_Accept.Size = new System.Drawing.Size(75, 23);
            this.b_Accept.TabIndex = 3;
            this.b_Accept.Text = "Применить";
            this.b_Accept.UseVisualStyleBackColor = true;
            this.b_Accept.Click += new System.EventHandler(this.b_Accept_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(141, 57);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 4;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // l_TaskCount
            // 
            this.l_TaskCount.AutoSize = true;
            this.l_TaskCount.ForeColor = System.Drawing.SystemColors.GrayText;
            this.l_TaskCount.Location = new System.Drawing.Point(12, 38);
            this.l_TaskCount.Margin = new System.Windows.Forms.Padding(3);
            this.l_TaskCount.Name = "l_TaskCount";
            this.l_TaskCount.Size = new System.Drawing.Size(95, 13);
            this.l_TaskCount.TabIndex = 2;
            this.l_TaskCount.Text = "Найдено задач: 0";
            // 
            // FFilterByExecutionDate
            // 
            this.AcceptButton = this.b_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(228, 92);
            this.Controls.Add(this.l_TaskCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.dtp_ExecutionDate);
            this.Controls.Add(this.b_Accept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FFilterByExecutionDate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Фильтр по сроку исполнения";
            this.Shown += new System.EventHandler(this.FFilterByExecutionDate_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtp_ExecutionDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button b_Accept;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.Label l_TaskCount;
    }
}