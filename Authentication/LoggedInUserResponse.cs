namespace E_Com_Monolithic.Authentication
{
    public class LoggedInUserResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; } = false;
    }
}
