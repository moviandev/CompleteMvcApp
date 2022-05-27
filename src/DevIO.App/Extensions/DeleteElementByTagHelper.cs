using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DevIO.App.Extensions
{
    [HtmlTargetElement("*", Attributes="supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes="supress-by-claim-value")]
	public class DeleteElementByTagHelper : TagHelper
	{
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteElementByTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if(output == null)
                throw new ArgumentNullException(nameof(context));

            var hasAccess = CustomerAuthorization.ValidateUserClaims(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (hasAccess)
                return;

            output.SuppressOutput();
        }
    }
}

