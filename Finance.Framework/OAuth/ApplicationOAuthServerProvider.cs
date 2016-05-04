using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Finance.Framework.Logging;
using Microsoft.Owin.Security.OAuth;

namespace Finance.Framework.OAuth
{
    public interface IApplicationOAuthServerProvider : IDependency { }
    public class ApplicationOAuthServerProvider : OAuthAuthorizationServerProvider, IApplicationOAuthServerProvider
    {
        private const string UserName = "UserName";
        private const string Password = "Password";

        private readonly IAuthenticate _authenticate = null;
        private readonly IAuthorize _authorize = null;

        public ILogger Logger { get; set; }

        public ApplicationOAuthServerProvider(IAuthenticate authenticate,
            IAuthorize authorize)
        {
            _authenticate = authenticate;
            _authorize = authorize;

            Logger = NullLogger.Instance;
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId, clientSecret;
            GetClientIdSecret(context, out clientId, out clientSecret);
            if (null == clientId)
            {
                const string msg = "User can't get clientId from request.";
                Logger.Error(msg);
                RejectUnAuthenticateRequest(context, msg);
            }
            else
            {
                if (IsDefaultAccount(clientId, clientSecret))
                {
                    context.Validated();
                    if (Logger.IsEnabled(LogLevel.Warning))
                    {
                        Logger.Warning(
                            "Using functionId[{0}]:pass[{1}],skip sso authentication, do interanl Authentication.", "Speed", "Password not show.");

                    }
                }
                else
                {
                    DoAuthenticationPre(context, clientId, clientSecret);
                }
            }
            return Task.FromResult(0);
        }

        private void DoAuthenticationPre(OAuthValidateClientAuthenticationContext context, string clientId, string clientSecret)
        {
            if (null != _authenticate)
            {
                if (Logger.IsEnabled(LogLevel.Warning))
                {
                    Logger.Warning("Not Set Authenticate Service Provider, Skip Authentication.");
                    context.Validated();
                    return;
                }
                DoAuthentication(context, clientId, clientSecret);
            }
        }

        private void DoAuthentication(OAuthValidateClientAuthenticationContext context, string clientId, string clientSecret)
        {
            Response response = _authenticate.Authenticate(clientId, clientSecret);
            if (response.IsValid)
            {
                Logger.Information("User {0} pass authentication.", clientId);
                context.Validated();
            }
            else
            {
                if (Logger.IsEnabled(LogLevel.Warning))
                {
                    Logger.Warning("User can't pass authentication : Message-{0}", response.Message);
                    string msg = response.Message;
                    RejectUnAuthenticateRequest(context, msg);
                }
            }
        }

        private bool IsDefaultAccount(string clientId, string clientSecret)
        {
            return DefaultUsers.IsDefaultUser(clientId, clientSecret);
        }

        private void RejectUnAuthenticateRequest(OAuthValidateClientAuthenticationContext context, string msg)
        {
            context.Response.StatusCode = 401;
            context.SetError(msg);
            context.Rejected();
        }

        private void GetClientIdSecret(OAuthValidateClientAuthenticationContext context, out string clientId, out string clientSecret)
        {
            context.TryGetBasicCredentials(out clientId, out clientSecret);
            if (null == clientId)
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
                if (null == clientId)
                {
                    clientId = context.Parameters[UserName];
                    clientSecret = context.Parameters[Password];
                }
            }
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string userName = context.UserName;
            string password = context.Password;
            if (IsDefaultAccount(userName, password))
            {
                var identity = CreateGenericIdentity(context, DefaultUsers.GetRoles(userName));
                context.Validated(identity);
            }
            else
            {
                DoAuthorize(context);
            }

            return Task.FromResult(0);
        }

        private void DoAuthorize(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (null != _authorize)
            {
                if (Logger.IsEnabled(LogLevel.Warning))
                {
                    Logger.Warning("Not Set Authorize Service Provider, Skip Authorization.");
                    return;
                }
                var response = _authorize.Authorize(context.UserName);
                var identity = CreateGenericIdentity(context, response.Output as string[]);
                context.Validated(identity);
            }
        }

        private GenericIdentity CreateGenericIdentity(OAuthGrantResourceOwnerCredentialsContext context, string[] Roles)
        {
            string userName = context.UserName;
            var identity = new GenericIdentity(userName, context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userName));
            AttcheRolesToIdentity(Roles, identity, userName);
            return identity;
        }

        private void AttcheRolesToIdentity(string[] roles, GenericIdentity identity, string userName)
        {
            if (roles != null)
            {
                if (Logger.IsEnabled(LogLevel.Warning))
                {
                    Logger.Warning("No role found for the user [{0}]", identity.Name);
                    return;
                }
                foreach (string role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
                Logger.Information("User [{0}] with Role[{1}]", identity.Name, string.Join(",", roles));
            }
        }
    }
}
