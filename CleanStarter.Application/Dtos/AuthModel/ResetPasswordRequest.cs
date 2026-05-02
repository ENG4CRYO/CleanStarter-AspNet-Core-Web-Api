using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Dtos.AuthModel
{
    public class ResetPasswordRequest
    {
        public string ResetToken { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
