using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualisation
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private Point center;
        private Random rnd;
        private Cloud cloud;

        [SetUp]
        public void SetUp()
        {
            center = new Point(500, 500);
            cloud = new Cloud(new Point(), new List<Rectangle>());
            rnd = new Random();
        }

        [TearDown]
        public void TestTearDown()
        {
            if (!TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed)) return;
            var path = $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.Name}";
            CloudVisualizer.Visualize(cloud, path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [Test]
        public void CircularCloudLayouter_AfterCreation_HasCenter()
        {
            var circularCloudLayouter = new CircularCloudLayouter(center);
            cloud = new Cloud(circularCloudLayouter.Center, circularCloudLayouter.Rectangles);
            circularCloudLayouter.Center.X.Should().Be(center.X);
            circularCloudLayouter.Center.Y.Should().Be(center.Y);
        }

        [TestCase(0, 0, TestName = "Когда размер (0,0)")]
        [TestCase(0, 100, TestName = "Когда размер (0,100)")]
        [TestCase(100, 0, TestName = "Когда размер (100,0)")]
        public void PutNextRectangle_ThrowArgumentException(int x, int y)
        {
            var layouter = new CircularCloudLayouter(center);
            cloud = new Cloud(layouter.Center, layouter.Rectangles);
            new Action(() => layouter.PutNextRectangle(new Size(x, y)))
                .ShouldThrow<ArgumentException>()
                .WithMessage("Сторона прямоугольника для текста не может быть равна 0");
        }

        [Test]
        public void PutNextRectangle_AfterPuttingTwoRectangles_RectanglesDoesNotIntersect()
        {
            var layouter = new CircularCloudLayouter(center);
            var firstRectangle = layouter.PutNextRectangle(new Size(10, 10));
            var secondRectangle = layouter.PutNextRectangle(new Size(10, 10));
            cloud = new Cloud(layouter.Center, layouter.Rectangles);
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [TestCase(100, 5, 30, 5, 10, TestName = "100 прямоугольников с размерами 5<x<30 5<y<10")]
        [TestCase(10, 5, 30, 5, 10, TestName = "10 прямоугольников с размерами 5<x<30 5<y<10")]
        [TestCase(1, 500, 550, 500, 550, TestName = "1 прямоугольник с размерами 500<x<550 500<y<550")]
        public void PutNextRectangle_AfterPuttingSeveralRectanglesWithRandomSize_RectanglesDoesNotIntersect(
            int count, int minSizeX, int maxSizeX, int minSizeY, int maxSizeY)
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next(minSizeX, maxSizeX), rnd.Next(minSizeY, maxSizeY));
                var rectangle = layouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }
            cloud = new Cloud(layouter.Center, layouter.Rectangles);
            for (var i = 0; i < count; i++)
                for (var j = i + 1; j < count; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
        }
    }
}