using TVS.Api;
using TVS.Api.Services;
using TVS.Core;
using TVS.Core.Requests.Email;
using TVS.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

Configuration.FrontendUrl = builder.Configuration["FrontendUrl"] ?? string.Empty;
Configuration.BackendUrl = builder.Configuration["BackendUrl"] ?? string.Empty;

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Cors");
app.MapControllers();

var smtp = new ApiConfig.SmtpConfiguration();
app.Configuration.GetSection("Smtp").Bind(smtp);
ApiConfig.Smtp = smtp;

app.MapPost("/api/contato", async (
    SendEmailRequest request,
    IEmailService emailService,
    CancellationToken cancellationToken) =>
{
    var send = await emailService.SendAsync(request, cancellationToken);

    return send.IsSuccess ? Results.Ok(send) : Results.BadRequest(send);
});

app.MapMethods("/", new[] { "GET", "HEAD" }, () => Results.Ok("API TVS está online 🚀"));

app.Run();
