﻿using Mapingway.Common;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Contracts.Abstractions.Authentication;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
}