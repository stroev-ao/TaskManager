using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TaskManager
{
    public partial class FEditTask : Form
    {
        CController controller;
        int taskId;
        CTask task;

        List<CReminder> reminders;
        List<CPreFile> files;

        public FEditTask(CController controller, int taskId)
        {
            InitializeComponent();

            this.controller = controller;
            this.taskId = taskId;
        }

        private void FEditTask_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            task = controller.GetTask(taskId);

            tb_Name.Text = task.Name;

            cb_Customer.Items.AddRange(controller.GetCustomerNames());
            cb_Customer.SelectedIndex = task.Customer;
            //cb_Customer.Text = controller.GetCustomerName(task.Customer);

            tb_Comment.Text = task.Comment;
            //tb_Location.Text = task.Location;

            dtp_Date.Value = task.ExecutionDate;
            dtp_Time.Value = task.ExecutionDate;
            l_PreviuosExecutionDate.Text = $"Последний срок исполнения: { (string.IsNullOrEmpty(task.PreviousExecutionDate) ? "нет" : task.PreviousExecutionDate) }";

            tb_EstimatedDuration.Text = GetEstimatedDuration(task.EstimatedDuration);

            reminders = new List<CReminder>(task.Reminders);

            files = new List<CPreFile>(task.GetFiles());
            
            hlb_Reminders.OnItemClick += RemoveReminder;
            UpdateReminderList();

            UpdateFileList();

            bool value = task.Status != CTask.EStatus.Завершена;
            tb_Name.ReadOnly = !value;
            cb_Customer.Enabled = value;
            tb_Comment.ReadOnly = !value;
            //tb_Location.ReadOnly = !value;
            dtp_Date.Enabled = value;
            dtp_Time.Enabled = value;
            tb_EstimatedDuration.ReadOnly = !value;
            cb_TimeUnit.Enabled = value;
            
            hlb_Reminders.Enabled = value;
            b_AddReminder.Enabled = value && reminders.Count < 3;
            b_ClearReminders.Enabled = value && reminders.Count > 0;

            lb_Files.Enabled = value;
            b_AddFile.Enabled = value;
            b_DeleteFile.Enabled = value && lb_Files.SelectedIndex >= 0;
            b_ClearFiles.Enabled = value && files.Count > 0;

            b_Accept.Enabled = value;
        }

        private void FEditTask_Shown(object sender, EventArgs e)
        {
            controller.CheckFilesInTaskFolder(taskId, out int foundCount, out int changedCount, out int failedToCheck, out int changedPath, out int deletedCount);
            
            bool haveFound = foundCount > 0;
            bool haveChanged = changedCount > 0;
            bool haveFailedToCheck = failedToCheck > 0;
            bool haveChangedPath = changedPath > 0;
            bool haveDeleted = deletedCount > 0;

            if (haveFound || haveChanged || haveChangedPath || haveFailedToCheck || haveDeleted)
            {

                string msg = $"Проверка файлов задачи показала следующие результаты:";

                if (haveFound)
                    msg += $"\n  • новых - { foundCount } шт.";

                if (haveChanged)
                    msg += $"\n  • изменено - { changedCount } шт.";

                if (haveChangedPath)
                    msg += $"\n  • изменено расположение - { changedPath } шт.";

                if (haveFailedToCheck)
                    msg += $"\n  • не удалось проверить - { failedToCheck } шт.";

                if (haveDeleted)
                    msg += $"\n  • удалено - { deletedCount } шт.";

                MessageBox.Show(msg, controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (haveFound)
                    if (MessageBox.Show($"Добавить новые файлы в список ({ foundCount } шт.)?", controller.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        try
                        {
                            controller.AddFoundFiles(taskId);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                if (haveDeleted)
                    if (MessageBox.Show($"Исключить из списка удаленные файлы ({ deletedCount } шт.)?", controller.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        controller.ExcludeDeletedFiles(taskId);
                
                files = new List<CPreFile>(task.GetFiles());

                UpdateFileList();
            }

            Cursor.Current = Cursors.Default;
        }

        private void b_AddReminder_Click(object sender, EventArgs e)
        {
            CReminder reminder = null;

            DateTime executionDate = GetExecutionDateTime();
            if (executionDate == DateTime.MinValue)
                return;

            using (FAddReminder form = new FAddReminder(controller, executionDate))
            {
                form.ShowDialog();

                if (form.Tag != null)
                    reminder = form.Tag as CReminder;
            }

            if (reminder == null)
                return;

            if (reminders.Count(el => el.Seconds == reminder.Seconds) == 0)
                reminders.Add(reminder);

            b_AddReminder.Enabled = reminders.Count < 3;
            b_ClearReminders.Enabled = reminders.Count > 0;

            UpdateReminderList();
        }

        private void b_ClearReminders_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите очистить все уведомления?", controller.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
                return;

            reminders.Clear();
            hlb_Reminders.Clear();

            b_AddReminder.Enabled = true;
            b_ClearReminders.Enabled = false;
        }

        private void lb_Files_SelectedIndexChanged(object sender, EventArgs e)
        {
            b_DeleteFile.Enabled = lb_Files.SelectedIndex >= 0;
        }

        private void lb_Files_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void lb_Files_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputs = (string[])e.Data.GetData(DataFormats.FileDrop);

            List<CPreFile> _files = new List<CPreFile>();
            for (int i = 0; i < inputs.Length; i++)
            {
                List<CPreFile> fls = new List<CPreFile>();

                string input = inputs[i];

                GetFiles(input, ref fls);

                if (!Path.HasExtension(input))
                    for (int j = 0; j < fls.Count; j++)
                    {
                        CPreFile pf = fls[j];
                        
                        pf.AdditionalPath = pf.AdditionalPath.
                            Substring(input.IndexOf($"\\{ input.Split('\\').Last() }")).
                            Replace($"\\{ Path.GetFileName(pf.FullPath) }", "");
                    }

                _files.AddRange(fls);
            }

            files.AddRange(_files);

            UpdateFileList();

            b_ClearFiles.Enabled = files.Count > 0;

            void GetFiles(string path, ref List<CPreFile> output)
            {
                if (Directory.Exists(path))
                {
                    string[] fls = Directory.GetFiles(path);
                    for (int i = 0; i < fls.Length; i++)
                        output.Add(new CPreFile(fls[i], fls[i]));

                    string[] subDirs = Directory.GetDirectories(path);
                    for (int i = 0; i < subDirs.Length; i++)
                        GetFiles(subDirs[i], ref output);
                }
                else if (File.Exists(path))
                    output.Add(new CPreFile(path, null));
            }
        }

        private void b_AddFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    files.Add(new CPreFile(ofd.FileName, null));

                    UpdateFileList();
                }
            }

            b_ClearFiles.Enabled = files.Count > 0;
        }

        private void b_DeleteFile_Click(object sender, EventArgs e)
        {
            int idx = lb_Files.SelectedIndex;
            if (idx < 0)
                return;

            string file = files[idx].FullPath;

            if (MessageBox.Show($"Вы действительно хотите удалить файл \"{ file }\"?", controller.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
                return;

            files.RemoveAt(idx);

            b_ClearFiles.Enabled = files.Count > 0;

            lb_Files.Items.Remove(lb_Files.SelectedItem);
        }

        private void b_ClearFiles_Click(object sender, EventArgs e)
        {
            if (files.Count == 0)
                return;

            if (MessageBox.Show("Вы действительно хотите удалить все файлы?", controller.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
                return;

            files.Clear();

            b_ClearFiles.Enabled = false;

            UpdateFileList();

            lb_Files.SelectedIndex = -1;
            lb_Files_SelectedIndexChanged(lb_Files, e);
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            string name = tb_Name.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Неверное название задачи", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb_Name.Focus();
                return;
            }

            string customer = cb_Customer.Text;
            if (string.IsNullOrEmpty(customer))
            {
                MessageBox.Show("Не задан постановщик задачи", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cb_Customer.Focus();
                return;
            }

            string comment = tb_Comment.Text;
            //string location = tb_Location.Text;

            DateTime executionDate = GetExecutionDateTime();
            if (executionDate == DateTime.MinValue)
                return;

            int estimatedDuration = -1;
            if (!int.TryParse(tb_EstimatedDuration.Text, out estimatedDuration) || estimatedDuration < 0)
            {
                MessageBox.Show("Неверная оценочная длительность", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb_EstimatedDuration.Focus();
                return;
            }

            switch (cb_TimeUnit.SelectedIndex)
            {
                case 0:
                    estimatedDuration *= 60;
                    break;
                case 1:
                    estimatedDuration *= 3600;
                    break;
                case 2:
                    estimatedDuration *= 86400;
                    break;
            }

            int secondsAvailable = Convert.ToInt32(executionDate.Subtract(DateTime.Now).TotalSeconds);
            if (estimatedDuration > secondsAvailable)
            {
                MessageBox.Show("Оценочная длительность больше доступного времени для выполнения задачи", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb_EstimatedDuration.Focus();
                return;
            }

            int busyTime = controller.GetEstimatedDuration(executionDate, taskId);
            if (busyTime + estimatedDuration > 28800)
            {
                if (MessageBox.Show("Суммарная оценочная длительность задач в заданный день превышает 8 часов. Продолжить?", controller.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    tb_EstimatedDuration.Focus();
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            bool success = true;
            CController.DProgressChanged progressChanged = null;
            CController.DWorkerCompleted workerCompleted = null;
            FProcess form = null;
            try
            {
                if (files.Count > 0)
                {
                    Enabled = false;

                    form = new FProcess(Location, Size, ref progressChanged, ref workerCompleted);
                    form.FormClosed += (ss, ee) => { form.Dispose(); };
                    form.Show();

                    controller.UpdateTask(taskId, name, customer, comment, executionDate, estimatedDuration, reminders, files, progressChanged, workerCompleted);

                    progressChanged = null;
                    workerCompleted = null;
                }
                else
                {
                    controller.UpdateTask(taskId, name, customer, comment, executionDate, estimatedDuration, reminders, files, null, null);
                }
            }
            catch (Exception ex)
            {
                success = false;
                MessageBox.Show(ex.Message, controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Focus();
            }
            finally
            {
                if (form != null && !form.IsDisposed)
                    form.Dispose();

                Enabled = true;
                Cursor.Current = Cursors.Default;
            }

            if (success)
            {
                Tag = true;
                Close();
            }
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateReminderList()
        {
            hlb_Reminders.Clear();

            for (int i = 0; i < reminders.Count; i++)
            {
                CReminder r = reminders[i];

                hlb_Reminders.AddItem(controller.GetReminder(r.Seconds, "За"), r.Shown);
            }
        }

        private void RemoveReminder(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Control c = sender as Control;
            Control parent = c.Parent;
            int idx = parent.Controls.IndexOf(c);

            CRoundButtonWithCross b = c as CRoundButtonWithCross;

            if (b.IsCrossEnter)
            {
                if
                (
                    MessageBox.Show
                    (
                        $"Вы действительно хотите удалить уведомление \"{ b.Text }\"?",
                        controller.Title,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button3
                    ) != DialogResult.Yes
                )
                    return;

                reminders.RemoveAt(idx);

                b_AddReminder.Enabled = reminders.Count < 3;

                b_ClearReminders.Enabled = reminders.Count > 0;

                hlb_Reminders.Remove(c);
            }
            else
            {
                DateTime executionDate = GetExecutionDateTime();
                if (executionDate == DateTime.MinValue)
                    return;

                CReminder reminder = null;

                using (FAddReminder form = new FAddReminder(controller, reminders[idx], executionDate))
                {
                    form.ShowDialog();

                    if (form.Tag != null)
                        reminder = form.Tag as CReminder;
                }

                if (reminder != null)
                {
                    reminders[idx] = reminder;
                    UpdateReminderList();
                }
            }
        }

        private void UpdateFileList()
        {
            lb_Files.Items.Clear();

            int filesCount = files.Count;
            for (int i = 0; i < filesCount; i++)
                lb_Files.Items.Add(files[i]);

            groupBox6.Text = (filesCount > 0) ? $"Файлы [ { filesCount } ]" : "Файлы";
        }

        private string GetEstimatedDuration(int value)
        {
            string result = string.Empty;

            if (value % 86400 == 0)
            {
                result = $"{ value / 86400 }";
                cb_TimeUnit.SelectedIndex = 2;
            }
            else if (value % 3600 == 0)
            {
                result = $"{ value / 3600 }";
                cb_TimeUnit.SelectedIndex = 1;
            }
            else if (value % 60 == 0)
            {
                result = $"{ value / 60 }";
                cb_TimeUnit.SelectedIndex = 0;
            }

            return result;
        }

        private DateTime GetExecutionDateTime()
        {
            DateTime executionDate = DateTime.MinValue;

            DateTime date = dtp_Date.Value;
            DateTime time = dtp_Time.Value;

            string execDate = $"{ date.Day }.{ date.Month }.{ date.Year } { time.Hour }:{ time.Minute }:{ time.Second }";

            if (DateTime.TryParse(execDate, out DateTime newExecDate) && newExecDate > DateTime.Now)
                executionDate = newExecDate;
            else
            {
                MessageBox.Show("Неверная дата или время срока исполнения", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtp_Date.Focus();
            }

            return executionDate;
        }
    }
}
