using Microsoft.AspNetCore.Mvc;
using urlShorterner;
using urlShorterner.Repository.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly IUrlShortenerService UrlShorternerServiceRepository;

    public UrlShortenerController(IUrlShortenerService UrlShorternerServiceRepository)
    {
        this.UrlShorternerServiceRepository = UrlShorternerServiceRepository ?? throw new ArgumentNullException(nameof(UrlShorternerServiceRepository));
    }

    [HttpPost]
    public IActionResult ShortenUrl([FromBody] UrlShortener request)
    {
        bool Execution = true;
        string WarningMessage = "";
        string shortUrl = "";
        string shortKey = null;

        if (Execution)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.LongUrl))
            {
                WarningMessage = "Invalid request";
                Execution = false;
            }

        }
        
        if (request.ShortUrl != null)
        {
            shortKey = request.ShortUrl;
        }
        if (request.ShortUrl == null)
        {
            shortKey = GenerateShortKey();
        }
        if (Execution)
        {
            var hasShortKey = UrlShorternerServiceRepository.TryGetLongUrl(shortKey, out var longUrl);
            if (hasShortKey)
            {
                WarningMessage = "this short url is used, please choose another short url.";
                Execution = false;
            }

        }
        if (Execution)
        {
            try
            {
                UrlShorternerServiceRepository.AddUrlMapping(shortKey, request.LongUrl);

                 shortUrl = $"{Request.Scheme}://{Request.Host}/api/UrlShortener/{shortKey}";
            }
            catch (Exception ex)
            {
                WarningMessage = ex.Message;
            }
        }
        if (Execution)
        {
            return Ok(new { ShortUrl = shortUrl, ShortKey = shortKey });
        }
        else
        {
            return BadRequest(WarningMessage);
        }
                
    }

    [HttpGet("{shortKey}")]
    public ActionResult RedirectUrl(string shortKey)
    {
        if (UrlShorternerServiceRepository.TryGetLongUrl(shortKey, out var longUrl))
        {
            return new RedirectResult(longUrl);
        }
        else
        {
            return NotFound("Short URL not found");
        }
    }

    private string GenerateShortKey()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 8);
    }
}




