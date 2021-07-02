using System.Windows.Forms;
using System.Drawing;

namespace TaskManager
{
    public partial class FProcess : Form
    {
        private const string TITLE = "Копирование файлов";

        public FProcess(Point location, Size size, ref CController.DProgressChanged progressChanged, ref CController.DWorkerCompleted workerCompleted)
        {
            InitializeComponent();

            Location = new Point(location.X + size.Width / 2 - Width / 2, location.Y + size.Height / 2 - Height / 2);

            progressChanged += OnProgressChanged;
            workerCompleted += OnWorkerCompleted;
        }

        private void FProcess_Load(object sender, System.EventArgs e)
        {
            Text = TITLE;
        }

        private void OnProgressChanged(int percent)
        {
            Text = TITLE + $" [{ percent }%]";
            pb_Main.Value = percent;
        }

        private void OnWorkerCompleted()
        {
            Close();
        }
    }
}
