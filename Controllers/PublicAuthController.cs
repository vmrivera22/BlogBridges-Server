using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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
        var keyVaultEndpoint = new Uri(_configuration["VaultKey"]);
        var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());

        KeyVaultSecret audience = secretClient.GetSecret("audience");
        KeyVaultSecret domain = secretClient.GetSecret("domain");
        KeyVaultSecret clientid = secretClient.GetSecret("clientid");

        Console.WriteLine("Audience: " + audience);
        var publicAuth = new PublicAuth()
        {
            Audience = audience.Value,
            Domain = domain.Value,
            ClientId = clientid.Value
        };
        return Ok(publicAuth);
    }
}
