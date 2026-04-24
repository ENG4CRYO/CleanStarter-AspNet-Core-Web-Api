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
        Task<ApiResponse<AuthModel>> RegisterAsync(RegisterModel model);
        Task<ApiResponse<AuthModel>> GetTokenAsync(TokenRequestModel model);
        Task<ApiResponse<AuthModel>> RefreshTokenAsync(string token);
        Task<ApiResponse<bool>> RevokeTokenAsync(string token);
    }
}
#endif