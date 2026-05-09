using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gestion_de_biblioteca
{
    public class BarChartControl : Panel
    {
        private Dictionary<string, int> _data = new();
        public string EmptyText { get; set; } = "Sin datos para mostrar";

        public BarChartControl()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
        }

        public void SetData(Dictionary<string, int> data)
        {
            _data = data ?? new Dictionary<string, int>();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (_data.Count == 0 || _data.All(x => x.Value == 0))
            {
                using Font font = new("Segoe UI", 10, FontStyle.Italic);
                using Brush brush = new SolidBrush(Color.DimGray);
                SizeF size = g.MeasureString(EmptyText, font);
                g.DrawString(EmptyText, font, brush, (Width - size.Width) / 2, (Height - size.Height) / 2);
                return;
            }

            int left = 95;
            int right = 20;
            int top = 18;
            int barHeight = 24;
            int gap = 13;
            int max = Math.Max(1, _data.Values.Max());
            int chartWidth = Math.Max(50, Width - left - right);

            using Font labelFont = new("Segoe UI", 8);
            using Font valueFont = new("Segoe UI", 8, FontStyle.Bold);
            using Brush textBrush = new SolidBrush(Color.FromArgb(45, 52, 54));
            using Brush barBrush = new SolidBrush(Color.FromArgb(52, 120, 246));
            using Brush barBackBrush = new SolidBrush(Color.FromArgb(232, 238, 248));

            int y = top;
            foreach (var item in _data)
            {
                string label = item.Key.Length > 16 ? item.Key.Substring(0, 15) + "…" : item.Key;
                g.DrawString(label, labelFont, textBrush, 8, y + 4);

                Rectangle back = new(left, y, chartWidth, barHeight);
                g.FillRectangle(barBackBrush, back);

                int width = (int)(chartWidth * (item.Value / (double)max));
                Rectangle bar = new(left, y, Math.Max(4, width), barHeight);
                g.FillRectangle(barBrush, bar);

                g.DrawString(item.Value.ToString(), valueFont, textBrush, left + Math.Min(width + 5, chartWidth - 20), y + 4);
                y += barHeight + gap;
            }
        }
    }
}
