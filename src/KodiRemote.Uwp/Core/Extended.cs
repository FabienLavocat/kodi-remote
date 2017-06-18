using System;
using KodiRemote.Core.Model;

namespace KodiRemote.Uwp.Core
{
    public class ExtendedVideoDetailsTvShow : Extended<VideoDetailsTvShow>
    {
        public ExtendedVideoDetailsTvShow(VideoDetailsTvShow value)
            : base(value, value.Art.Banner, "ms-appx:///Assets/Default/DefaultTvShow.png")
        {
        }
    }

    public class ExtendedAudioDetailsAlbum : Extended<AudioDetailsAlbum>
    {
        public ExtendedAudioDetailsAlbum(AudioDetailsAlbum value)
            : base(value, value.Thumbnail, "ms-appx:///Assets/Default/DefaultAlbumCover.png")
        {
        }
    }

    public class ExtendedAudioDetailsSong : Extended<AudioDetailsSong>
    {
        public ExtendedAudioDetailsSong(AudioDetailsSong value)
            : base(value, value.Thumbnail, "ms-appx:///Assets/Default/DefaultAlbumCover.png")
        {
        }
    }

    public class ExtendedVideoDetailsMovieSet : Extended<VideoDetailsMovieSet>
    {
        public ExtendedVideoDetailsMovieSet(VideoDetailsMovieSet value)
            : base(value, value.Thumbnail, "ms-appx:///Assets/Default/DefaultMovieSmall.png")
        {
        }
    }

    public class ExtendedAddonDetailsBase : Extended<AddonDetailsBase>
    {
        public ExtendedAddonDetailsBase(AddonDetailsBase value)
            : base(value, value.Thumbnail, "ms-appx:///Assets/MainMenu/appbar.deeplink.png")
        {
        }
    }

    public class ExtendedVideoCast : Extended<VideoCast>
    {
        public ExtendedVideoCast(VideoCast value)
            : base(value, value.Thumbnail, "ms-appx:///Assets/Default/DefaultCast.png")
        {
        }
    }

    public class ExtendedVideoDetailsMovie : Extended
    {
        public VideoDetailsMovie Movie { get; private set; }

        public ExtendedVideoDetailsMovie(VideoDetailsMovie movie, bool loadDetails)
            : base(null, "ms-appx:///Assets/Default/DefaultMovieSmall.png")
        {
            LoadMovie(movie, loadDetails);
        }

        private async void LoadMovie(VideoDetailsMovie movie, bool loadDetails)
        {
            if (loadDetails)
                Movie = await App.Context.Connection.Kodi.VideoLibrary.GetMovieDetailsAsync(movie.MovieId);
            else
                Movie = movie;

            NotifyPropertyHasChanged(nameof(Movie));

            LoadThumbailUrl(Movie.Thumbnail);
        }
    }

    public abstract class Extended : NotifyPropertyChanged
    {
        private string _thumbnail;

        public Uri Thumbnail { get { return _thumbnail != null ? new Uri(_thumbnail, UriKind.RelativeOrAbsolute) : null; } }

        protected Extended(string thumbnail, string defaultThumbnail = null)
        {
            SetThumbnail(defaultThumbnail);

            if (!string.IsNullOrWhiteSpace(thumbnail))
                LoadThumbailUrl(thumbnail);
        }

        private void SetThumbnail(string thumbnail)
        {
            _thumbnail = thumbnail;
            NotifyPropertyHasChanged(nameof(Thumbnail));
        }

        protected async void LoadThumbailUrl(string thumbnail)
        {
            if (string.IsNullOrWhiteSpace(thumbnail)
                || !App.Context.DownloadThumbnails) return;

            var url = await Helpers.LoadImageUrl(thumbnail);
            SetThumbnail(url);
        }
    }

    public abstract class Extended<T> : Extended
    {
        public T Value { get; private set; }

        protected Extended(T value, string thumbnail, string defaultThumbnail = null)
            : base(thumbnail, defaultThumbnail)
        {
            Value = value;
            NotifyPropertyHasChanged(nameof(Value));
        }
    }
}
