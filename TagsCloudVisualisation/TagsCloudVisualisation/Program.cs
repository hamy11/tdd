using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Mime;

namespace TagsCloudVisualisation
{
    class Program
    {
        static void Main(string[] args)
        {
            var center = new Point(500, 500);
            var rnd = new Random();
            var massiveCloud = CloudGenerator.GenerateCloud(center, 500, () => new Size(rnd.Next(10, 40), rnd.Next(5, 30)));
            CloudVisualizer.Visualize(massiveCloud, "massiveCloud");
            var smallCloud = CloudGenerator.GenerateCloud(center, 50, () => new Size(rnd.Next(5, 30), rnd.Next(5, 20)));
            CloudVisualizer.Visualize(smallCloud, "smallCloud");
            var bigDispersionCloud = CloudGenerator.GenerateCloud(center, 20, () => new Size(rnd.Next(5, 300), rnd.Next(5, 200)));
            CloudVisualizer.Visualize(bigDispersionCloud, "bigDispersionCloud");
        }
    }
}