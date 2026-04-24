using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace CleanStarter.Application.Features.Auth.Queries.Login
{
#if IsCQRS
    public class LoginQuery : IRequest<ApiResponse<AuthModel>>
    {
        public TokenRequestModel Model { get; set; }

        public LoginQuery(TokenRequestModel model)
        {
            Model = model;
        }
    }
#endif
}