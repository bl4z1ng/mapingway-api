﻿using System.Text;
using Mapingway.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.API.OptionsSetup;

public class TokenValidationParametersSetup : IConfigureOptions<TokenValidationParameters>
{
    private readonly JwtOptions _jwtOptions;


    public TokenValidationParametersSetup(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }


    public void Configure(TokenValidationParameters options)
    {
        options.ValidateIssuer = true;
        options.ValidateAudience = true;
        options.ValidateIssuerSigningKey = true;
        options.ValidIssuer = _jwtOptions.Issuer;
        options.ValidAudience = _jwtOptions.Audience;
        options.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        options.ClockSkew = TimeSpan.FromSeconds(15);
    }
}