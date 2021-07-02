using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TaskManager
{
    class CHorizontalListBox : Control
    {
        public delegate void DOnItemClick(object sender, MouseEventArgs e);
        
        public DOnItemClick OnItemClick { get; set; }

        private bool init;

        public CHorizontalListBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true);

            DoubleBuffered = true;

            Dock = DockStyle.Top;
            MinimumSize = new Size(Width, 29);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (init)
                return;

            if (Width > 0)
                SortItems();

            foreach (Control c in Controls)
                c.Invalidate();

            Invalidate();

            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle main = e.ClipRectangle;

            using (SolidBrush brush = new SolidBrush(BackColor))
                g.FillRectangle(brush, main);

            if (DesignMode)
                using (Pen pen = new Pen(Color.Red))
                    g.DrawRectangle(pen, main.X, main.Y, main.Width - 1, main.Height - 1);

            base.OnPaint(e);
        }

        public void AddItem(string item, object tag = null)
        {
            CRoundButtonWithCross b = new CRoundButtonWithCross()
            {
                Text = item,
                Parent = this,
                Tag = tag
            };

            b.MouseClick += (ss, ee) => { OnItemClick(ss, ee); };

            //int x = 0;
            //foreach (Control c in Controls)
            //    if (c != b)
            //        x += c.Margin.Left + c.Width + c.Margin.Right;
            
            //b.Location = new Point(x + b.Margin.Left, b.Margin.Top);

            Controls.Add(b);

            SortAndInvalidate();
        }

        public void Remove(Control item)
        {
            if (!Controls.Contains(item))
                throw new Exception($"{ Name } does not contains item { item.ToString() }");

            Controls.Remove(item);

            SortAndInvalidate();
        }

        public void Clear()
        {
            Controls.Clear();

            Invalidate();
        }

        private void SortItems()
        {
            int availableWidth = Width;
            int usedWidth = 0;
            int usedHeight = 0;

            int rowCount = 1;
            int itemOnCurrentRow = 0;

            bool godMode = false;

            init = true;

            for (int i = 0; i < Controls.Count; i++)
            {
                Control c = Controls[i];

                int width = c.Margin.Left + c.Width + c.Margin.Right;
                int height = c.Margin.Top + c.Height + c.Margin.Bottom;

                Height = height * rowCount;

                if (width < availableWidth || godMode)
                {
                    c.Location = new Point(usedWidth + c.Margin.Left, usedHeight + c.Margin.Top);

                    if (godMode)
                    {
                        usedHeight += height;
                        rowCount++;
                    }
                    else
                    {
                        usedWidth += width;
                        availableWidth -= width;

                        itemOnCurrentRow++;
                    }
                }
                else
                {
                    if (itemOnCurrentRow == 0)
                        godMode = true;
                    else
                    {
                        usedHeight += height;

                        rowCount++;
                        itemOnCurrentRow = 0;

                        usedWidth = 0;
                        availableWidth = Width;
                    }

                    i--;
                }
            }

            init = false;
        }

        private void SortAndInvalidate()
        {
            SortItems();

            Invalidate();
        }
    }

    class CRoundButtonWithCross : Control
    {
        private bool isMouseEnter, isMouseDown, isCrossEnter;
        private int side;
        private float offset = 4;

        public bool IsCrossEnter { get { return isCrossEnter; } }

        public CRoundButtonWithCross()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true);

            DoubleBuffered = true;

            side = -1;

            ChangeSize();

            side = Math.Min(Width - 1, Height - 1);

            ChangeSize();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            ChangeSize();

            base.OnFontChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            ChangeSize();

            base.OnTextChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            Rectangle main = e.ClipRectangle;

            Color color1 = Parent.BackColor;
            Color light = GetColor(color1, 1.1);
            Color light1 = GetColor(color1, 1.2);
            Color dark = GetColor(color1, 0.9);
            Color dark1 = GetColor(color1, 0.7);

            Color colorCurrent = !Enabled ? dark1 :
                isMouseEnter & isMouseDown ? dark :
                isMouseEnter ? light1 :
                light;

            if (Tag != null && Convert.ToBoolean(Tag))
                colorCurrent = dark;

            using (SolidBrush b = new SolidBrush(colorCurrent))
            using (GraphicsPath path = new GraphicsPath())
            {
                float side = Math.Min(Width - 1, Height - 1);
                float half = side / 2;

                float x = main.X;
                float y = main.Y;
                float right = main.Width - side - 1;

                path.AddArc(x, y, side, side, 90, 180);
                path.AddLine(x + side, y, right, y);
                path.AddArc(right, y, side, side, 270, 180);

                g.FillPath(b, path);
            }

            using (SolidBrush b = new SolidBrush(Enabled ? Parent.ForeColor : Color.FromArgb(64, 64, 64)))
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Trimming = StringTrimming.EllipsisCharacter;

                g.DrawString(Text, Parent.Font, b, new Rectangle(main.X + 6, main.Y, main.Width - 1 - side, main.Height), sf);
            }

            if (isCrossEnter)
                using (SolidBrush b = new SolidBrush(dark))
                {
                    float x = main.Width - 1 - side / 2 - offset / 2;
                    float y = main.Height / 2;
                    float d = offset + offset;
                    float q = d + d;

                    g.FillEllipse(b, x - d, y - d, q, q);
                }

            //using (Pen p = new Pen(isCrossEnter ? Color.Red : ForeColor))
            using (Pen p = new Pen(ForeColor))
            {
                float x = main.Width - 1 - side / 2 - offset / 2;
                float y = main.Height / 2;

                float x1 = x - offset;
                float y1 = y - offset;

                float x2 = x + offset;
                float y2 = y + offset;

                g.DrawLine(p, x1, y1, x2, y2);
                g.DrawLine(p, x2, y1, x1, y2);
            }

            if (DesignMode || isMouseEnter)
            {
                using (Pen p = new Pen(dark1))
                using (GraphicsPath path = new GraphicsPath())
                {
                    float side = Math.Min(Width - 1, Height - 1);
                    float half = side / 2;

                    float x = main.X;
                    float y = main.Y;
                    float right = main.Width - side - 1;
                    float bottom = main.Height - 1;

                    path.AddArc(x, y, side, side, 90, 180);
                    path.AddLine(x + side, y, right, y);
                    path.AddArc(right, y, side, side, 270, 180);
                    path.AddLine(right, bottom, x + half, bottom);

                    g.DrawPath(p, path);
                }
            }

            base.OnPaint(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            isMouseEnter = true;

            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isMouseEnter = false;

            Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;

                Invalidate();
            }

            base.OnMouseUp(e);
        }

        private Color GetColor(Color color, double scale)
        {
            int r = (int)(color.R * scale);
            r = r < 0 ? 0 : r > 255 ? 255 : r;

            int g = (int)(color.G * scale);
            g = g < 0 ? 0 : g > 255 ? 255 : g;

            int b = (int)(color.B * scale);
            b = b < 0 ? 0 : b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }

        private void ChangeSize()
        {
            using (Control c = new Control() { Font = this.Font })
            using (Graphics g = c.CreateGraphics())
            {
                SizeF size = g.MeasureString(Text, Font);

                int width = Math.Max(23, Convert.ToInt32(size.Width) + Margin.Left + Margin.Right);
                width += side > 0 ? side : 0;

                int height = Math.Max(23, Convert.ToInt32(size.Height) + Margin.Top + Margin.Bottom);

                Size = new Size(width, height);
            }

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            float x = Width - 1 - side / 2 - offset / 2;
            float y = Height / 2;
            float d = offset + offset;
            float q = d + d;

            RectangleF rect = new RectangleF(x - d, y - d, q, q);

            isCrossEnter = rect.Contains(e.Location);

            Invalidate();

            base.OnMouseMove(e);
        }
    }
}
