using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Dtos.AuthModel
{
    public class RevokeTokenRequest
    {
        public string Token { get; set; } = default!;
    }
}
