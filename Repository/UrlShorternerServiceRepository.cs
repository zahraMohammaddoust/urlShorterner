using urlShorterner.Repository.Interfaces;

namespace urlShorterner.Repository
{
    public class UrlShorternerServiceRepository : IUrlShortenerService
    {
        private readonly Dictionary<string, string> urlMappings = new Dictionary<string, string>();

        public bool TryGetLongUrl(string shortKey, out string longUrl)
        {
            return urlMappings.TryGetValue(shortKey, out longUrl);
        }

        public void AddUrlMapping(string shortKey, string longUrl)
        {
            urlMappings.Add(shortKey, longUrl);
        }
    }
}
