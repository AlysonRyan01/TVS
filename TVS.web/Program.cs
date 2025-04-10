using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TVS.Core;
using TVS.Core.Services;
using TVS.web;
using TVS.web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddMudServices();

builder.Services.AddHttpClient("email", client =>
{
    client.BaseAddress = new Uri($"http://localhost:5296/api/");
    client.Timeout = TimeSpan.FromSeconds(60);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

await builder.Build().RunAsync();
