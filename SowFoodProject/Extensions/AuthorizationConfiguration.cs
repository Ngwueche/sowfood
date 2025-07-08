using Microsoft.AspNetCore.Authorization;

namespace ASS.Api.Extensions
{
    public static class PolicyNames
    {
        public const string RequireAdminOnly = "RequireAdminOnly";
        public const string OrdinaryUser = "OrdinaryUser";
        public const string ApplicationUser = "ApplicationUser";
    }

    public static class AuthorizationConfiguration
    {
        //public static void ConfigureAuthorization(AuthorizationOptions options)
        //{
        //    // Single role policies
        //    options.AddPolicy(PolicyNames.RequireAdminOnly,
        //        policy => policy.RequireRole(UserRoles.Admin));

        //    options.AddPolicy(PolicyNames.OrdinaryUser,
        //        policy => policy.RequireRole(UserRoles.OrdinaryUser));

        //    options.AddPolicy(PolicyNames.ApplicationUser,
        //        policy => policy.RequireRole(UserRoles.ApplicationUser));
        //}
    }
}
