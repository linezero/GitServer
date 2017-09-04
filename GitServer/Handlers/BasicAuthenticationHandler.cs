using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

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
        private string _realm;

        public IServiceCollection ServiceCollection { get; set; }
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
            using (var serviceScope = ServiceCollection.BuildServiceProvider().CreateScope())
            {
                var _user = serviceScope.ServiceProvider.GetService<IRepository<User>>();
                var user = _user.List(r => r.Name == userName && r.Password == password).FirstOrDefault();
                if (user == null)
                    return null;
                var identity = new ClaimsIdentity(BasicAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                var principal = new ClaimsPrincipal(identity);
                return principal;
            }
        }
    }

    public static class BasicAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Basic";
    }
    public static class BasicAuthenticationExtensions
    {
        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder)
            => builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme, _ => { _.ServiceCollection = builder.Services;_.Realm = "GitServer"; });

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddBasic(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<BasicAuthenticationOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptions<BasicAuthenticationOptions>, BasicAuthenticationOptions>());
            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}