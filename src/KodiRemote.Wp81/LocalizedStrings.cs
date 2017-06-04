using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81
{
    public class LocalizedStrings
    {
        private static readonly AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}