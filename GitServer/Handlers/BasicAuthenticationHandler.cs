using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using GitServer.ApplicationCore.Models;
using Microsoft.Extensions.Logging;

namespace GitServer.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.NoResult();

            string authHeader = Request.Headers["Authorization"];
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            string token = authHeader.Substring("Basic ".Length).Trim();
            string credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            string[] credentials = credentialString.Split(':');

            if (credentials.Length != 2)
                return AuthenticateResult.Fail("More than two strings seperated by colons found");

            ClaimsPrincipal principal = await Options.SignInAsync(credentials[0], credentials[1]);

            if (principal != null)
            {
                AuthenticationTicket ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), BasicAuthenticationDefaults.AuthenticationScheme);
                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Wrong credentials supplied");
        }
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            return base.HandleForbiddenAsync(properties);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            string headerValue = $"{BasicAuthenticationDefaults.AuthenticationScheme} realm=\"{Options.Realm}\"";
            Response.Headers.Append(Microsoft.Net.Http.Headers.HeaderNames.WWWAuthenticate, headerValue);
            return base.HandleChallengeAsync(properties);
        }
    }

    public class BasicAuthenticationOptions : AuthenticationSchemeOptions, IOptions<BasicAuthenticationOptions>
    {
        private string _realm="GitServer";

        public IServiceProvider ServiceProvider { get; set; }
        public BasicAuthenticationOptions Value => this;
        public string Realm
        {
            get { return _realm; }
            set
            {
                _realm = value;
            }
        }

        public async Task<ClaimsPrincipal> SignInAsync(string userName, string password)
        {
            User user = new User() {
                Name=userName,
                Password=password
            };//await UserManager.FindByNameAsync(userName);
                             //No user with the specified name found
            if (user == null)
                return null;

            //Wrong password supplied
            //if (!(await UserManager.CheckPasswordAsync(user, password)))
            //    return null;
            var identity = new ClaimsIdentity(BasicAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }

    public static class BasicAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Basic";
    }
    public static class BasicAuthenticationExtensions
    {
        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder)
            => builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddBasic(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<BasicAuthenticationOptions> configureOptions)
        {
            //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<BasicAuthenticationOptions>, BasicAuthenticationOptions>());
            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}