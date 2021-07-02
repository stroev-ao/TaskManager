namespace TaskManager
{
    partial class FEditTask
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
            this.tb_Name = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_Customer = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_Comment = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.l_PreviuosExecutionDate = new System.Windows.Forms.Label();
            this.dtp_Time = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp_Date = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lb_Files = new TaskManager.CCustomListBoxFile();
            this.b_ClearFiles = new System.Windows.Forms.Button();
            this.b_DeleteFile = new System.Windows.Forms.Button();
            this.b_AddFile = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.b_ClearReminders = new System.Windows.Forms.Button();
            this.b_AddReminder = new System.Windows.Forms.Button();
            this.hlb_Reminders = new TaskManager.CHorizontalListBox();
            this.b_Accept = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cb_TimeUnit = new System.Windows.Forms.ComboBox();
            this.tb_EstimatedDuration = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_Name);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 45);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "*Название задачи";
            // 
            // tb_Name
            // 
            this.tb_Name.Location = new System.Drawing.Point(6, 19);
            this.tb_Name.Name = "tb_Name";
            this.tb_Name.Size = new System.Drawing.Size(268, 20);
            this.tb_Name.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_Customer);
            this.groupBox2.Location = new System.Drawing.Point(12, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 46);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "*Постановщик";
            // 
            // cb_Customer
            // 
            this.cb_Customer.FormattingEnabled = true;
            this.cb_Customer.Location = new System.Drawing.Point(6, 19);
            this.cb_Customer.Name = "cb_Customer";
            this.cb_Customer.Size = new System.Drawing.Size(268, 21);
            this.cb_Customer.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_Comment);
            this.groupBox3.Location = new System.Drawing.Point(12, 115);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 85);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Комментарий";
            // 
            // tb_Comment
            // 
            this.tb_Comment.Location = new System.Drawing.Point(6, 19);
            this.tb_Comment.Multiline = true;
            this.tb_Comment.Name = "tb_Comment";
            this.tb_Comment.Size = new System.Drawing.Size(268, 60);
            this.tb_Comment.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.l_PreviuosExecutionDate);
            this.groupBox5.Controls.Add(this.dtp_Time);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.dtp_Date);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(12, 206);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(280, 58);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "*Срок исполнения";
            // 
            // l_PreviuosExecutionDate
            // 
            this.l_PreviuosExecutionDate.AutoSize = true;
            this.l_PreviuosExecutionDate.ForeColor = System.Drawing.SystemColors.GrayText;
            this.l_PreviuosExecutionDate.Location = new System.Drawing.Point(6, 42);
            this.l_PreviuosExecutionDate.Name = "l_PreviuosExecutionDate";
            this.l_PreviuosExecutionDate.Size = new System.Drawing.Size(176, 13);
            this.l_PreviuosExecutionDate.TabIndex = 4;
            this.l_PreviuosExecutionDate.Text = "Последний срок исполнения: нет";
            // 
            // dtp_Time
            // 
            this.dtp_Time.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtp_Time.Location = new System.Drawing.Point(202, 19);
            this.dtp_Time.Name = "dtp_Time";
            this.dtp_Time.ShowUpDown = true;
            this.dtp_Time.Size = new System.Drawing.Size(70, 20);
            this.dtp_Time.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Время:";
            // 
            // dtp_Date
            // 
            this.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_Date.Location = new System.Drawing.Point(47, 19);
            this.dtp_Date.Name = "dtp_Date";
            this.dtp_Date.Size = new System.Drawing.Size(100, 20);
            this.dtp_Date.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дата:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lb_Files);
            this.groupBox6.Controls.Add(this.b_ClearFiles);
            this.groupBox6.Controls.Add(this.b_DeleteFile);
            this.groupBox6.Controls.Add(this.b_AddFile);
            this.groupBox6.Location = new System.Drawing.Point(12, 437);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(280, 110);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Файлы";
            // 
            // lb_Files
            // 
            this.lb_Files.FormattingEnabled = true;
            this.lb_Files.Location = new System.Drawing.Point(6, 19);
            this.lb_Files.Name = "lb_Files";
            this.lb_Files.Size = new System.Drawing.Size(268, 56);
            this.lb_Files.TabIndex = 4;
            // 
            // b_ClearFiles
            // 
            this.b_ClearFiles.Location = new System.Drawing.Point(168, 81);
            this.b_ClearFiles.Name = "b_ClearFiles";
            this.b_ClearFiles.Size = new System.Drawing.Size(75, 23);
            this.b_ClearFiles.TabIndex = 3;
            this.b_ClearFiles.Text = "Очистить";
            this.b_ClearFiles.UseVisualStyleBackColor = true;
            this.b_ClearFiles.Click += new System.EventHandler(this.b_ClearFiles_Click);
            // 
            // b_DeleteFile
            // 
            this.b_DeleteFile.Location = new System.Drawing.Point(87, 81);
            this.b_DeleteFile.Name = "b_DeleteFile";
            this.b_DeleteFile.Size = new System.Drawing.Size(75, 23);
            this.b_DeleteFile.TabIndex = 2;
            this.b_DeleteFile.Text = "Удалить";
            this.b_DeleteFile.UseVisualStyleBackColor = true;
            this.b_DeleteFile.Click += new System.EventHandler(this.b_DeleteFile_Click);
            // 
            // b_AddFile
            // 
            this.b_AddFile.Location = new System.Drawing.Point(6, 81);
            this.b_AddFile.Name = "b_AddFile";
            this.b_AddFile.Size = new System.Drawing.Size(75, 23);
            this.b_AddFile.TabIndex = 1;
            this.b_AddFile.Text = "Добавить";
            this.b_AddFile.UseVisualStyleBackColor = true;
            this.b_AddFile.Click += new System.EventHandler(this.b_AddFile_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.b_ClearReminders);
            this.groupBox7.Controls.Add(this.b_AddReminder);
            this.groupBox7.Controls.Add(this.hlb_Reminders);
            this.groupBox7.Location = new System.Drawing.Point(12, 321);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(280, 110);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Уведомления";
            // 
            // b_ClearReminders
            // 
            this.b_ClearReminders.Location = new System.Drawing.Point(87, 81);
            this.b_ClearReminders.Name = "b_ClearReminders";
            this.b_ClearReminders.Size = new System.Drawing.Size(75, 23);
            this.b_ClearReminders.TabIndex = 2;
            this.b_ClearReminders.Text = "Очистить";
            this.b_ClearReminders.UseVisualStyleBackColor = true;
            this.b_ClearReminders.Click += new System.EventHandler(this.b_ClearReminders_Click);
            // 
            // b_AddReminder
            // 
            this.b_AddReminder.Location = new System.Drawing.Point(6, 81);
            this.b_AddReminder.Name = "b_AddReminder";
            this.b_AddReminder.Size = new System.Drawing.Size(75, 23);
            this.b_AddReminder.TabIndex = 1;
            this.b_AddReminder.Text = "Добавить";
            this.b_AddReminder.UseVisualStyleBackColor = true;
            this.b_AddReminder.Click += new System.EventHandler(this.b_AddReminder_Click);
            // 
            // hlb_Reminders
            // 
            this.hlb_Reminders.Dock = System.Windows.Forms.DockStyle.Top;
            this.hlb_Reminders.Location = new System.Drawing.Point(3, 16);
            this.hlb_Reminders.MinimumSize = new System.Drawing.Size(0, 29);
            this.hlb_Reminders.Name = "hlb_Reminders";
            this.hlb_Reminders.OnItemClick = null;
            this.hlb_Reminders.Size = new System.Drawing.Size(274, 59);
            this.hlb_Reminders.TabIndex = 0;
            this.hlb_Reminders.Text = "cHorizontalListBox1";
            // 
            // b_Accept
            // 
            this.b_Accept.Location = new System.Drawing.Point(136, 553);
            this.b_Accept.Name = "b_Accept";
            this.b_Accept.Size = new System.Drawing.Size(75, 23);
            this.b_Accept.TabIndex = 8;
            this.b_Accept.Text = "Применить";
            this.b_Accept.UseVisualStyleBackColor = true;
            this.b_Accept.Click += new System.EventHandler(this.b_Accept_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(217, 553);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 9;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.cb_TimeUnit);
            this.groupBox8.Controls.Add(this.tb_EstimatedDuration);
            this.groupBox8.Location = new System.Drawing.Point(12, 270);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(280, 45);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Оценочная длительность";
            // 
            // cb_TimeUnit
            // 
            this.cb_TimeUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_TimeUnit.FormattingEnabled = true;
            this.cb_TimeUnit.Items.AddRange(new object[] {
            "мин.",
            "час.",
            "дн."});
            this.cb_TimeUnit.Location = new System.Drawing.Point(224, 18);
            this.cb_TimeUnit.Name = "cb_TimeUnit";
            this.cb_TimeUnit.Size = new System.Drawing.Size(50, 21);
            this.cb_TimeUnit.TabIndex = 1;
            // 
            // tb_EstimatedDuration
            // 
            this.tb_EstimatedDuration.Location = new System.Drawing.Point(6, 19);
            this.tb_EstimatedDuration.Name = "tb_EstimatedDuration";
            this.tb_EstimatedDuration.Size = new System.Drawing.Size(212, 20);
            this.tb_EstimatedDuration.TabIndex = 0;
            // 
            // FEditTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(304, 594);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Accept);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FEditTask";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактировать задачу";
            this.Load += new System.EventHandler(this.FEditTask_Load);
            this.Shown += new System.EventHandler(this.FEditTask_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_Name;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tb_Comment;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DateTimePicker dtp_Time;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp_Date;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l_PreviuosExecutionDate;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button b_ClearFiles;
        private System.Windows.Forms.Button b_DeleteFile;
        private System.Windows.Forms.Button b_AddFile;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button b_Accept;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.Button b_ClearReminders;
        private System.Windows.Forms.Button b_AddReminder;
        private CHorizontalListBox hlb_Reminders;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.ComboBox cb_TimeUnit;
        private System.Windows.Forms.TextBox tb_EstimatedDuration;
        private System.Windows.Forms.ComboBox cb_Customer;
        private CCustomListBoxFile lb_Files;
    }
}