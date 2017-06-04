using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class AudioLibrary : CommandCollection
    {
        private readonly Request _request;

        internal AudioLibrary(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Cleans the audio library from non-existent items.</summary>
        public async Task CleanAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("AudioLibrary.Clean");
        }

        /// <summary>Retrieve details about a specific album.</summary>
        public async Task<AudioDetailsAlbum> GetAlbumDetailsAsync(int albumId, params AudioFieldsAlbum[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(AudioFieldsAlbum));

            var method = new ParameteredMethodMessage<AlbumFieldsParameters>
                             {
                                 Method = "AudioLibrary.GetAlbumDetails",
                                 Parameters = new AlbumFieldsParameters
                                                  {
                                                      AlbumId = albumId,
                                                      Properties = items
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<AlbumDetails>>(method);
            return result.Result.Details;
        }

        /// <summary>Retrieve all albums from specified artist or genre.</summary>
        public async Task<AlbumsResponse> GetAlbumsAsync(int? artistId = null,
                                                         bool ignorearticle = false,
                                                         SortMethod sortMethod = SortMethod.none,
                                                         SortOrder sortOrder = SortOrder.Ascending,
                                                         int start = 0, int end = int.MaxValue,
                                                         params AudioFieldsAlbum[] fields)
        {
            return await GetFilteredAlbumsSongsAsync<AlbumsResponse, AudioFieldsAlbum>("AudioLibrary.GetAlbums",
                                                                                       ignorearticle, sortMethod,
                                                                                       sortOrder,
                                                                                       start, end, fields, artistId);
        }

        /// <summary>Retrieve details about a specific artist.</summary>
        public async Task<AudioDetailsArtist> GetArtistDetailsAsync(int artistId, params AudioFieldsArtist[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(AudioFieldsArtist));

            var method = new ParameteredMethodMessage<ArtistFieldsParameters>
                {
                    Method = "AudioLibrary.GetArtistDetails",
                    Parameters = new ArtistFieldsParameters
                        {
                            ArtistId = artistId,
                            Properties = items
                        }
                };

            var result = await _request.SendRequestAsync<BasicResponseMessage<ArtistDetails>>(method);
            return result.Result.Details;
        }

        /// <summary>Retrieve all artists.</summary>
        public async Task<ArtistsResponse> GetArtistsAsync(bool? albumArtistsOnly = null,
                                                           bool ignorearticle = false,
                                                           SortMethod sortMethod = SortMethod.none,
                                                           SortOrder sortOrder = SortOrder.Ascending,
                                                           int start = 0, int end = int.MaxValue,
                                                           params AudioFieldsArtist[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(AudioFieldsArtist));

            var method = new ParameteredMethodMessage<ArtistsParameters>
                {
                    Method = "AudioLibrary.GetArtists",
                    Parameters = new ArtistsParameters
                        {
                            AlbumArtistsOnly = albumArtistsOnly,
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

            var result = await _request.SendRequestAsync<BasicResponseMessage<ArtistsResponse>>(method);
            return result.Result;
        }

        /// <summary>Retrieve all genres.</summary>
        public async Task<GenresResponse> GetGenresAsync(bool ignorearticle = false,
                                                         SortMethod sortMethod = SortMethod.none,
                                                         SortOrder sortOrder = SortOrder.Ascending,
                                                         int start = 0, int end = int.MaxValue,
                                                         params LibraryFieldsGenre[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(LibraryFieldsGenre));

            var method = new ParameteredMethodMessage<LimitsSortPropertiesParameters>
                             {
                                 Method = "AudioLibrary.GetGenres",
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

            var result = await _request.SendRequestAsync<BasicResponseMessage<GenresResponse>>(method);
            return result.Result;
        }

        /// <summary>Retrieve all songs from specified album, artist or genre.</summary>
        public async Task<SongsResponse> GetSongsAsync(int? albumId = null,
                                                       int? artistId = null,
                                                       int? genreId = null,
                                                       bool ignorearticle = false,
                                                       SortMethod sortMethod = SortMethod.none,
                                                       SortOrder sortOrder = SortOrder.Ascending,
                                                       int start = 0, int end = int.MaxValue,
                                                       params AudioFieldsSong[] fields)
        {
            return await GetFilteredAlbumsSongsAsync<SongsResponse, AudioFieldsSong>("AudioLibrary.GetSongs",
                                                                                     ignorearticle, sortMethod,
                                                                                     sortOrder,
                                                                                     start, end, fields,
                                                                                     artistId, albumId, genreId);
        }

        /// <summary>Retrieve recently added albums.</summary>
        public async Task<AlbumsResponse> GetRecentlyAddedAlbumsAsync(bool ignorearticle = false,
                                                                      SortMethod sortMethod = SortMethod.none,
                                                                      SortOrder sortOrder = SortOrder.Ascending,
                                                                      int start = 0, int end = int.MaxValue,
                                                                      params AudioFieldsAlbum[] fields)
        {
            return await GetAlbumsSongsAsync<AlbumsResponse, AudioFieldsAlbum>("AudioLibrary.GetRecentlyAddedAlbums",
                                                                               ignorearticle, sortMethod, sortOrder,
                                                                               start, end, fields);
        }

        /// <summary>Retrieve recently added songs.</summary>
        public async Task<SongsResponse> GetRecentlyAddedSongsAsync(bool ignorearticle = false,
                                                                    SortMethod sortMethod = SortMethod.none,
                                                                    SortOrder sortOrder = SortOrder.Ascending,
                                                                    int start = 0, int end = int.MaxValue,
                                                                    params AudioFieldsSong[] fields)
        {
            return await GetAlbumsSongsAsync<SongsResponse, AudioFieldsSong>("AudioLibrary.GetRecentlyAddedSongs",
                                                                             ignorearticle, sortMethod, sortOrder,
                                                                             start, end, fields);
        }

        /// <summary>Retrieve recently played albums.</summary>
        public async Task<AlbumsResponse> GetRecentlyPlayedAlbumsAsync(bool ignorearticle = false,
                                                                       SortMethod sortMethod = SortMethod.none,
                                                                       SortOrder sortOrder = SortOrder.Ascending,
                                                                       int start = 0, int end = int.MaxValue,
                                                                       params AudioFieldsAlbum[] fields)
        {
            return await GetAlbumsSongsAsync<AlbumsResponse, AudioFieldsAlbum>("AudioLibrary.GetRecentlyPlayedAlbums",
                                                                               ignorearticle, sortMethod, sortOrder,
                                                                               start, end, fields);
        }

        /// <summary>Retrieve recently played songs.</summary>
        public async Task<SongsResponse> GetRecentlyPlayedSongsAsync(bool ignorearticle = false,
                                                                     SortMethod sortMethod = SortMethod.none,
                                                                     SortOrder sortOrder = SortOrder.Ascending,
                                                                     int start = 0, int end = int.MaxValue,
                                                                     params AudioFieldsSong[] fields)
        {
            return await GetAlbumsSongsAsync<SongsResponse, AudioFieldsSong>("AudioLibrary.GetRecentlyPlayedSongs",
                                                                             ignorearticle, sortMethod, sortOrder,
                                                                             start, end, fields);
        }

        /// <summary>Scans the audio sources for new library items.</summary>
        public async Task<string> ScanAsync(string directory = "")
        {
            var method = new ParameteredMethodMessage<ScanParameters>
                             {
                                 Method = "AudioLibrary.Scan",
                                 Parameters = new ScanParameters { Directory = directory }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return result.Result;
        }

        private async Task<T> GetFilteredAlbumsSongsAsync<T, E>(string methodName,
                                                                bool ignorearticle,
                                                                SortMethod sortMethod,
                                                                SortOrder sortOrder,
                                                                int start, int end,
                                                                IEnumerable<E> fields,
                                                                int? artistId = null,
                                                                int? albumId = null,
                                                                int? genreId = null)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(E));

            Filter filter = null;

            if (artistId != null)
                filter = new Filter { ArtistId = artistId };

            if (albumId != null)
                filter = new Filter { AlbumId = albumId };

            if (genreId != null)
                filter = new Filter { GenreId = genreId };

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

        private async Task<T> GetAlbumsSongsAsync<T, E>(string methodName,
                                                        bool ignorearticle,
                                                        SortMethod sortMethod,
                                                        SortOrder sortOrder,
                                                        int start, int end,
                                                        IEnumerable<E> fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(E));

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