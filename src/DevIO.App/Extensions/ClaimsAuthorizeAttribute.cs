using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Extensions
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
		public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequesterClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }
}

