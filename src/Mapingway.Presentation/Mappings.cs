using Mapingway.Application.Features.Auth.Logout;
using Mapster;

namespace Mapingway.Presentation;

public static class Mappings
{
    public static void Add()
    {
        TypeAdapterConfig<(string Email, string RefreshToken), LogoutCommand>.NewConfig()
            .Map(target => target.Email, src => src.Email)
            .Map(target => target.RefreshToken, src => src.RefreshToken);
    }
}
