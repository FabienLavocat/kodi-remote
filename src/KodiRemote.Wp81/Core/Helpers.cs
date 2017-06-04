using Microsoft.Devices;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KodiRemote.Wp81.Core
{
    internal static class Helpers
    {
        public static async Task<ImageBrush> LoadBackground(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;

            var imageUrl = await LoadImageUrl(url);

            return new ImageBrush
                {
                    ImageSource = (ImageSource) new ImageSourceConverter().ConvertFromString(imageUrl),
                    Opacity = .7
                };
        }

        public static async Task<string> LoadImageUrl(string image)
        {
            if (App.Context.Connection.Xbmc.IsMocked)
                return image;

            try
            {
                var download = await App.Context.Connection.Xbmc.Files.PrepareDownloadAsync(image);
                return App.Context.Connection.Xbmc.GetFileUrl(download.Details.Path);
            }
            catch
            {
                return "";
            }
        }

        public static string Combine(string[] values)
        {
            if (values == null || !values.Any()) return string.Empty;

            var result = values.Aggregate("", (current, v) => string.Concat(current, v, " / "));

            return result.Substring(0, result.Length - 3);
        }

        public static void Vibrate()
        {
            if (!App.Context.AllowVibrate) return;

            VibrateController.Default.Start(TimeSpan.FromMilliseconds(50));
        }
    }
}