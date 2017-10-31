using System;
using System.Drawing;

namespace TagsCloudVisualisation
{
    public class CloudGenerator
    {
        public static Cloud GenerateCloud(Point center, int rectanglesCount, Func<Size> getSize)
        {
            var layouter = new CircularCloudLayouter(center);
            for (var i = 0; i < rectanglesCount; i++)
            {
                var size = getSize();
                layouter.PutNextRectangle(size);
            }
            return new Cloud(center, layouter.Rectangles);
        }
    }
}