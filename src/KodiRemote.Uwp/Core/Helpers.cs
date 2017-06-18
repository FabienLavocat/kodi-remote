using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Devices.Notification;
using Windows.System.Profile;
using Windows.UI.Xaml.Media;

namespace KodiRemote.Uwp.Core
{
    internal static class Helpers
    {
        //public static async Task<ImageBrush> LoadBackground(string url)
        //{
        //    if (string.IsNullOrWhiteSpace(url)) return null;

        //    var imageUrl = await LoadImageUrl(url);

        //    return new ImageBrush
        //    {
        //        ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(imageUrl),
        //        Opacity = .7
        //    };
        //}

        public static async Task<string> LoadImageUrl(string image)
        {
            if (App.Context.Connection.Kodi.IsMocked)
                return image;

            try
            {
                var download = await App.Context.Connection.Kodi.Files.PrepareDownloadAsync(image);
                return App.Context.Connection.Kodi.GetFileUrl(download.Details.Path);
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

            if ("Windows.Mobile".Equals(AnalyticsInfo.VersionInfo.DeviceFamily, StringComparison.OrdinalIgnoreCase))
            {
                VibrationDevice v = VibrationDevice.GetDefault();
                v.Vibrate(TimeSpan.FromMilliseconds(50));
            }
        }
    }
}
