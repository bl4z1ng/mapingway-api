﻿namespace Mapingway.Application.Abstractions;

public interface IHasher
{
    string GenerateHash(string rawValue, string salt);
    string GenerateSalt();
}