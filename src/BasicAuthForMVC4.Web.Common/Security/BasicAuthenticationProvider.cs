using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using BasicAuthForMVC4.Common.Configuration;
using log4net;

namespace BasicAuthForMVC4.Web.Common.Security
{
    public class BasicAuthenticationProvider : IAuthenticationProvider
    {
        public const string RequireSecureConnectionKey = "RequireSecureConnection";
        public const bool RequireSecureConnectionDefault = true;

        public const string AuthorizationSchemeBasic = "Basic";

        private readonly IConfigurationManager _configurationManger;
        private readonly ILog _logger;

        public BasicAuthenticationProvider(IConfigurationManager configurationManger, ILog logger)
        {
            _configurationManger = configurationManger;
            _logger = logger;
        }

        public bool Authenticate(HttpContext context, out string errorDescription)
        {
            if (!context.Request.Headers.AllKeys.Contains("Authorization"))
            {
                errorDescription = "Authorization header is missing";
                return false;
            }

            var requireSslString = _configurationManger.GetAppSetting(RequireSecureConnectionKey);
            bool requireSsl;
            if(!bool.TryParse(requireSslString, out requireSsl))
            {
                requireSsl = RequireSecureConnectionDefault;
            }

            if (requireSsl && !context.Request.IsSecureConnection)
            {
                errorDescription = "SSL required";
                return false;
            }

            var authHeader = context.Request.Headers["Authorization"];

            _logger.DebugFormat("AuthHeader = {0}", authHeader);

            IPrincipal principal;
            if (TryGetPrincipal(authHeader, out principal, out errorDescription))
            {
                context.User = principal;
                return true;
            }

            return false;
        }

        private bool TryGetPrincipal(string authHeader, out IPrincipal principal, out string errorDescription)
        {
            principal = null;

            if (string.IsNullOrEmpty(authHeader))
            {
                errorDescription = "Missing authorization header";
                return false;
            }

            if (authHeader.StartsWith(AuthorizationSchemeBasic))
            {
                return TryParseAuthHeaderBasic(authHeader, out principal, out errorDescription);
            }

            errorDescription = "Unsupported authorization scheme";
            return false;
        }

        private bool TryParseAuthHeaderBasic(string authHeader, out IPrincipal principal, out string errorDescription)
        {
            string[] credentials;
            principal = null;
            errorDescription = string.Empty;

            try
            {
                var schemeLength = AuthorizationSchemeBasic.Length;
                var base64Credentials = authHeader.Substring(schemeLength + 1);
                var base64Bytes = Convert.FromBase64String(base64Credentials);
                credentials = Encoding.ASCII.GetString(base64Bytes).Split(':');
            }
            catch (Exception ex)
            {
                _logger.WarnFormat("Error parsing authorization header '{0}': {1}", authHeader, ex);
                errorDescription = "Invalid authorization header";
                return false;
            }

            if (credentials.Length != 2 ||
                string.IsNullOrEmpty(credentials[0]) ||
                string.IsNullOrEmpty(credentials[0]))
            {
                _logger.WarnFormat("Error parsing authorization header '{0}'", authHeader);
                errorDescription = "Invalid authorization header";
                return false;
            }

            var username = credentials[0];
            var password = credentials[1];

            long userId;
            var roles = new List<string>();
            if(!ValidateUser(username, password, out userId, roles))
            {
                errorDescription = "Invalid username or password";
                return false;
            }

            var identity = new GenericIdentity(username);
            principal = new GenericPrincipal(identity, roles.ToArray());
            
            return true;
        }

        private bool ValidateUser(string username, string password, out long userId, List<string> roles)
        {
            userId = 0;

            try
            {
                if (username == password)
                {
                    roles.Add("Administrator");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Error looking up username and password in database: {0}", ex);
            }

            return false;
        }
    }
}