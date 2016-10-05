using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Tfs2015SetPicture
{
    /// <summary>
    ///     Class for image loading.
    /// </summary>
    internal static class ImageLoader
    {
        private const int TargetWidth = 144;
        private const int TargetHeight = 144;

        /// <summary>
        ///     Loads an image from path into a byte array.
        /// </summary>
        public static byte[] LoadImage(string path)
        {
            var image = Image.FromFile(path);

            var destinationRectangle = new Rectangle(0, 0, TargetWidth, TargetHeight);
            var destinationImage = new Bitmap(TargetWidth, TargetHeight);

            destinationImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(image))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destinationRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            byte[] buffer;
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                buffer = stream.ToArray();
            }
            return buffer;
        }
    }
}