using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace DevIO.App.Extensions
{
    public class RequesterClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequesterClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Identity", page = "/Account/Login", ReturnUrl = context.HttpContext.Request.Path.ToString() }));
                return;
            }

            if (!CustomerAuthorization.ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
                context.Result = new StatusCodeResult(403);
        }
    }
}

