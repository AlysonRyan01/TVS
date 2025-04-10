using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TVS.Core.Requests.Email;
using TVS.Core.Responses;
using TVS.Core.Services;

namespace TVS.web.Services;

public class EmailService : IEmailService
{
    private readonly HttpClient _client;

    public EmailService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("email");
    }
    
    public async Task<BaseResponse<string>> SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _client.PostAsJsonAsync($"contato", request);
            
            return await result.Content.ReadFromJsonAsync<BaseResponse<string>>()
                   ?? new BaseResponse<string>(null, 400, "Falha ao enviar o email");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<string>(null, 404, "O serviço de pedido não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<string>(null, 401, "Credenciais inválidas para acessar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<string>(null, 400, "Dados de pedido incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<string>(null, 503, $"Erro no serviço de pedido: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<string>(null, 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<string>(null, 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>(null, 500, $"Falha no pedido: {ex.Message}");
        }
    }
}