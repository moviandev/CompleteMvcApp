﻿using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DevIO.App.Extensions
{
    public class CustomerAuthorization
	{
		public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
        {
			return context.User.Identity.IsAuthenticated &&
				context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }
	}
}

