using System.Drawing.Drawing2D;

namespace MacroBot.Forms
{
    /// <summary>
    /// Draws the Play, Record and Stop button images so that they stay sharp at any DPI.
    /// </summary>
    internal static class ButtonIcons
    {
        public static Bitmap Play(int size)
            => Draw(size, (g, r) =>
            {
                using var brush = new SolidBrush(Color.FromArgb(0x2E, 0x7D, 0x32));
                //Nudge right so the triangle looks optically centered.
                var dx = r.Width * 0.06f;
                g.FillPolygon(brush, new[]
                {
                    new PointF(r.Left + dx, r.Top),
                    new PointF(r.Right + dx, r.Top + r.Height / 2),
                    new PointF(r.Left + dx, r.Bottom)
                });
            });

        public static Bitmap Record(int size)
            => Draw(size, (g, r) =>
            {
                using var brush = new SolidBrush(Color.FromArgb(0xC6, 0x28, 0x28));
                g.FillEllipse(brush, r);
            });

        public static Bitmap Stop(int size)
            => Draw(size, (g, r) =>
            {
                using var brush = new SolidBrush(Color.FromArgb(0x21, 0x21, 0x21));
                g.FillRectangle(brush, r);
            });

        private static Bitmap Draw(int size, Action<Graphics, RectangleF> paint)
        {
            var bitmap = new Bitmap(size, size);
            using var g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            var inset = size * 0.14f;
            paint(g, new RectangleF(inset, inset, size - inset * 2, size - inset * 2));
            return bitmap;
        }
    }
}
