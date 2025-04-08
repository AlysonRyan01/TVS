using Microsoft.AspNetCore.Components;
using MudBlazor;
using TVS.Core.Requests.Contact;

namespace TVS.web.Pages;

public partial class Home : ComponentBase
{
    private bool _open;
    private bool FormIsBusy;
    public ContactRequest Contact { get; set; } = new();

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private async Task SendForm()
    {
        FormIsBusy = true;
        try
        {
            Snackbar.Add("Email enviado com sucesso! Aguarde o retorno", Severity.Success);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            FormIsBusy = false;
        }
    }
    
    private void OpenDrawer()
    {
        _open = true;
    }
}