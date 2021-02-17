using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SearchIcd10.Utils
{
    /// <summary>
    /// Internal class for handling Window Icons
    /// </summary>
    internal static class Icons
    {
        /// <summary>
        /// Convert a System.Drawing.Icon (from resources for instance) to a ImageSource for use in WPF
        /// </summary>
        /// <param name="icon">Icon to convert</param>
        /// <returns>New ImageSource handle</returns>
        public static ImageSource ToImageSource(this Icon icon)
        {
            using (MemoryStream iconStream = new MemoryStream())
            {
                icon.Save(iconStream);
                iconStream.Seek(0, SeekOrigin.Begin);

                var ret = BitmapFrame.Create(iconStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                return ret;
            }
        }
    }
}
