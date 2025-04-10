using System.Net;
using System.Net.Mail;
using TVS.Core.Requests.Email;
using TVS.Core.Responses;
using TVS.Core.Services;

namespace TVS.Api.Services;

public class EmailService : IEmailService
{
    private readonly string _toEmail = "tvseletronica@tvseletronica.com.br";
    private readonly string _toName = "Site TVS";
    private readonly string _subject = "Mensagem do site";
    private readonly string _fromEmail = "tvseletronica@tvseletronica.com.br";

    public async Task<BaseResponse<string>> SendAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var smtp = new SmtpClient();
            smtp.Host = ApiConfig.Smtp.Host;
            smtp.Port = ApiConfig.Smtp.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(ApiConfig.Smtp.Username, ApiConfig.Smtp.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            using var mail = new MailMessage();
            mail.From = new MailAddress(_fromEmail, request.Name);
            mail.Subject = _subject;
            mail.Body = request.Body;
            mail.IsBodyHtml = true;

            mail.To.Add(new MailAddress(_toEmail, _toName));

            await smtp.SendMailAsync(mail, cancellationToken);
            
            return new BaseResponse<string>("Mensagem enviada com sucesso!", 200, "Mensagem enviada com sucesso!");
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
                Console.WriteLine("Erro interno: " + ex.InnerException.Message);
            return new BaseResponse<string>("Erro ao enviar a mensagem!", 500, "Erro ao enviar a mensagem!");
        }
    }
}