using System.Security.Claims;


namespace eGranjaCAT.Api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new Exception("User ID claim not found.");

            return userIdClaim.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.Role);
            if (userIdClaim == null) throw new Exception("User role claim not found.");

            return userIdClaim.Value;
        }
    }
}
