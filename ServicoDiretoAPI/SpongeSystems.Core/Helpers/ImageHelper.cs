using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace SpongeSolutions.Core.Helpers
{
    public class ImageHelper
    {
        public static void ResizeImage(string originalFile, string newFile, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            try
            {
                System.Drawing.Image fullsizeImage = System.Drawing.Image.FromFile(originalFile);
                string thumPath = Path.Combine(Path.GetDirectoryName(originalFile), "thumb");
                if (!Directory.Exists(thumPath))
                    Directory.CreateDirectory(thumPath);
                
                // Prevent using images internal thumbnail
                fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                if (onlyResizeIfWider)
                {
                    if (fullsizeImage.Width <= newWidth)
                    {
                        newWidth = fullsizeImage.Width;
                    }
                }

                int NewHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                if (NewHeight > maxHeight)
                {
                    // Resize with height instead
                    newWidth = fullsizeImage.Width * maxHeight / fullsizeImage.Height;
                    NewHeight = maxHeight;
                }

                System.Drawing.Image NewImage = fullsizeImage.GetThumbnailImage(newWidth, NewHeight, null, IntPtr.Zero);
                // Clear handle to original file so that we can overwrite it if necessary
                fullsizeImage.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                // Save resized picture
                NewImage.Save(newFile);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string GetThumbURL(string url)
        {
            if (url != null)
                return url.Split('|')[1];
            else
                return "/_images/clipping_picture.png";
        }

        public static string GetFullImageURL(string url)
        {
            if (url != null)
                return url.Split('|')[0];
            else
                return "/_images/clipping_picture.png";
        }

    }
}
