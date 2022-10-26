using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Resources;
using System.Windows;

namespace CharaTools
{
    public class ImageReplacer
    {
        #region Variables
        private int origWidth = 0;
        private int origHeight = 0;

        // private const string card_default = "card_default.png";
        private const string card_bkg_frame = "card_bkg_frame.png";
        private const string card_frame = "card_frame.png";
        private const string card_frame_simple = "card_frame_simple.png";
        #endregion

        #region Constructor
        public ImageReplacer(int origWidth, int origHeight)
        {
            this.origWidth = origWidth;
            this.origHeight = origHeight;
        }
        #endregion

        #region Properties
        public Constants.CardResolutions Resolution { get; set; }

        public bool DrawBkgImage { get; set; }

        public bool DrawFrame { get; set; }

        public Color BkgColor { get; set; }

        public byte[] PngData { get; private set; } = null;
        #endregion

        #region Methods
        public void SetBackgroundColor(int a, int r, int g, int b)
        {
            BkgColor = Color.FromArgb(a, r, g, b);
        }

        public bool Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return false;
            
            try
            {
                var bmp = Bitmap.FromFile(filePath);
                if (bmp != null)
                {
                    Bitmap bkgImage = null;
                    Bitmap frameImage = null;

                    if (DrawBkgImage)
                    {
                        bkgImage = LoadFromResource(card_bkg_frame);
                        if (DrawFrame)
                            frameImage = LoadFromResource(card_frame);
                    }
                    else if (DrawFrame)
                        frameImage = LoadFromResource(card_frame_simple);

                    System.Drawing.Size size = new System.Drawing.Size(origWidth, origHeight);
                    switch (Resolution)
                    {
                        case Constants.CardResolutions.Original:
                            size = new System.Drawing.Size(252, 352);
                            break;
                        case Constants.CardResolutions.Normal:
                            size = new System.Drawing.Size(504, 704);
                            break;
                        case Constants.CardResolutions.Large:
                            size = new System.Drawing.Size(756, 1056);
                            break;
                        case Constants.CardResolutions.ExtraLarge:
                            size = new System.Drawing.Size(1134, 1584);
                            break;
                        default:
                            break;
                    }
                    CrateImage(bmp, bkgImage, frameImage, BkgColor, size);

                    return true;
                }
            }
            catch (Exception)
            { }

            return false;
        }

        private Bitmap LoadFromResource(string resName)
        {
            if (string.IsNullOrEmpty(resName)) return null;

            try
            {
#if NET
                var resUri = new Uri($"/Resources/{resName}", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(resUri);
                if (info != null && info.Stream != null)
                    return (Bitmap)Bitmap.FromStream(info.Stream);
#else
                var assm = Assembly.GetExecutingAssembly();
                string[] resNames = assm.GetManifestResourceNames();
                string resourceName = resNames.Single(str => str.EndsWith(resName));

                if (!string.IsNullOrEmpty(resourceName))
                {
                    using (Stream stream = assm.GetManifestResourceStream(resourceName))
                        return (Bitmap)Bitmap.FromStream(stream);
                }
#endif
            }
            catch (Exception)
            { }

            return null;
        }

        private void CrateImage(Image bmp, Image bkgImage, Image frameImage, Color bkgColor, System.Drawing.Size size)
        {
            Bitmap bmPhoto = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            using (Graphics graphic = Graphics.FromImage(bmPhoto))
            {
                graphic.Clear(bkgColor);

                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;

                if (bkgImage != null)
                {
                    graphic.DrawImage(bkgImage,
                        new Rectangle(0, 0, size.Width, size.Height),
                        new Rectangle(0, 0, bkgImage.Width, bkgImage.Height),
                        GraphicsUnit.Pixel);
                }

                Bitmap image = ResizeBitmap(bmp, size.Width, size.Height);
                graphic.DrawImage(image,
                        new Rectangle(0, 0, size.Width, size.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);

                if (frameImage != null)
                {
                    graphic.DrawImage(frameImage,
                        new Rectangle(0, 0, size.Width, size.Height),
                        new Rectangle(0, 0, frameImage.Width, frameImage.Height),
                        GraphicsUnit.Pixel);
                }
            }

            using (var stream = new MemoryStream())
            {
                bmPhoto.Save(stream, ImageFormat.Png);
                PngData = stream.ToArray();
            }
        }

        private Bitmap ResizeBitmap(Image image, int width, int height)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int destX = 0;
            int destY = 0;

            float nPercent;

            float nPercentW = ((float)width / (float)sourceWidth);
            float nPercentH = ((float)height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((width -
                            (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((height -
                            (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (Graphics graphic = Graphics.FromImage(bmPhoto))
            {
                graphic.Clear(Color.Transparent);

                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;

                graphic.DrawImage(image,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(0, 0, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
            }

            return bmPhoto;
        }
        #endregion
    }
}
