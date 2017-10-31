using System.Drawing;
using System.Drawing.Drawing2D;

namespace TagsCloudVisualisation
{
    public class CloudVisualizer
    {
        public static void Visualize(Cloud cloud, string visualisationName)
        {
            var pen = new Pen(Color.LightSeaGreen, 1.5f);
            var bm = new Bitmap(cloud.Center.X*2, cloud.Center.Y*2);
            var graphics = Graphics.FromImage(bm);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle (new Point(0,0), new Size(bm.Width, bm.Height)));
            foreach (var rectangle in cloud.Rectangles)
            {
                graphics.DrawRectangle(pen, rectangle);
            }
            pen.Color = Color.Brown;
            graphics.DrawEllipse(pen, new Rectangle(cloud.Center, new Size(2,2)));
            graphics.Save();
            bm.Save($"{visualisationName}.png");
        }
    }
}