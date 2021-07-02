using System;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FSetStoragePath : Form
    {
        CController controller;

        public FSetStoragePath(CController controller)
        {
            InitializeComponent();

            this.controller = controller;
        }

        private void FSetStoragePath_Load(object sender, EventArgs e)
        {
            tb_StoragePath.Text = Properties.Settings.Default.storagePath;
        }

        private void FSetStoragePath_Shown(object sender, EventArgs e)
        {
            string path = tb_StoragePath.Text;
            
            if (!controller.IsFolderExists(path))
                MessageBox.Show($"Путь \"{ path }\" не существует", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void b_Browse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { SelectedPath = tb_StoragePath.Text })
                if (fbd.ShowDialog() == DialogResult.OK)
                    tb_StoragePath.Text = fbd.SelectedPath;
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            string path = tb_StoragePath.Text;

            if (!CheckPath(path))
                return;

            string oldPath = Properties.Settings.Default.storagePath;

            if (oldPath != path)
            {
                if (!controller.IsFolderEmpty(Properties.Settings.Default.storagePath))
                {
                    if (MessageBox.Show("Перенести файлы и папки хранилища в новое расположение?", controller.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        CController.DProgressChanged progressChanged = null;
                        CController.DWorkerCompleted workerCompleted = null;
                        FProcess form = null;

                        try
                        {
                            Enabled = false;

                            form = new FProcess(Location, Size, ref progressChanged, ref workerCompleted);
                            form.FormClosed += (ss, ee) => { form.Dispose(); };
                            form.Show();

                            controller.CopyStorage(path, progressChanged, workerCompleted);

                            progressChanged = null;
                            workerCompleted = null;

                            MessageBox.Show("Хранилище успешно перенесено", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        finally
                        {
                            if (form != null && !form.IsDisposed)
                                form.Dispose();

                            Enabled = true;
                        }
                    }
                }

                controller.SetStoragePath(path);
            }

            Close();
        }

        private bool CheckPath(string path)
        {
            bool exists = controller.IsFolderExists(path);

            if (!exists)
                MessageBox.Show($"Путь \"{ path }\" не существует", controller.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return exists;
        }
    }
}
