using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Dtos.AuthModel
{
    public class ResetPasswordCacheDto
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }
}
