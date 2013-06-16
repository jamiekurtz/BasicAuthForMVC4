using System.Web;

namespace BasicAuthForMVC4.Web.Common.Security
{
    public interface IAuthenticationProvider
    {
        bool Authenticate(HttpContext context, out string errorDescription);
    }
}