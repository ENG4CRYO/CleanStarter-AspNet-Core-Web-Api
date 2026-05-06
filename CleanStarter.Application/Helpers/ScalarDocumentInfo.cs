namespace CleanStarter.Application.Helpers
{
    public static class ScalarDocumentInfo
    {
        public static string GetScalarDocumentInfo()
        {
            string template = """
                
                # CleanStarter API Documentation

                Welcome to the official API documentation for the **CleanStarter Enterprise Template**.  
                This API is designed with scalability, security, and clean architecture principles in mind.

                ---

                ## 🔐 Authentication & Security Flow

                The API follows a **stateless OTP-based flow** for sensitive operations:

                1. `initiate-registration` / `forgot-password`  
                   → Returns a `RegisterToken` or `ResetToken`.

                2. The user receives a secure **One-Time Password (OTP)** via email.

                3. `verify-registration` / `reset-password`  
                 → Requires the Token + OTP to complete the process.

                > ⚠️ Tokens are temporary and must be used within their validity period.

                ---

                ## 📌 Required Headers

                Include the following headers where applicable:

                | Header Name        | Value              | Description                                      | Required |
                |-------------------|-------------------|--------------------------------------------------|----------|
                | Authorization     | Bearer {token}    | JWT token for secured endpoints                  | Yes      |
                | Accept-Language   | en / ar           | Controls response language (default: en)          | Optional |
                | X-Api-Version     | 1.0               | Specifies the target API version                 | Yes      |

                ---

                 ## ⚙️ General API Behavior

                ### 1. Standard Response Format

                All endpoints return a unified response structure: `ApiResponse<T>`

                #### ✅ Success Response
                ```json
                {
                  "succeeded": true,
                  "message": "Operation completed successfully.",
                  "errors": {},
                   "data": { }
                }
                """;

            return template;
        }
    }
}