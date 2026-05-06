namespace CleanStarter.Application.Helpers
{
    public static class ScalarDocumentInfo
    {
        public static string GetScalarDocumentInfo()
        {
            string template = """
                Welcome to the official API documentation for the CleanStarter Enterprise Template.

                ## 🔐 Authentication & Security Flow
                This API uses a highly secure, Stateless OTP mechanism:
                1. Call `initiate-registration` or `forgot-password` to receive a `Token`.
                2. A secure OTP is sent to the user's email.
                3. Call `verify-registration` or `reset-password` using the `Token` + `OTP` to complete the action.

                ## 📌 Required Headers
                Please include the following headers in your requests where applicable:

                | Header Name | Value | Description | Required? |
                | :--- | :--- | :----- | :--- |
                | **Authorization** | `Bearer {token}` | JWT Token for secured endpoints. | Yes |
                | **Accept-Language** | `en` or `ar` | Determines the localization of messages and errors. | Optional (Default: en) |
                | **X-Api-Version** | `1.0` | Target API version. | Yes |

                ## ⚙️ General API Behavior
                All endpoints return a unified JSON wrapper (`ApiResponse<T>`).

                **✅ In Case of Success:**
                ```json
                {
                  "succeeded": true,
                  "message": "Operation completed successfully.",
                  "errors": {},
                  "data": { ... } 
                }
                ```

                **❌ In Case of Failure (Validation Errors):**
                ```json
                {
                  "succeeded": false,
                  "message": "Validation Errors Occurred.",
                  "errors": {
                    "Email": [ "Invalid email format." ]
                  },
                  "data": null 
                }
                ```
                """;

            return template;
        }
    }
}