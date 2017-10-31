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
            return archimedeanSpiral.GetNextPoint()
                .Select(point => BuildRectangle(point, size))
                .First(rectangle => !Rectangles.Any(rectangle.IntersectsWith))
                .ApproachToCenter(this);
        }

        private static Rectangle BuildRectangle(Point point, Size size)
        {
            var centralizedRectangle = new Rectangle(new Point(point.X - size.Width/2, point.Y - size.Height/2), size);
            return centralizedRectangle;
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
                const double turningDistance = 0.5;
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

    public static class RectangleExtension
    {
        public static Rectangle ApproachToCenter(this Rectangle rectangle, CircularCloudLayouter layouter)
        {
            if (layouter.Rectangles.Count == 0)
                return rectangle;

            while (true)
            {
                var rectangleCenter = new Point(rectangle.X + rectangle.Size.Width/2,
                    rectangle.Y + rectangle.Size.Height/2);
                var normalizedVector = GetNormalizedDirectionVector(layouter, rectangleCenter);
                var potentionalRectanglesIntersectStatuses = new Dictionary<Rectangle, bool>
                {
                    {new Rectangle(new Point(rectangle.X + normalizedVector.X, rectangle.Y), rectangle.Size), false},
                    {new Rectangle(new Point(rectangle.X, rectangle.Y + normalizedVector.Y), rectangle.Size), false}
                };

                foreach (var placedRectangle in layouter.Rectangles)
                    foreach (var pair in potentionalRectanglesIntersectStatuses.ToArray())
                    {
                        if (rectangle.Equals(pair.Key) || placedRectangle.IntersectsWith(pair.Key))
                            potentionalRectanglesIntersectStatuses[pair.Key] = true;
                    }

                if (potentionalRectanglesIntersectStatuses.Select(x => x.Value).All(x => x))
                    break;

                rectangle = potentionalRectanglesIntersectStatuses.First(x => !x.Value).Key;
            }
            return rectangle;
        }

        private static Point GetNormalizedDirectionVector(CircularCloudLayouter layouter, Point rectangleCenter)
        {
            var vector = new Point(layouter.Center.X - rectangleCenter.X, layouter.Center.Y - rectangleCenter.Y);
            var vectorLength = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            var normalizedVector = new Point((int) Math.Round(vector.X/vectorLength),
                (int) Math.Round(vector.Y/vectorLength));
            return normalizedVector;
        }
    }
}