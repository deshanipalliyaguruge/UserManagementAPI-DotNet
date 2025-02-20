namespace UserManagementAPI.Models.AuthModels
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }

        public LoginResponse(string token, string message)
        {
            Token = token;
            Message = message;
        }
    }
}
