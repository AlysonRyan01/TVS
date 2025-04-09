using System.Net;
using System.Net.Mail;
using TVS.Core.Requests.Email;

namespace TVS.Api.Services;

public class EmailService
{
    private readonly string _toEmail = "tvseletronica@tvseletronica.com.br";
    private readonly string _toName = "Site TVS";
    private readonly string _subject = "redirecionado pelo site";
    
    private readonly string _fromEmail = "tvseletronica@tvseletronica.com.br";
    
    public async Task<bool> SendAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Body)) throw new ArgumentException("Body cannot be empty", nameof(request.Body));

        try
        {
            using var smtp = new SmtpClient()
            {
                Host = ApiConfig.Smtp.Host,
                Port = ApiConfig.Smtp.Port,
                Credentials = new NetworkCredential(ApiConfig.Smtp.Username, ApiConfig.Smtp.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Timeout = 10000
            };

            using var mail = new MailMessage()
            {
                From = new MailAddress(_fromEmail, request.Name),
                Subject = _subject,
                Body = request.Body,
                IsBodyHtml = true
            };

            mail.To.Add(new MailAddress(_toEmail, _toName));

            await smtp.SendMailAsync(mail, cancellationToken);
            return true;
        }
        catch (Exception ex) when (ex is SmtpException or OperationCanceledException)
        {
            Console.WriteLine($"Failed to send email:, {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            return false;
        }
    }

}