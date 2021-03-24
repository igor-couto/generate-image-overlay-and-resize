using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Image = System.Drawing.Image;

namespace generate_image_overlay
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory();

            MergeImages("clothes.png", "skin.png", "generated.png", 400, 400);
        }

        static void MergeImages(
            string overlayImageName,
            string bottomImageName,
            string newFileName,
            int finalWidth,
            int finalHeight)
        {
            var currentPath = Directory.GetCurrentDirectory();

            var bottomImage = Image.FromFile(Path.GetFullPath(currentPath + "\\Images\\Source\\" + bottomImageName));
            var overlayImage = Image.FromFile(Path.GetFullPath(currentPath + "\\Images\\Overlay\\" + overlayImageName));

            var generatedImage = bottomImage;
            var graphics = Graphics.FromImage(generatedImage);

            graphics.DrawImage(overlayImage, 0, 0, overlayImage.Width, overlayImage.Height);

            generatedImage = ResizeImage(generatedImage, finalWidth, finalHeight);

            Directory.CreateDirectory(currentPath + "\\Images\\Result\\");

            generatedImage.Save(currentPath + "\\Images\\Result\\" + newFileName, ImageFormat.Png);
        }

        static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.Default;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.None;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }
    }
}
