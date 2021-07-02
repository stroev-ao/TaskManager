using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FFilterByCustomer : Form
    {
        CController controller;

        public FFilterByCustomer(CController controller)
        {
            InitializeComponent();

            this.controller = controller;
        }

        private void FFilterByCustomer_Load(object sender, EventArgs e)
        {
            clb_Customers.Items.AddRange(controller.GetCustomerNames());

            if (controller.ActiveFilters.ContainsKey(CController.EFilter.ByCustomer))
            {
                int[] idxs = controller.ActiveFilters[CController.EFilter.ByCustomer].Select(f => Convert.ToInt32(f)).ToArray();

                for (int i = 0; i < idxs.Length; i++)
                    clb_Customers.SetItemChecked(idxs[i], true);
            }
        }

        private void FFilterByCustomer_Shown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }

        private void tsb_CkechAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clb_Customers.Items.Count; i++)
                clb_Customers.SetItemChecked(i , true);
        }

        private void tsb_UncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clb_Customers.Items.Count; i++)
                clb_Customers.SetItemChecked(i, false);
        }

        private void tstb_Filter_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string text = tstb_Filter.Text;

            clb_Customers.Items.Clear();

            clb_Customers.Items.AddRange(controller.GetCustomerNames().Where(s => Regex.IsMatch(s.ToLower(), $"^{ text }")).ToArray());

            Cursor.Current = Cursors.Default;
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            int count = clb_Customers.CheckedItems.Count;
            
            if (count > 0)
            {
                string[] names = new string[count];

                for (int i = 0; i < count; i++)
                    names[i] = clb_Customers.CheckedItems[i].ToString();

                controller.SetFilterByCustomer(names);
            }

            Close();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
