namespace CleanStarter.Application.Dtos.AuthModel
{
    public class VerifyRegistrationRequest
    {
        public string RegisterToken { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }
}