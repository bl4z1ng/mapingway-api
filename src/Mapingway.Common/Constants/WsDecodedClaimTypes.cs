using System.IdentityModel.Tokens.Jwt;

namespace Mapingway.Common.Constants;

public static class WsDecodedClaimTypes
{
    public static readonly IDictionary<string, string> Keys = new Dictionary<string, string>
    {
        { JwtRegisteredClaimNames.Email, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" }
    };
}