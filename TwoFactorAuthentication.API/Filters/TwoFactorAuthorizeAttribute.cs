namespace TwoFactorAuthentication.API.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Helpers;

    public class TwoFactorAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (principal != null)
            {
                string preSharedKey = principal.FindFirst("PSK").Value;
                bool hasValidTotp = actionContext.Request.HasValidTotp(preSharedKey);

                if (hasValidTotp)
                {
                    return Task.FromResult<object>(null);
                }
            }
            actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.Unauthorized,
                new CustomError {Code = 100, Message = "Time sensitive passcode is invalid"});
            return Task.FromResult<object>(null);
        }
    }

    public class CustomError
    {
        public int Code { get; set; }

        public string Message { get; set; }
    }
}