﻿using Mapingway.SharedKernel;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Contracts.Authentication;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
}