using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day08 : AdventOfCode2019SolverBase
    {
        private string _imageDate = "";
        public int? ImageWidth;
        public int? ImageHeight;

        public override string Name => "Day 8: Space Image Format";
        public Day08(IIOProvider provider) : base(provider) { }

        public override string ExecutePart1()
        {
            var width = ImageWidth ?? Int32.Parse(IO.RequestInput("Image width?") ?? "");
            var height = ImageHeight ?? Int32.Parse(IO.RequestInput("Image height?") ?? "");
            var image = SifImage.Parse(_imageDate, width, height);

            var targetlayer = image.Layers.OrderBy(p => p.ImageData.Sum(p => p.Count(q => q == 0))).First();
            return (targetlayer.ImageData.Sum(p => p.Count(q => q == 1))
                * targetlayer.ImageData.Sum(p => p.Count(q => q == 2))).ToString();
        }

        public override string ExecutePart2()
        {
            var width = ImageWidth ?? Int32.Parse(IO.RequestInput("Image width?") ?? "");
            var height = ImageHeight ?? Int32.Parse(IO.RequestInput("Image height?") ?? "");
            var image = SifImage.Parse(_imageDate, width, height);

            int[,] sumImage = new int[width, height];
            foreach (var layer in image.Layers.Reverse())
            {
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        sumImage[x, y] = layer.ImageData[x][y] switch
                        {
                            1 => 1,
                            0 => 0,
                            _ => sumImage[x, y]
                        };
            }

            return String.Join(Environment.NewLine,
                Enumerable.Range(0, height)
                .Select(y =>
                    String.Join("", Enumerable.Range(0, width)
                        .Select(x => sumImage[x, y]))));
        }

        public override void Initialize(string input) => _imageDate = input;

        public class SifImage
        {
            public SifImageLayer[] Layers { get; private set; } = new SifImageLayer[0];
            public static SifImage Parse(string encoding, int width, int height)
            {
                if (encoding.Length % width != 0 || encoding.Length % height != 0)
                    throw new InvalidOperationException("Invalid image dimensions, encoding is not divisible by height or width.");

                var layerSize = width * height;
                return new SifImage
                {
                    Layers = Enumerable.Range(0, encoding.Length / layerSize)
                    .Select(p => SifImageLayer.Parse(encoding.Substring(p * layerSize, layerSize), width))
                    .ToArray()
                };
            }

            public class SifImageLayer
            {
                public int[][] ImageData { get; }
                public SifImageLayer(int[][] imageData)
                    => ImageData = imageData;

                public static SifImageLayer Parse(string encoding, int width)
                {
                    var height = encoding.Length / width;
                    int[][] imageData = new int[width][];
                    for (int y = 0; y < width; y++)
                        imageData[y] = new int[height];

                    for (int x = 0; x < width; x++)

                        for (int y = 0; y < height; y++)
                            imageData[x][y] = encoding[y * width + x] - '0';
                    return new SifImageLayer(imageData);
                }
            }
        }
    }
}
