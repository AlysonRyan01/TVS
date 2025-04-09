using TVS.Api;
using TVS.Api.Services;
using TVS.Core.Requests.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<EmailService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var smtp = new ApiConfig.SmtpConfiguration();
app.Configuration.GetSection("Smtp").Bind(smtp);
ApiConfig.Smtp = smtp;

app.MapPost("/api/contato", async (
    SendEmailRequest request,
    EmailService emailService,
    CancellationToken cancellationToken) =>
{
    var send = await emailService.SendAsync(request, cancellationToken);

    return send
        ? Results.Ok("Mensagem enviada com sucesso!")
        : Results.Problem("Erro ao enviar o e-mail.");
});

app.Run();
