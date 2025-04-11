using Microsoft.AspNetCore.Http;
using TVS.Core.Model;

namespace TVS.Core.Requests.Email;

public class SendEmailRequest
{
    public string Name { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public List<AttachmentFile?> Attachments { get; set; } = new();
}