using System.Windows.Forms;
using System.Drawing;

namespace TaskManager
{
    public class CCustomListBoxFile : ListBox
    {
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;

            int idx = e.Index;
            if (idx < 0)
                return;

            using (StringFormat sf = new StringFormat() { Trimming = StringTrimming.EllipsisPath })
                using (SolidBrush b = new SolidBrush(Color.Red))
                    g.DrawString(Items[idx].ToString(), e.Font, b, e.Bounds, sf);

            base.OnDrawItem(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
                e.Graphics.DrawRectangle(Pens.Red, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);

            base.OnPaint(e);
        }
    }
}
