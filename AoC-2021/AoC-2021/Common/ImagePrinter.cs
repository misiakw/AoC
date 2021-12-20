using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace AoC_2021.Common
{
    public class ImagePrinter
    {
        private string InputDir;
        public ImagePrinter(string inputDir)
        {
            InputDir = inputDir;
        }
        private static bool isOpened = false;
        public void DrawImage(int width, int height, string testName, Action<Graphics, Image> drawFunc)
        {

            var fileName = Path.Combine(InputDir, "Img", $"{testName}.png");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            if (File.Exists(fileName))
                File.Delete(fileName);

            using (var image = new Bitmap(width, height))
            {
                using (var canvas = Graphics.FromImage(image))
                {
                    drawFunc(canvas, image);
                }
                image.Save(fileName);
            }

            if (!isOpened)
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(fileName));
                isOpened = true;
            }
        }
    }
}
