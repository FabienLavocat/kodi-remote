using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class VideoLibrary : CommandCollection
    {
        private readonly Request _request;

        internal VideoLibrary(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Cleans the video library from non-existent items.</summary>
        public async Task CleanAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("VideoLibrary.Clean");
        }

        /// <summary>Retrieve details about a specific tv show episode.</summary>
        public async Task<VideoDetailsEpisodeResponse> GetEpisodeDetailsAsync(int episodeId, params VideoFieldsEpisode[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(VideoFieldsEpisode));

            var method = new ParameteredMethodMessage<GetEpidodeParameters>
                             {
                                 Method = "VideoLibrary.GetEpisodeDetails",
                                 Parameters = new GetEpidodeParameters
                                                  {
                                                      EpisodeId = episodeId,
                                                      Properties = items
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<VideoDetailsEpisodeResponse>>(method);
            return result.Result;
        }

        /// <summary>Retrieve all tv show episodes.</summary>
        public async Task<EpisodesResponse> GetEpisodesAsync(int tvShowId, int season,
                                                             bool ignorearticle = false,
                                                             SortMethod sortMethod = SortMethod.none,
                                                             SortOrder sortOrder = SortOrder.Ascending,
                                                             int start = 0, int end = int.MaxValue,
                                                             params VideoFieldsEpisode[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(VideoFieldsEpisode));

            var method = new ParameteredMethodMessage<GetEpidodesParameters>
                             {
                                 Method = "VideoLibrary.GetEpisodes",
                                 Parameters = new GetEpidodesParameters
                                                  {
                                                      TvShowId = tvShowId,
                                                      Season = season,
                                                      Properties = properties,
                                                      Limits = new ListLimits { Start = start, End = end },
                                                      Sort = new ListSort
                                                                 {
                                                                     IgnoreArticle = ignorearticle,
                                                                     Order = sortOrder.ToString().ToLowerInvariant(),
                                                                     Method = sortMethod.ToString().ToLowerInvariant()
                                                                 }
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<EpisodesResponse>>(method);
            return result.Result;
        }

        /// <summary>Retrieve all genres.</summary>
        public async Task<GenresResponse> GetGenresAsync(VideoTypes videoType,
                                                         bool ignorearticle = false,
                                                         SortMethod sortMethod = SortMethod.none,
                                                         SortOrder sortOrder = SortOrder.Ascending,
                                                         int start = 0, int end = int.MaxValue,
                                                         params LibraryFieldsGenre[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(LibraryFieldsGenre));

            var method = new ParameteredMethodMessage<VideoGenreParameters>
                             {
                                 Method = "VideoLibrary.GetGenres",
                                 Parameters = new VideoGenreParameters
                                                  {
                                                      Type = videoType.ToString().ToLowerInvariant(),
                                                      Properties = properties,
                                                      Limits = new ListLimits { Start = start, End = end },
                                                      Sort = new ListSort
                                                                 {
                                                                     IgnoreArticle = ignorearticle,
                                                                     Order = sortOrder.ToString().ToLowerInvariant(),
                                                                     Method = sortMethod.ToString().ToLowerInvariant()
                                                                 }
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<GenresResponse>>(method);
            return result.Result;
        }

        /// <summary>Retrieve details about a specific movie.</summary>
        public async Task<VideoDetailsMovie> GetMovieDetailsAsync(int movieId, params VideoFieldsMovie[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(VideoFieldsMovie));

            var method = new ParameteredMethodMessage<GetMovieParameters>
                             {
                                 Method = "VideoLibrary.GetMovieDetails",
                                 Parameters = new GetMovieParameters
                                                  {
                                                      MovieId = movieId,
                                                      Properties = items
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<VideoDetailsMovieDetails>>(method);
            return result.Result.DetailsMovie;
        }

        /// <summary>Retrieve details about a specific movie set.</summary>
        public async Task<VideoDetailsMovieSetExtended> GetMovieSetDetailsAsync(int setId, params VideoFieldsMovieSet[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(VideoFieldsMovieSet));

            var method = new ParameteredMethodMessage<GetMovieSetParameters>
                             {
                                 Method = "VideoLibrary.GetMovieSetDetails",
                                 Parameters = new GetMovieSetParameters
                                                  {
                                                      SetId = setId,
                                                      Properties = items
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<VideoDetailsMovieSetExtended>>(method);
            return result.Result;
        }

        /// <summary>Retrieve all movies.</summary>
        public async Task<MovieSetsResponse> GetMovieSetsAsync(bool ignorearticle = false,
                                                               SortMethod sortMethod = SortMethod.none,
                                                               SortOrder sortOrder = SortOrder.Ascending,
                                                               int start = 0, int end = int.MaxValue,
                                                               params VideoFieldsMovieSet[] fields)
        {
            return await GetAsync<MovieSetsResponse, VideoFieldsMovieSet>("VideoLibrary.GetMovieSets",
                                                                          ignorearticle, null, sortMethod,
                                                                          sortOrder, start, end, fields);
        }

        /// <summary>Retrieve all movies.</summary>
        public async Task<MoviesResponse> GetMoviesAsync(int? genreId = null,
                                                         bool ignorearticle = false,
                                                         SortMethod sortMethod = SortMethod.none,
                                                         SortOrder sortOrder = SortOrder.Ascending,
                                                         int start = 0, int end = int.MaxValue,
                                                         params VideoFieldsMovie[] fields)
        {
            Filter filter = null;

            if (genreId != null)
                filter = new Filter { GenreId = genreId };

            return await GetAsync<MoviesResponse, VideoFieldsMovie>("VideoLibrary.GetMovies",
                                                                    ignorearticle, filter, sortMethod,
                                                                    sortOrder, start, end, fields);
        }

        /// <summary>Retrieve details about a specific music video.</summary>
        public async Task<VideoDetailsMusicVideo> GetMusicVideoDetailsAsync(int musicVideoId, params VideoFieldsMusicVideo[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(VideoFieldsMusicVideo));

            var method = new ParameteredMethodMessage<GetMusicVideoParameters>
                             {
                                 Method = "VideoLibrary.GetMusicVideoDetails",
                                 Parameters = new GetMusicVideoParameters
                                                  {
                                                      MusicVideoId = musicVideoId,
                                                      Properties = items
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<VideoDetailsMusicVideo>>(method);
            return result.Result;
        }

        /// <summary>Retrieve all music videos.</summary>
        public async Task<MusicVideoResponse> GetMusicVideosAsync(bool ignorearticle = false,
                                                                  SortMethod sortMethod = SortMethod.none,
                                                                  SortOrder sortOrder = SortOrder.Ascending,
                                                                  int start = 0, int end = int.MaxValue,
                                                                  params VideoFieldsMusicVideo[] fields)
        {
            return await GetAsync<MusicVideoResponse, VideoFieldsMusicVideo>("VideoLibrary.GetMusicVideos",
                                                                             ignorearticle, null, sortMethod,
                                                                             sortOrder, start, end, fields);
        }

        /// <summary>Retrieve recently added tv episodes.</summary>
        public async Task<EpisodesResponse> GetRecentlyAddedEpisodesAsync(bool ignorearticle = false,
                                                                      SortMethod sortMethod = SortMethod.none,
                                                                      SortOrder sortOrder = SortOrder.Ascending,
                                                                      int start = 0, int end = int.MaxValue,
                                                                      params VideoFieldsEpisode[] fields)
        {
            return await GetRecentlyAddedAsync<EpisodesResponse, VideoFieldsEpisode>(
                "VideoLibrary.GetRecentlyAddedEpisodes",
                ignorearticle, sortMethod, sortOrder,
                start, end, fields);
        }

        /// <summary>Retrieve recently added movies.</summary>
        public async Task<MoviesResponse> GetRecentlyAddedMoviesAsync(bool ignorearticle = false,
                                                                      SortMethod sortMethod = SortMethod.none,
                                                                      SortOrder sortOrder = SortOrder.Ascending,
                                                                      int start = 0, int end = int.MaxValue,
                                                                      params VideoFieldsMovie[] fields)
        {
            return await GetRecentlyAddedAsync<MoviesResponse, VideoFieldsMovie>(
                "VideoLibrary.GetRecentlyAddedMovies",
                ignorearticle, sortMethod, sortOrder,
                start, end, fields);
        }

        /// <summary>Retrieve recently added music videos.</summary>
        public async Task<MusicVideoResponse> GetRecentlyAddedMusicVideosAsync(bool ignorearticle = false,
                                                                               SortMethod sortMethod = SortMethod.none,
                                                                               SortOrder sortOrder = SortOrder.Ascending,
                                                                               int start = 0, int end = int.MaxValue,
                                                                               params VideoFieldsMusicVideo[] fields)
        {
            return await GetRecentlyAddedAsync<MusicVideoResponse, VideoFieldsMusicVideo>(
                "VideoLibrary.GetRecentlyAddedMusicVideos",
                ignorearticle, sortMethod, sortOrder,
                start, end, fields);
        }

        /// <summary>Retrieve all tv seasons.</summary>
        public async Task<SeasonsResponse> GetSeasonsAsync(int tvShowId,
                                                           bool ignorearticle = false,
                                                           SortMethod sortMethod = SortMethod.none,
                                                           SortOrder sortOrder = SortOrder.Ascending,
                                                           int start = 0, int end = int.MaxValue,
                                                           params VideoFieldsSeason[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(VideoFieldsSeason));

            var method = new ParameteredMethodMessage<GetSeasonsParameters>
                             {
                                 Method = "VideoLibrary.GetSeasons",
                                 Parameters = new GetSeasonsParameters
                                                  {
                                                      TvShowId = tvShowId,
                                                      Properties = properties,
                                                      Limits = new ListLimits { Start = start, End = end },
                                                      Sort = new ListSort
                                                                 {
                                                                     IgnoreArticle = ignorearticle,
                                                                     Order = sortOrder.ToString().ToLowerInvariant(),
                                                                     Method = sortMethod.ToString().ToLowerInvariant()
                                                                 }
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<SeasonsResponse>>(method);
            return result.Result;
        }

        /// <summary>Retrieve details about a specific tv show.</summary>
        public async Task<VideoDetailsTvShowResponse> GetTvShowDetailsAsync(int tvShowId, params VideoFieldsTVShow[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(VideoFieldsTVShow));

            var method = new ParameteredMethodMessage<GetTVShowParameters>
                             {
                                 Method = "VideoLibrary.GetTVShowDetails",
                                 Parameters = new GetTVShowParameters
                                                  {
                                                      TvShowId = tvShowId,
                                                      Properties = items
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<VideoDetailsTvShowResponse>>(method);
            return result.Result;
        }

        /// <summary>Retrieve all tv shows.</summary>
        public async Task<TvShowsResponse> GetTvShowsAsync(bool ignorearticle = false,
                                                           SortMethod sortMethod = SortMethod.none,
                                                           SortOrder sortOrder = SortOrder.Ascending,
                                                           int start = 0, int end = int.MaxValue,
                                                           params VideoFieldsTVShow[] fields)
        {
            return await GetAsync<TvShowsResponse, VideoFieldsTVShow>("VideoLibrary.GetTVShows",
                                                                      ignorearticle, null, sortMethod,
                                                                      sortOrder, start, end, fields);
        }

        /// <summary>Scans the video sources for new library items.</summary>
        public async Task<string> ScanAsync(string directory = "")
        {
            var method = new ParameteredMethodMessage<ScanParameters>
                             {
                                 Method = "VideoLibrary.Scan",
                                 Parameters = new ScanParameters { Directory = directory }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return result.Result;
        }

        private async Task<T> GetAsync<T, TEnum>(string methodName, bool ignorearticle,
                                                 Filter filter,
                                                 SortMethod sortMethod,
                                                 SortOrder sortOrder,
                                                 int start, int end,
                                                 IEnumerable<TEnum> fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(TEnum));

            var method = new ParameteredMethodMessage<FilteredPropertiesParameters>
                             {
                                 Method = methodName,
                                 Parameters = new FilteredPropertiesParameters
                                                  {
                                                      Filter = filter,
                                                      Properties = properties,
                                                      Limits = new ListLimits { Start = start, End = end },
                                                      Sort = new ListSort
                                                                 {
                                                                     IgnoreArticle = ignorearticle,
                                                                     Order = sortOrder.ToString().ToLowerInvariant(),
                                                                     Method = sortMethod.ToString().ToLowerInvariant()
                                                                 }
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<T>>(method);
            return result.Result;
        }

        private async Task<T> GetRecentlyAddedAsync<T, TEnum>(string methodName,
                                                              bool ignorearticle,
                                                              SortMethod sortMethod,
                                                              SortOrder sortOrder,
                                                              int start, int end,
                                                              IEnumerable<TEnum> fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(TEnum));

            var method = new ParameteredMethodMessage<LimitsSortPropertiesParameters>
                             {
                                 Method = methodName,
                                 Parameters = new LimitsSortPropertiesParameters
                                                  {
                                                      Properties = properties,
                                                      Limits = new ListLimits { Start = start, End = end },
                                                      Sort = new ListSort
                                                                 {
                                                                     IgnoreArticle = ignorearticle,
                                                                     Order = sortOrder.ToString().ToLowerInvariant(),
                                                                     Method = sortMethod.ToString().ToLowerInvariant()
                                                                 }
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<T>>(method);
            return result.Result;
        }
    }
}