using Microsoft.AspNetCore.Mvc;

namespace ExemploProxyReverso.Api.Controllers;

/// <summary>
/// Sample controller.
/// </summary>
[ApiController]
public class HttpController : ControllerBase
{
    /// <summary>
    /// Returns a 200 response dumping all info from the incoming request.
    /// </summary>
    [HttpGet, HttpPost]
    [Route("/api/dump")]
    public IActionResult Dump()
    {
        // Dá para ser híbrido:
        // parte das rotas direcionadas pelo Proxy Reverso,
        // parte das rotas definidas em Controllers.
        var result = new
        {
            Request.Protocol,
            Request.Method,
            Request.Scheme,
            Host = Request.Host.Value,
            PathBase = Request.PathBase.Value,
            Path = Request.Path.Value,
            Query = Request.QueryString.Value,
            Headers = Request.Headers.ToDictionary(kvp => kvp.Key, kvp => string.Join(',', kvp.Value)),
            Time = DateTimeOffset.UtcNow
        };

        return Ok(result);
    }
}
