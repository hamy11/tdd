using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualisation
{
    public class Cloud
    {
        public Point Center;
        public IEnumerable<Rectangle> Rectangles;

        public Cloud(Point center, IEnumerable<Rectangle> rectangles)
        {
            Center = center;
            Rectangles = rectangles;
        }
    }
}