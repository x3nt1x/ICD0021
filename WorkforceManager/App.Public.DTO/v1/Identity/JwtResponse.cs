﻿namespace App.Public.DTO.v1.Identity;

public class JwtResponse
{
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}