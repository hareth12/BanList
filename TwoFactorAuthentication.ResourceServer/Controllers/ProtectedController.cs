using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace AngularJSAuthentication.ResourceServer.Controllers
{
    [Authorize]
    [RoutePrefix("api/protected")]
    public class ProtectedController : ApiController
    {
        [Route("")]
        public IEnumerable<object> Get()
        {
            //string token = "";
            //Microsoft.Owin.Security.AuthenticationTicket ticket = Startup.OAuthBearerOptions.AccessTokenFormat.Unprotect(token);

            var identity = User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                return identity.Claims.Select(c => new
                {
                    c.Type,
                    c.Value
                });
            }
            return null;
        }
    }
}