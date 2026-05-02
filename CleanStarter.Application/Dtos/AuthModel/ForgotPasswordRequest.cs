using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Dtos.AuthModel
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}
