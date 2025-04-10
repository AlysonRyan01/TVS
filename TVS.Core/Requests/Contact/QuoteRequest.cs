using Microsoft.AspNetCore.Http;

namespace TVS.Core.Requests.Contact;

public class QuoteRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFileCollection Files { get; set; } = null!;
}