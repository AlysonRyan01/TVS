using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TVS.Core.Model;
using TVS.Core.Requests.Contact;
using TVS.Core.Requests.Email;
using TVS.Core.Services;

namespace TVS.web.Pages;

public partial class Orcamento : ComponentBase
{
    private bool _formIsBusy;
    private ContactRequest Contact { get; set; } = new();
    private List<IBrowserFile> _etiquetaFiles = new();
    private List<IBrowserFile> _defeitoFiles = new();
    
    private bool _isButtonDisabled;
    private TimeSpan _buttonCountdown = TimeSpan.Zero;
    private Timer? _countdownTimer;

    
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IEmailService EmailService { get; set; } = null!;

    
    private async Task SendForm()
    {
        _formIsBusy = true;
        _isButtonDisabled = true;
        _buttonCountdown = TimeSpan.FromMinutes(2);
        
        _countdownTimer = new Timer(_ =>
        {
            if (_buttonCountdown.TotalSeconds <= 1)
            {
                _countdownTimer?.Dispose();
                _isButtonDisabled = false;
                _buttonCountdown = TimeSpan.Zero;
            }
            else
            {
                _buttonCountdown = _buttonCountdown.Subtract(TimeSpan.FromSeconds(1));
            }

            InvokeAsync(StateHasChanged);
        }, null, 0, 1000);

        try
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Contact.Email) || !Contact.Email.Contains("@"))
                    Snackbar.Add("Por favor, insira um endereço de e-mail válido", Severity.Error);
                
                if (string.IsNullOrWhiteSpace(Contact.Name))
                    Snackbar.Add("Por favor, insira um nome", Severity.Error);
                
                if (string.IsNullOrWhiteSpace(Contact.Message))
                    Snackbar.Add("Por favor, insira um texto", Severity.Error);
                
                if (string.IsNullOrWhiteSpace(Contact.Phone))
                    Snackbar.Add("Por favor, insira um telefone", Severity.Error);
                
                if (!_etiquetaFiles.Any() || !_defeitoFiles.Any())
                {
                    if (!_etiquetaFiles.Any())
                        Snackbar.Add("Por favor, insira pelo menos uma imagem da etiqueta", Severity.Error);
    
                    if (!_defeitoFiles.Any())
                        Snackbar.Add("Por favor, insira pelo menos uma imagem ou vídeo do defeito", Severity.Error);
                }

                var attachments = new List<AttachmentFile?>();

                foreach (var file in _etiquetaFiles.Concat(_defeitoFiles))
                {
                    using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    attachments.Add(new AttachmentFile
                    {
                        FileName = file.Name,
                        Content = ms.ToArray(),
                        ContentType = file.ContentType
                    });
                }

                var request = new SendEmailRequest
                {
                    Name = Contact.Name,
                    Body = $"{Contact.Message}<br/><br/>Email: {Contact.Email}<br/>Telefone: {Contact.Phone}",
                    Attachments = attachments,
                    Subject = "Orçamento via site",
                };
                
                var result = await EmailService.SendAsync(request);
                
                if (result.IsSuccess)
                    Snackbar.Add("Email enviado com sucesso! Aguarde nosso retorno", Severity.Success);
                else
                    Snackbar.Add("Erro ao enviar a mensagem, tente novamente mais tarde", Severity.Error);
            }
            catch (ArgumentException ex)
            {
                Snackbar.Add(ex.Message, Severity.Warning);
            }
            catch (HttpRequestException ex)
            {
                Snackbar.Add("Erro de conexão. Verifique sua internet e tente novamente", Severity.Error);
                Console.WriteLine($"HTTP Error: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                Snackbar.Add("Tempo limite excedido. Tente novamente mais tarde", Severity.Error);
                Console.WriteLine($"Timeout Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Snackbar.Add("Operação inválida. Recarregue a página e tente novamente", Severity.Error);
                Console.WriteLine($"Invalid Operation: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Snackbar.Add("Ocorreu um erro inesperado. Por favor, tente novamente", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            Contact = new();
            _etiquetaFiles.Clear();
            _defeitoFiles.Clear();
            _formIsBusy = false;
        }
    }
    
    private async Task LoadEtiquetaFiles(InputFileChangeEventArgs e)
    {
        var newFiles = e.GetMultipleFiles();

        foreach (var file in newFiles)
        {
            if (!_etiquetaFiles.Any(f => f.Name == file.Name && f.Size == file.Size))
            {
                _etiquetaFiles.Add(file);
            }
        }
    }

    private async Task LoadDefeitoFiles(InputFileChangeEventArgs e)
    {
        var newFiles = e.GetMultipleFiles();

        foreach (var file in newFiles)
        {
            if (!_defeitoFiles.Any(f => f.Name == file.Name && f.Size == file.Size))
            {
                _defeitoFiles.Add(file);
            }
        }
    }


}