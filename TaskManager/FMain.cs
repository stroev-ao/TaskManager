using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
using System.Drawing;

namespace TaskManager
{
    public partial class FMain : Form
    {
        private CController controller;

        private Timer timerReminder;//, timerUpdater;

        private bool showActive;
        private bool showPaused;
        private bool showDone;
        private bool showOutstanding;

        private enum EOrder { LessMore = 0, MoreLess = 1 };
        
        private bool orderByCreationDate;
        private EOrder creationDateOrder;

        private bool orderByExecutionDate;
        private EOrder executionDateOrder;

        private bool orderByPriority;
        private EOrder priorityOrder;

        private bool orderByEstimatedDuration;
        private EOrder estimatedDurationOrder;

        private bool launchInTray, realExit;

        public FMain(string[] args)
        {
            if (args.Length > 0)
                launchInTray = args.Contains("-t") ? true : false;

            InitializeComponent();

            try
            {
                Program.controller = new CController();
                controller = Program.controller;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (controller == null)
                    Environment.Exit(0);
            }

            Text = ni_Main.Text = controller.Title;
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            showActive = Properties.Settings.Default.showActive;
            showPaused = Properties.Settings.Default.showPaused;
            showDone = Properties.Settings.Default.showDone;
            showOutstanding = Properties.Settings.Default.showOutstanding;

            bool lessMore;

            orderByCreationDate = Properties.Settings.Default.orderByCreationDate;
            creationDateOrder = (EOrder)Properties.Settings.Default.creationDateOrder;
            if (orderByCreationDate)
            {
                lessMore = creationDateOrder == EOrder.LessMore;
                controller.Order = lessMore ? CController.EOrder.ByCreationDateDU : CController.EOrder.ByCreationDateUD;
                tsmi_OrderByCreationDate.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;
                tsmi_OrderByCreationDate.Checked = orderByCreationDate;
            }

            orderByExecutionDate = Properties.Settings.Default.orderByExecutionDate;
            executionDateOrder = (EOrder)Properties.Settings.Default.executionDateOrder;
            if (orderByExecutionDate)
            {
                lessMore = executionDateOrder == EOrder.LessMore;
                controller.Order = lessMore ? CController.EOrder.ByExecutionDateUD : CController.EOrder.ByExecutionDateDU;
                tsmi_OrderByExecutionDate.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;
                tsmi_OrderByExecutionDate.Checked = orderByExecutionDate;
            }

            orderByPriority = Properties.Settings.Default.orderByPriority;
            priorityOrder = (EOrder)Properties.Settings.Default.priorityOrder;
            if (orderByPriority)
            {
                lessMore = priorityOrder == EOrder.LessMore;
                controller.Order = lessMore ? CController.EOrder.ByPriorityUD : CController.EOrder.ByPriorityDU;
                tsmi_OrderByPriority.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;
                tsmi_OrderByPriority.Checked = orderByPriority;
            }

            orderByEstimatedDuration = Properties.Settings.Default.orderByEstimatedDuration;
            estimatedDurationOrder = (EOrder)Properties.Settings.Default.estimatedDurationOrder;
            if (orderByEstimatedDuration)
            {
                lessMore = estimatedDurationOrder == EOrder.LessMore;
                controller.Order = lessMore ? CController.EOrder.ByEstimatedDurationUD : CController.EOrder.ByEstimatedDurationDU;
                tsmi_OrderByEstimatedDuration.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;
                tsmi_OrderByEstimatedDuration.Checked = orderByEstimatedDuration;
            }

            tsmi_ShowActive.Checked = showActive;
            tsmi_ShowPaused.Checked = showPaused;
            tsmi_ShowDone.Checked = showDone;
            tsmi_ShowOutstanding.Checked = showOutstanding;

            lb_Tasks.SelectedIndex = -1;
            lb_Tasks.OnMouseClick1 += OnTaskClick;
            lb_Tasks_SelectedIndexChanged(lb_Tasks, new EventArgs());

            timerReminder = new Timer() { Interval = 60000 };
            timerReminder.Tick += timerReminder_Tick;

            //timerUpdater = new Timer() { Interval = 5000 };
            //timerUpdater.Tick += (ss, ee) => { lb_Tasks.Invalidate(); };

            ni_Main.Visible = false;

            mi_Autorun.Checked = Properties.Settings.Default.autorun;

            UpdateTaskList();

            l_ActiveFilter.Visible = false;
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;

            //timerUpdater.Start();
            timerReminder.Start();

            if (launchInTray)
            {
                HideForm();

                int count = controller.GetTaskCountByExecutionDate(DateTime.Now);

                if (count > 0)
                    ni_Main.ShowBalloonTip
                    (
                        5000,
                        "Информация",
                        $"Запланировано задач на сегодня: { count }",
                        ToolTipIcon.Info
                    );
            }
        }

        private void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized && !realExit)
            {
                e.Cancel = true;

                HideForm();
                
                return;
            }

            ni_Main.Visible = false;

            //timerUpdater.Stop();
            //timerUpdater.Dispose();

            timerReminder.Stop();
            timerReminder.Dispose();

            Cursor.Current = Cursors.WaitCursor;

            Properties.Settings.Default.orderByCreationDate = orderByCreationDate;
            Properties.Settings.Default.creationDateOrder = (int)creationDateOrder;

            Properties.Settings.Default.orderByExecutionDate = orderByExecutionDate;
            Properties.Settings.Default.executionDateOrder = (int)executionDateOrder;

            Properties.Settings.Default.orderByPriority = orderByPriority;
            Properties.Settings.Default.priorityOrder = (int)priorityOrder;

            Properties.Settings.Default.orderByEstimatedDuration = orderByEstimatedDuration;
            Properties.Settings.Default.estimatedDurationOrder = (int)estimatedDurationOrder;

            Properties.Settings.Default.Save();

            try
            {
                controller.SaveTasksAndConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void timerReminder_Tick(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                ni_Main.Visible = false;

            string message = controller.GetActiveReminder();
            if (message == null)
                return;

            if (WindowState != FormWindowState.Minimized)
                ni_Main.Visible = true;

            ni_Main.ShowBalloonTip(5000, "Внимание", message, ToolTipIcon.Info);
        }

        private void tsb_AddTask_Click(object sender, EventArgs e)
        {
            int id = controller.AddTask();

            using (FEditTask form = new FEditTask(controller, id))
            {
                form.ShowDialog();

                if (form.Tag == null)
                    controller.RemoveTask(id);
                else
                {
                    Cursor.Current = Cursors.WaitCursor;

                    UpdateTaskList();

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void tsb_EditTask_Click(object sender, EventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            EditTask(t.ID);
        }

        private void tsb_RemoveTask_Click(object sender, EventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            string taskName = t.Name;

            switch (MessageBox.Show($"Вы действительно хотите удалить задачу \"{ taskName }\"?", controller.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
            {
                case DialogResult.Cancel:
                case DialogResult.No:
                    return;
            }

            bool deleteFiles = false;

            int filesCount = t.Files.Count;
            if (filesCount > 0)
                switch (MessageBox.Show($"Удалить файлы, связанные с задачей \"{ taskName }\" ({ filesCount } шт.)?", controller.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
                {
                    case DialogResult.Yes:
                        deleteFiles = true;
                        break;
                    case DialogResult.Cancel:
                        return;
                }

            try
            {
                controller.RemoveTask(t.ID, deleteFiles);

                UpdateTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsddb_SetStatus_DropDownOpening(object sender, EventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            bool value = t.Status != CTask.EStatus.Завершена;

            tsddb_SetStatus.DropDownItems[0].Enabled = t.Status != CTask.EStatus.Активная && value;
            tsddb_SetStatus.DropDownItems[1].Enabled = t.Status != CTask.EStatus.Приостановлена && value;
            tsddb_SetStatus.DropDownItems[2].Enabled = value;
        }

        private void tsmi_Active_Click(object sender, EventArgs e)
        {
            SetTaskStatus(CTask.EStatus.Активная);
        }

        private void tsmi_Paused_Click(object sender, EventArgs e)
        {
            SetTaskStatus(CTask.EStatus.Приостановлена);
        }

        private void tsmi_Done_Click(object sender, EventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            switch (MessageBox.Show($"Вы действительно хотите завершить задачу \"{ t.Name }\"? Данное действие необратимо.", controller.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
            {
                case DialogResult.Yes:
                    SetTaskStatus(CTask.EStatus.Завершена, t);
                    break;
            }
        }

        private void tsddb_SetPriority_DropDownOpening(object sender, EventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            bool value = t.Status != CTask.EStatus.Завершена;

            foreach (ToolStripMenuItem i in tsddb_SetPriority.DropDownItems)
                i.Enabled = value;
        }

        private void tsmi_SetPriority0_Click(object sender, EventArgs e)
        {
            SetPriority(0);
        }

        private void tsmi_SetPriority1_Click(object sender, EventArgs e)
        {
            SetPriority(1);
        }

        private void tsmi_SetPriority2_Click(object sender, EventArgs e)
        {
            SetPriority(2);
        }

        private void tsmi_SetPriority3_Click(object sender, EventArgs e)
        {
            SetPriority(3);
        }

        private void tsmi_SetPriority4_Click(object sender, EventArgs e)
        {
            SetPriority(4);
        }

        private void tsmi_SetPriority5_Click(object sender, EventArgs e)
        {
            SetPriority(5);
        }

        private void tsmi_SetPriority6_Click(object sender, EventArgs e)
        {
            SetPriority(6);
        }

        private void tsmi_SetPriority7_Click(object sender, EventArgs e)
        {
            SetPriority(7);
        }

        private void tsmi_SetPriority8_Click(object sender, EventArgs e)
        {
            SetPriority(8);
        }

        private void tsmi_SetPriority9_Click(object sender, EventArgs e)
        {
            SetPriority(9);
        }

        private void tsmi_ShowActive_Click(object sender, EventArgs e)
        {
            tsmi_ShowActive.Checked = !tsmi_ShowActive.Checked;

            showActive = tsmi_ShowActive.Checked;

            Properties.Settings.Default.showActive = showActive;

            UpdateTaskList();
        }

        private void tsmi_ShowPaused_Click(object sender, EventArgs e)
        {
            tsmi_ShowPaused.Checked = !tsmi_ShowPaused.Checked;

            showPaused = tsmi_ShowPaused.Checked;

            Properties.Settings.Default.showPaused = showPaused;

            UpdateTaskList();
        }

        private void tsmi_ShowDone_Click(object sender, EventArgs e)
        {
            tsmi_ShowDone.Checked = !tsmi_ShowDone.Checked;

            showDone = tsmi_ShowDone.Checked;

            Properties.Settings.Default.showDone = showDone;

            UpdateTaskList();
        }

        private void tsmi_ShowOutstanding_Click(object sender, EventArgs e)
        {
            tsmi_ShowOutstanding.Checked = !tsmi_ShowOutstanding.Checked;

            showOutstanding = tsmi_ShowOutstanding.Checked;

            Properties.Settings.Default.showOutstanding = showOutstanding;

            UpdateTaskList();
        }

        private void lb_Tasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool value = lb_Tasks.SelectedIndex >= 0;

            tsb_EditTask.Enabled = value;
            tsb_RemoveTask.Enabled = value;
            tsddb_SetStatus.Enabled = value;
            tsddb_SetPriority.Enabled = value;
        }

        private void lb_Tasks_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            int idx = lb_Tasks.IndexFromPoint(e.Location);
            if (idx != ListBox.NoMatches)
            {
                CTask t = lb_Tasks.Items[idx] as CTask;
                if (t == null)
                    return;

                EditTask(t.ID);
            }
        }

        private void lb_Tasks_MouseDown(object sender, MouseEventArgs e)
        {
            int idx = lb_Tasks.IndexFromPoint(e.Location);

            if (idx != ListBox.NoMatches)
                lb_Tasks.SelectedIndex = idx;
        }

        private void lb_Tasks_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            int idx = lb_Tasks.IndexFromPoint(e.Location);

            if (idx != ListBox.NoMatches)
                cms_Task.Show(lb_Tasks, e.Location);
        }

        private void ni_Main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsmi_ShowMainWindow_Click(tsmi_ShowMainWindow, new EventArgs());
        }

        private void tsmi_ShowMainWindow_Click(object sender, EventArgs e)
        {
            //timerUpdater.Start();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            ni_Main.Visible = false;
        }

        private void tsmi_AddTask_Click(object sender, EventArgs e)
        {
            tsmi_ShowMainWindow_Click(tsmi_ShowMainWindow, e);

            tsb_AddTask_Click(tsb_AddTask, e);
        }

        private void tsmi_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cms_Task_Opening(object sender, CancelEventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
            {
                e.Cancel = true;
                return;
            }

            bool value = t.Status != CTask.EStatus.Завершена;

            cms_Task.Items[1].Enabled = value;
            cms_Task.Items[2].Enabled = value;
            cms_Task.Items[4].Enabled = t.Files.Count > 0;
        }

        private void tsmi_SetStatus_DropDownOpening(object sender, EventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            bool value = t.Status != CTask.EStatus.Завершена;

            tsmi_SetStatus.DropDownItems[0].Enabled = t.Status != CTask.EStatus.Активная && value;
            tsmi_SetStatus.DropDownItems[1].Enabled = t.Status != CTask.EStatus.Приостановлена && value;
            tsmi_SetStatus.DropDownItems[2].Enabled = value;
        }

        private void tsmi_ShowFiles_Click(object sender, EventArgs e)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            Cursor.Current = Cursors.WaitCursor;

            controller.OpenFolder(t.ID);

            Cursor.Current = Cursors.Default;
        }

        private void tsmi_RemoveTask_Click(object sender, EventArgs e)
        {
            tsb_RemoveTask_Click(tsb_RemoveTask, e);
        }

        private void tsmi_OrderByCreationDate_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            bool lessMore = creationDateOrder == EOrder.LessMore;

            if (orderByCreationDate)
            {
                creationDateOrder = lessMore ? EOrder.MoreLess : EOrder.LessMore;

                lessMore = creationDateOrder == EOrder.LessMore;
            }
            else
            {
                orderByCreationDate = true;
                orderByExecutionDate = false;
                orderByPriority = false;
                orderByEstimatedDuration = false;

                foreach (ToolStripMenuItem i in item.Owner.Items)
                    i.Checked = false;

                item.Checked = true;
            }

            controller.Order = lessMore ? CController.EOrder.ByCreationDateDU : CController.EOrder.ByCreationDateUD;
            item.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;

            UpdateTaskList();
        }

        private void tsmi_OrderByExecutionDate_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            bool lessMore = executionDateOrder == EOrder.LessMore;

            if (orderByExecutionDate)
            {
                executionDateOrder = lessMore ? EOrder.MoreLess : EOrder.LessMore;

                lessMore = executionDateOrder == EOrder.LessMore;
            }
            else
            {
                orderByCreationDate = false;
                orderByExecutionDate = true;
                orderByPriority = false;
                orderByEstimatedDuration = false;

                foreach (ToolStripMenuItem i in item.Owner.Items)
                    i.Checked = false;

                item.Checked = true;
            }

            controller.Order = lessMore ? CController.EOrder.ByExecutionDateUD : CController.EOrder.ByExecutionDateDU;
            item.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;

            UpdateTaskList();
        }

        private void tsmi_OrderByPriority_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            bool lessMore = priorityOrder == EOrder.LessMore;

            if (orderByPriority)
            {
                priorityOrder = lessMore ? EOrder.MoreLess : EOrder.LessMore;

                lessMore = priorityOrder == EOrder.LessMore;
            }
            else
            {
                orderByCreationDate = false;
                orderByExecutionDate = false;
                orderByPriority = true;
                orderByEstimatedDuration = false;

                foreach (ToolStripMenuItem i in item.Owner.Items)
                    i.Checked = false;

                item.Checked = true;
            }

            controller.Order = lessMore ? CController.EOrder.ByPriorityUD : CController.EOrder.ByPriorityDU;
            item.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;

            UpdateTaskList();
        }

        private void tsmi_OrderByEstimatedDuration_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            bool lessMore = estimatedDurationOrder == EOrder.LessMore;

            if (orderByEstimatedDuration)
            {
                estimatedDurationOrder = lessMore ? EOrder.MoreLess : EOrder.LessMore;

                lessMore = estimatedDurationOrder == EOrder.LessMore;
            }
            else
            {
                orderByCreationDate = false;
                orderByExecutionDate = false;
                orderByPriority = false;
                orderByEstimatedDuration = true;

                foreach (ToolStripMenuItem i in item.Owner.Items)
                    i.Checked = false;

                item.Checked = true;
            }

            controller.Order = lessMore ? CController.EOrder.ByEstimatedDurationUD : CController.EOrder.ByEstimatedDurationDU;
            item.Image = lessMore ? Properties.Resources.du : Properties.Resources.ud;

            UpdateTaskList();
        }

        private void UpdateTaskList()
        {
            bool selected = lb_Tasks.SelectedItem != null;

            int id = -1;

            if (selected)
                id = (lb_Tasks.SelectedItem as CTask).ID;

            lb_Tasks.BeginUpdate();

            lb_Tasks.Items.Clear();

            CTask[] tasks = controller.GetTasks(showActive, showPaused, showDone, showOutstanding);

            lb_Tasks.Items.AddRange(tasks);

            if (selected)
            {
                CTask t = tasks.FirstOrDefault(el => el.ID == id);

                if (t != null)
                    lb_Tasks.SelectedIndex = tasks.ToList().IndexOf(t);
            }

            lb_Tasks.EndUpdate();
        }

        private void EditTask(int id)
        {
            using (FEditTask form = new FEditTask(controller, id))
            {
                form.ShowDialog();

                if (form.Tag != null)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    UpdateTaskList();

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void SetPriority(int priority)
        {
            CTask t = lb_Tasks.SelectedItem as CTask;
            if (t == null)
                return;

            Cursor.Current = Cursors.WaitCursor;

            controller.SetPriority(t.ID, priority);

            Cursor.Current = Cursors.Default;

            lb_Tasks.Invalidate();
        }

        private void OnTaskClick(int taskId)
        {
            Cursor.Current = Cursors.WaitCursor;

            controller.OpenFolder(taskId);

            Cursor.Current = Cursors.Default;
        }

        private void mi_StoragePath_Click(object sender, EventArgs e)
        {
            using (FSetStoragePath form = new FSetStoragePath(controller))
                form.ShowDialog();
        }

        private void mi_Exit_Click(object sender, EventArgs e)
        {
            realExit = true;
            Close();
        }

        private void mi_About_Click(object sender, EventArgs e)
        {
            using (FAbout form = new FAbout())
                form.ShowDialog();
        }

        private void SetTaskStatus(CTask.EStatus status, CTask t = null)
        {
            if (t == null)
            {
                t = lb_Tasks.SelectedItem as CTask;
                
                if (t == null)
                    return;
            }

            Cursor.Current = Cursors.WaitCursor;

            controller.SetTaskStatus(t.ID, status);

            Cursor.Current = Cursors.Default;

            UpdateTaskList();
        }

        private void mi_Autorun_Click(object sender, EventArgs e)
        {
            bool value = !mi_Autorun.Checked;

            string title = controller.Title;

            try
            {
                controller.SetAutorun(value);

                mi_Autorun.Checked = value;

                string msg = value ? $"{ title } успешно добавлена в список автозагрузки" : $"{ title } успешно убрана из списка автозагрузки";

                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HideForm()
        {
            //timerUpdater.Stop();
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            switch (controller.GetTaskCountByExecutionDate(DateTime.Now))
            {
                case 0:
                    ni_Main.Icon = Properties.Resources.ni0;
                    break;
                case 1:
                    ni_Main.Icon = Properties.Resources.ni1;
                    break;
                case 2:
                    ni_Main.Icon = Properties.Resources.ni2;
                    break;
                case 3:
                    ni_Main.Icon = Properties.Resources.ni3;
                    break;
                case 4:
                    ni_Main.Icon = Properties.Resources.ni4;
                    break;
                case 5:
                    ni_Main.Icon = Properties.Resources.ni5;
                    break;
                case 6:
                    ni_Main.Icon = Properties.Resources.ni6;
                    break;
                case 7:
                    ni_Main.Icon = Properties.Resources.ni7;
                    break;
                case 8:
                    ni_Main.Icon = Properties.Resources.ni8;
                    break;
                case 9:
                    ni_Main.Icon = Properties.Resources.ni9;
                    break;
                default:
                    ni_Main.Icon = Properties.Resources.ni10;
                    break;
            }

            ni_Main.Visible = true;
        }

        private void tsmi_FilterByCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (FFilterByCustomer form = new FFilterByCustomer(controller))
                form.ShowDialog();

            UpdateActiveFilters();

            UpdateTaskList();
        }

        private void tsmi_ClearFilters_Click(object sender, EventArgs e)
        {
            controller.ActiveFilters.Clear();

            UpdateActiveFilters();

            UpdateTaskList();
        }

        private void UpdateActiveFilters()
        {
            int count = controller.ActiveFilters.Count;

            if (count == 0)
            {
                l_ActiveFilter.Visible = false;
                l_ActiveFilter.Text = "";
            }
            else
            {
                string filterText = "";

                for (int i = 0; i < count; i++)
                {
                    switch (controller.ActiveFilters.ElementAt(i).Key)
                    {
                        case CController.EFilter.ByCustomer:
                            {
                                if (!string.IsNullOrEmpty(filterText))
                                    filterText += "\n";

                                filterText += "Фильтр по постановщику: ";

                                int[] idxs = controller.ActiveFilters.ElementAt(i).Value.Select(v => Convert.ToInt32(v)).ToArray();

                                int len = idxs.Length;

                                for (int j = 0; j < len; j++)
                                {
                                    filterText += controller.GetCustomerName(idxs[j]);

                                    if (j < len - 1)
                                        filterText += "; ";
                                }

                                break;
                            }
                        case CController.EFilter.ByExecutionDate:
                            {
                                if (!string.IsNullOrEmpty(filterText))
                                    filterText += "\n";

                                filterText += "Фильтр по сроку исполнения: ";

                                DateTime[] dates = controller.ActiveFilters.ElementAt(i).Value.Select(v => Convert.ToDateTime(v)).ToArray();

                                int len = dates.Length;

                                for (int j = 0; j < len; j++)
                                {
                                    filterText += dates[j].GetShortDate();

                                    if (j < len - 1)
                                        filterText += "; ";
                                }

                                break;
                            }
                    }
                }

                l_ActiveFilter.Text = filterText;

                using (Graphics g = CreateGraphics())
                    l_ActiveFilter.Height = Convert.ToInt32(g.MeasureString(filterText, Font).Height);

                l_ActiveFilter.Visible = true;
            }
        }

        private void tsddb_Filters_DropDownOpening(object sender, EventArgs e)
        {
            tsmi_ClearFilters.Enabled = controller.ActiveFilters.Count > 0;
        }

        private void tsmi_FilterByExecutionDate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (FFilterByExecutionDate form = new FFilterByExecutionDate(controller))
                form.ShowDialog();

            UpdateActiveFilters();

            UpdateTaskList();
        }

        private void FMain_ResizeEnd(object sender, EventArgs e)
        {
            lb_Tasks.Invalidate();
        }

        

        private void tsmi_News_Click(object sender, EventArgs e)
        {
            using (FNews form = new FNews())
                form.ShowDialog();
        }
    }
}
