﻿namespace SWD.SheritonHotel.Domain.DTO.Auth
{
    public class AuthServiceResponseDto
    {
        public bool IsSucceed { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }
}