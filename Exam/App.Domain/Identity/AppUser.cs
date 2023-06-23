﻿using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    [MinLength(1)] [MaxLength(32)]
    public string FirstName { get; set; } = default!;

    [MinLength(1)] [MaxLength(32)]
    public string LastName { get; set; } = default!;

    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }
}