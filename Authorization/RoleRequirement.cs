﻿using Microsoft.AspNetCore.Authorization;

namespace SCHAPP.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}
