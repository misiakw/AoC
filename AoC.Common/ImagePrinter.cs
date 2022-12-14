using ImageMagick;

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
        public void DrawImage(int width, int height, string Name, Action<MagickImage> drawFunc)
        {

            var fileName = Path.Combine(InputDir, "Img", $"{Name}.png");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            if (File.Exists(Name))
                File.Delete(Name);

            using(MagickImage image = new MagickImage(new MagickColor(0,0,0,0), width, height)){
                drawFunc(image);
                image.Write(fileName, MagickFormat.Png);
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
