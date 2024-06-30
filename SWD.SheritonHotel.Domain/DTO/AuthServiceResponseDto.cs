namespace Dtos
{
    public class AuthServiceResponseDto
    {
        public bool IsSucceed { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
    }
}