using Svg;
using System.Drawing.Imaging;
using System.IO;

namespace SVGToPNG
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"C:\Users\Sloan\source\repos\SVGToPNG\SVGToPNG\svg", "*.svg");

            foreach (var file in files)
            {
                var doc = SvgDocument.Open(file);
                using (var img = doc.Draw(300, 300))
                {
                    var filename = Path.GetFileNameWithoutExtension(file) + ".png";
                    img.Save(filename, ImageFormat.Png);
                }
            }
        }
    }
}
