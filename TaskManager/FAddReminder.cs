using System;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FAddReminder : Form
    {
        CController controller;
        DateTime executionDate;
        CReminder reminder;
        int mode;

        public FAddReminder(CController controller, DateTime executionDate)
        {
            InitializeComponent();

            this.controller = controller;
            this.executionDate = executionDate;

            mode = 0;
        }

        public FAddReminder(CController controller, CReminder reminder, DateTime executionDate)
        {
            InitializeComponent();

            this.controller = controller;
            this.executionDate = executionDate;
            this.reminder = reminder;

            mode = 1;
        }

        private void FAddReminder_Load(object sender, EventArgs e)
        {
            switch (mode)
            {
                case 0:
                    {
                        dtp_Date.Value = executionDate;
                        dtp_Time.Value = executionDate.AddHours(-1);

                        break;
                    }
                case 1:
                    {
                        Text = "Изменить уведомление";
                        b_Accept.Text = "Применить";

                        DateTime reminderDate = executionDate.AddSeconds(-reminder.Seconds);

                        dtp_Date.Value = reminderDate;
                        dtp_Time.Value = reminderDate;

                        tb_Comment.Text = reminder.Comment;

                        dtp_Date.Enabled = dtp_Time.Enabled = b_Accept.Enabled = !reminder.Shown;
                        tb_Comment.ReadOnly = reminder.Shown;

                        break;
                    }
            }
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            DateTime newDate;
            DateTime date = dtp_Date.Value;
            DateTime time = dtp_Time.Value;

            string execDate = $"{ date.Day }.{ date.Month }.{ date.Year } { time.Hour }:{ time.Minute }:{ time.Second }";

            if (DateTime.TryParse(execDate, out DateTime newExecDate) && newExecDate > DateTime.Now)
                newDate = newExecDate;
            else
            {
                MessageBox.Show("Недопустимая дата или время", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtp_Date.Focus();
                return;
            }

            int seconds = Convert.ToInt32(executionDate.Subtract(newDate).TotalSeconds);

            Tag = new CReminder(seconds, tb_Comment.Text);

            Close();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
