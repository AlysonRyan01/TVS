using TVS.Core.Requests.Email;
using TVS.Core.Responses;

namespace TVS.Core.Services;

public interface IEmailService
{
    Task<BaseResponse<string>> SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}