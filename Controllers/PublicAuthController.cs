using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurant_server.Entities;

namespace WebBlog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PublicAuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public PublicAuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("auth")]
    public ActionResult<PublicAuth> GetPublicAuth()
    {
        string audience = _configuration.GetValue<string>("ClientAuth:Audience");
        string domain = _configuration.GetValue<string>("ClientAuth:Domain");
        string clientid = _configuration.GetValue<string>("ClientAuth:ClientId");

        Console.WriteLine("Audience: " + audience);
        var publicAuth = new PublicAuth()
        {
            Audience = audience,
            Domain = domain,
            ClientId = clientid
        };
        return Ok(publicAuth);
    }
}
