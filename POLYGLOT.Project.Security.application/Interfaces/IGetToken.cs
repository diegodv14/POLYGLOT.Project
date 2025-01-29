﻿using POLYGLOT.Project.Security.application.Dto;

namespace POLYGLOT.Project.Security.application.Interfaces
{
    public interface IGetToken
    {
        Task<AuthResponse> GetToken(AuthRequest request);
    }
}
