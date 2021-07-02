using System;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FAbout : Form
    {
        public FAbout()
        {
            InitializeComponent();
        }

        private void FAbout_Load(object sender, EventArgs e)
        {
            pb_Logo.Image = Properties.Resources.logo1;
        }

        private void b_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
