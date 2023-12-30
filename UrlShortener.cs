using System.ComponentModel.DataAnnotations;

namespace urlShorterner
{
    public class UrlShortener
    {
        public string LongUrl { get; set; }
        [MaxLength(10, ErrorMessage = "ShortUrl must not exceed 10 characters.")]
        [MinLength(1, ErrorMessage = "ShortUrl must be at least 1 characters.")]
        public string? ShortUrl { get; set; }
    }
}