using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AoC.Common
{
    public class ImagePrinter
    {
        private string InputDir;
        public ImagePrinter(string inputDir)
        {
            InputDir = inputDir;
        }
        private static bool isOpened = false;
        public void DrawImage(int width, int height, string Name, Action<Image> drawFunc)
        {

            var fileName = Path.Combine(InputDir, "Img", $"{Name}.png");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            if (File.Exists(Name))
                File.Delete(Name);

            using (Image<Rgba32> image = new(width, height)){
                drawFunc.Invoke(image);
                image.SaveAsPng(fileName);
            }
            /*
            if (!isOpened)
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(fileName));
                isOpened = true;
            }*/
        }
    }
}
