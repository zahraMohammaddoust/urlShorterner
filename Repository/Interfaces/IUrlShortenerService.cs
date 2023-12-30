namespace urlShorterner.Repository.Interfaces
{
    public interface IUrlShortenerService
    {
        bool TryGetLongUrl(string shortKey, out string longUrl);
        void AddUrlMapping(string shortKey, string longUrl);
    }
}
