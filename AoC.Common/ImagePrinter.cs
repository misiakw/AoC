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
        public void DrawImage(int width, int height, string testName, Action<Image> drawFunc)
        {

            var fileName = Path.Combine(InputDir, "Img", $"{testName}.png");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            if (File.Exists(fileName))
                File.Delete(fileName);

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
