using System;
using System.Net;
using System.Windows.Navigation;

namespace KodiRemote.Wp81.Core
{
    internal class AssociationUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            string tempUri = HttpUtility.UrlDecode(uri.ToString());
            if (tempUri.Contains("remote:"))
                return new Uri("/PageServers.xaml", UriKind.Relative);

            return uri;
        }
    }
}
