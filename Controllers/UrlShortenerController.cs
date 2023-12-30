using Microsoft.AspNetCore.Mvc;
using System;
using urlShorterner;
using urlShorterner.Interfaces;

public class UrlShortenerService : IUrlShortenerService
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

[Route("api/[controller]")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly IUrlShortenerService urlShortenerService;

    public UrlShortenerController(IUrlShortenerService urlShortenerService)
    {
        this.urlShortenerService = urlShortenerService ?? throw new ArgumentNullException(nameof(urlShortenerService));
    }

    [HttpPost]
    public IActionResult ShortenUrl([FromBody] UrlShortener request)
    {
        bool Execution = true;
        string WarningMessage = "";
        string shortUrl = "";

        if (Execution)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.LongUrl))
            {
                WarningMessage = "Invalid request";
                Execution = false;
            }

        }
        string shortKey = null;
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
            var hasShortKey = urlShortenerService.TryGetLongUrl(shortKey, out var longUrl);
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
                urlShortenerService.AddUrlMapping(shortKey, request.LongUrl);

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
        if (urlShortenerService.TryGetLongUrl(shortKey, out var longUrl))
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




