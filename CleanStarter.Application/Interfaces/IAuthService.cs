using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;

#if IsRepository
namespace CleanStarter.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthModel>> RegisterAsync(RegisterModel model, CancellationToken cancellationToken);
        Task<ApiResponse<AuthModel>> GetTokenAsync(TokenRequestModel model, CancellationToken cancellationToken);
        Task<ApiResponse<AuthModel>> RefreshTokenAsync(string token, CancellationToken cancellationToken);
        Task<ApiResponse<bool>> RevokeTokenAsync(string token, CancellationToken cancellationToken);
    }
}
#endif