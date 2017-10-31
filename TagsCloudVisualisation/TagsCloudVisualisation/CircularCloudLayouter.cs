using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualisation
{
    public class CircularCloudLayouter
    {
        private readonly ArchimedeanSpiral archimedeanSpiral;
        public readonly List<Rectangle> Rectangles;
        public Point Center;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            archimedeanSpiral = new ArchimedeanSpiral(center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height*rectangleSize.Width == 0)
                throw new ArgumentException("Сторона прямоугольника для текста не может быть равна 0");

            var resultRectangle = GetNextRectangle(rectangleSize);
            Rectangles.Add(resultRectangle);
            return resultRectangle;
        }

        private Rectangle GetNextRectangle(Size size)
        {
            var rectangle = Rectangle.Empty;
            foreach (var point in archimedeanSpiral.GetNextPoint())
            {
                var position = new Point(point.X - size.Width/2, point.Y - size.Height/2);
                rectangle = new Rectangle(position, size);
                if (!Rectangles.Any(x => x.IntersectsWith(rectangle)))
                    break;
            }

            return rectangle;
        }

        private class ArchimedeanSpiral
        {
            private Point center;

            public ArchimedeanSpiral(Point center)
            {
                this.center = center;
            }

            public IEnumerable<Point> GetNextPoint()
            {
                return Enumerable.Range(0, int.MaxValue).Select(ArchimedeanPoint);
            }

            private Point ArchimedeanPoint(int degrees)
            {
                const double turningDistance = 2;
                var theta = degrees*Math.PI/180;
                var radius = turningDistance*theta;
                return new Point
                {
                    X = (int) (center.X + radius*Math.Cos(theta)),
                    Y = (int) (center.Y + radius*Math.Sin(theta))
                };
            }
        }
    }
}