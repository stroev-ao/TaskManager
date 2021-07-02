using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TaskManager
{
    public class CCustomListBoxTask : ListBox
    {
        public delegate void DOnMouseClick(int taskId);
        DOnMouseClick onMouseClick;

        public DOnMouseClick OnMouseClick1 { get { return onMouseClick; } set { onMouseClick = value; } }

        public CCustomListBoxTask()
        {
            DoubleBuffered = true;

            //Activate double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            //Windows messages before they get to the form WndProc
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            DrawMode = DrawMode.OwnerDrawFixed;

            ItemHeight = FontHeight * 3 + 22;
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int idx = e.Index;
            if (idx < 0 || Items.Count == 0)
                return;

            CTask t = Items[idx] as CTask;
            if (t == null)
                return;

            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            double remain = t.ExecutionDate.Subtract(DateTime.Now).TotalSeconds;
            double total = t.ExecutionDate.Subtract(t.CreationDate).TotalSeconds;
            float percent = 1 - (float)(remain / total);

            if (percent > 1 && t.Status != CTask.EStatus.Завершена)
                t.Status = CTask.EStatus.Просрочена;

            bool outstanding = false;

            Color foreColor;
            switch (t.Status)
            {
                case CTask.EStatus.Приостановлена:
                case CTask.EStatus.Завершена:
                    foreColor = Color.FromKnownColor(KnownColor.GrayText);
                    break;
                case CTask.EStatus.Активная:
                default:
                    foreColor = ForeColor;
                    break;
                case CTask.EStatus.Просрочена:
                    {
                        outstanding = true;
                        foreColor = Color.Red;
                        break;
                    }
            }

            //если выделенный элемент
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs
                    (
                        e.Graphics,
                        e.Font,
                        e.Bounds,
                        e.Index,
                        e.State ^ DrawItemState.Selected,
                        foreColor,
                        Color.FromArgb(240, 240, 240)
                    );

                //using (Pen p = new Pen(Color.Red, 3)) //FromArgb(64, 64, 64)
                //{
                //    using (GraphicsPath path = new GraphicsPath())
                //    {
                //        float size = 8;

                //        float x = e.Bounds.X;
                //        //float y = e.Bounds.Y + e.Bounds.Height * e.Index;
                //        float y = e.Bounds.Y;
                //        float width = e.Bounds.Width - 1;
                //        float height = e.Bounds.Height;

                //        path.AddArc(new RectangleF(x, y, size, size), 180, 90);
                //        path.AddLine(x + size, y, width - size - size, y);
                //        path.AddArc(new RectangleF(width - size, y, size, size), 270, 90);
                //        path.AddLine(width, y + size, width, height - size);
                //        path.AddArc(new RectangleF(width - size, height - size, size, size), 0, 90);
                //        path.AddLine(width - size, height, x + size, height);
                //        path.AddArc(new RectangleF(x, height - size, size, size), 90, 90);
                //        path.AddLine(x, height - size, x, y + size / 2);

                //        g.DrawPath(p, path);
                //    }
                //}
            }

            e.DrawBackground();

            float maxWidth = e.Bounds.Width - 6;

            RectangleF rect = new RectangleF(e.Bounds.X + 3, e.Bounds.Y, 4 * maxWidth / 5, 13);

            //пишем название работы, статус и постановщика
            using (SolidBrush b = new SolidBrush(foreColor))
            {
                using (StringFormat sf = new StringFormat(StringFormatFlags.NoWrap) { Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter })
                {
                    using (Font f = new Font(Font, FontStyle.Bold))
                        g.DrawString(t.Name, f, b, rect, sf);

                    //rect.Width = maxWidth;

                    float x = e.Bounds.Width - 16 - 3;
                    float y = e.Bounds.Y + 3;
                    float width = 16;
                    float height = 16;

                    //рисуем приоритет задачи
                    switch (t.Priority)
                    {
                        case 0:
                            g.DrawImage(Properties.Resources._0, x, y, width, height);
                            break;
                        case 1:
                            g.DrawImage(Properties.Resources._1, x, y, width, height);
                            break;
                        case 2:
                            g.DrawImage(Properties.Resources._2, x, y, width, height);
                            break;
                        case 3:
                            g.DrawImage(Properties.Resources._3, x, y, width, height);
                            break;
                        case 4:
                            g.DrawImage(Properties.Resources._4, x, y, width, height);
                            break;
                        case 5:
                            g.DrawImage(Properties.Resources._5, x, y, width, height);
                            break;
                        case 6:
                            g.DrawImage(Properties.Resources._6, x, y, width, height);
                            break;
                        case 7:
                            g.DrawImage(Properties.Resources._7, x, y, width, height);
                            break;
                        case 8:
                            g.DrawImage(Properties.Resources._8, x, y, width, height);
                            break;
                        case 9:
                            g.DrawImage(Properties.Resources._9, x, y, width, height);
                            break;
                    }

                    x -= 16 + 6;

                    //рисуем статус задачи
                    switch (t.Status)
                    {
                        case CTask.EStatus.Активная:
                            g.DrawImage(Properties.Resources.active, x, y, width, height);
                            break;
                        case CTask.EStatus.Завершена:
                            g.DrawImage(Properties.Resources.done, x, y, width, height);
                            break;
                        case CTask.EStatus.Приостановлена:
                            g.DrawImage(Properties.Resources.paused, x, y, width, height);
                            break;
                        case CTask.EStatus.Просрочена:
                            g.DrawImage(Properties.Resources.outstanding, x, y, width, height);
                            break;
                    }

                    //если у задачи есть файлы, то рисуем иконку файлов
                    if (t.Files.Count > 0)
                        g.DrawImage(Properties.Resources.folder1, x - 16 - 6, y, width, height);

                    rect.Offset(0, 16);

                    g.DrawString(Program.controller.GetCustomerName(t.Customer), Font, b, rect, sf);

                    rect.Width = maxWidth;
                }
            }

            rect.Offset(0, 16);

            //пишем дату создания и срок исполнения
            using (SolidBrush b = new SolidBrush(outstanding ? foreColor : Color.FromKnownColor(KnownColor.GrayText)))
            {
                using (StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.LineLimit) { Alignment = StringAlignment.Near })
                    g.DrawString(t.CreationDate.GetDate(), Font, b, rect, sf);

                using (StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.LineLimit) { Alignment = StringAlignment.Far })
                    g.DrawString(t.ExecutionDate.GetDate(), Font, b, rect, sf);
            }

            rect.Offset(0, 16);
            rect.Height -= 3;

            //рисуем фон шкалы
            Color backColor = Color.FromArgb(64, 64, 64, 64);
            using (SolidBrush b = new SolidBrush(backColor))
                g.FillRectangle(b, rect);

            //рисуем оценочную длительность
            if (t.EstimatedDuration > 0)
            {
                float wid = (float)(rect.Width * t.EstimatedDuration / total);
                wid = (wid > rect.Width) ? rect.Width : wid;
                using (HatchBrush b = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Red, Color.FromArgb(64, 64, 64, 64)))
                    g.FillRectangle(b, new RectangleF(rect.Width - wid + 3, rect.Y, wid, rect.Height));
            }

            //рисуем шкалу дедлайна
            switch(t.Status)
            {
                case CTask.EStatus.Активная:
                case CTask.EStatus.Приостановлена:
                    {
                        using (LinearGradientBrush b = new LinearGradientBrush(rect, Color.Lime, Color.Red, LinearGradientMode.Horizontal))
                        {
                            rect.Width *= percent;
                            rect.Width = (rect.Width > maxWidth) ? maxWidth : rect.Width;

                            g.FillRectangle(b, rect);
                        }

                        break;
                    }
                case CTask.EStatus.Просрочена:
                    {
                        rect.Width *= percent;
                        rect.Width = (rect.Width > maxWidth) ? maxWidth : rect.Width;

                        g.FillRectangle(Brushes.Red, rect);

                        break;
                    }
            }

            //рисуем границу шкалы
            using (Pen p = new Pen(Color.Black))
                g.DrawRectangle(p, rect.X, rect.Y, maxWidth, rect.Height);

            e.DrawFocusRectangle();

            base.OnDrawItem(e);

            //Color CountColor(float val)
            //{
            //    int r = (int)(255 * val);
            //    r = r > 255 ? 255 : r;

            //    return Color.FromArgb(r, (byte)(255 - r), 0);
            //}
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
                e.Graphics.DrawRectangle(Pens.Red, e.ClipRectangle);

            base.OnPaint(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            CTask t = SelectedItem as CTask;
            if (t == null || t.Files.Count == 0)
                return;

            Rectangle rect = this.GetItemRectangle(Items.IndexOf(SelectedItem));
            rect = new Rectangle(rect.Width - 16 - 3 - 16 - 6 - 16 - 6, rect.Y + 3, 16, 16);

            if (rect.Contains(e.Location))
                onMouseClick?.Invoke(t.ID);

            base.OnMouseClick(e);
        }
    }
}
