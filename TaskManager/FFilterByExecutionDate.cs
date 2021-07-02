using System;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FFilterByExecutionDate : Form
    {
        CController controller;

        public FFilterByExecutionDate(CController controller)
        {
            InitializeComponent();

            this.controller = controller;
        }

        private void FFilterByExecutionDate_Shown(object sender, EventArgs e)
        {
            dtp_ExecutionDate_ValueChanged(dtp_ExecutionDate, e);
        }

        private void dtp_ExecutionDate_ValueChanged(object sender, EventArgs e)
        {
            CountTasks(dtp_ExecutionDate.Value);
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            controller.SetFilterByExecutionDate(dtp_ExecutionDate.Value);

            Close();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CountTasks(DateTime dateTime)
        {
            Cursor.Current = Cursors.WaitCursor;

            l_TaskCount.Text = $"Найдено задач: { controller.GetTaskCountByExecutionDate(dateTime) }";

            Cursor.Current = Cursors.Default;
        }
    }
}
