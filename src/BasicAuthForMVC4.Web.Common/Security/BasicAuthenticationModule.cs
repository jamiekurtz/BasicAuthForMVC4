using System;
using System.Web;
using BasicAuthForMVC4.Common;

namespace BasicAuthForMVC4.Web.Common.Security
{
    public class BasicAuthenticationModule : IHttpModule
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public BasicAuthenticationModule()
            :this(ContainerManager.Resolve<IAuthenticationProvider>())
        {
        }

        public BasicAuthenticationModule(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += context_AuthenticateRequest;
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;

            string errorDescription;
            if (!_authenticationProvider.Authenticate(app.Context, out errorDescription))
            {
                app.Context.Response.Status = "401 Unauthorized";
                app.Context.Response.StatusCode = 401;
                app.Context.Response.StatusDescription = errorDescription;
                app.Context.Response.AddHeader("WWW-Authenticate", "Basic");
                app.Context.Response.End();
            }
        }

        public void Dispose() { }
    }
}