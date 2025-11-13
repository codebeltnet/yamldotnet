using System.Net;
using System.Threading.Tasks;
using Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Authentication;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;
using YamlDotNet.Serialization.NamingConventions;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml
{
    public class AuthorizationResponseHandlerTest : Test
    {
        public AuthorizationResponseHandlerTest(ITestOutputHelper output) : base(output)
        {
        }


        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.None)]
        public async Task AuthorizationResponseHandler_BasicScheme_ShouldRenderResponseInYaml_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails) // # legacy test from  Cuemon.Extensions.AspNetCore.Authentication
        {
            using (var startup = WebHostTestFactory.Create(services =>
                   {
                       services.AddYamlExceptionResponseFormatter(o => o.Settings.NamingConvention = PascalCaseNamingConvention.Instance);
                       services.AddAuthorizationResponseHandler();
                       services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                           .AddBasic(o =>
                           {
                               o.RequireSecureConnection = false;
                               o.Authenticator = (username, password) => null;
                           });
                       services.AddAuthorization(o =>
                       {
                           o.FallbackPolicy = new AuthorizationPolicyBuilder()
                               .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                               .RequireAuthenticatedUser()
                               .Build();

                       });
                       services.AddRouting();
                       services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseAuthentication();
                       app.UseAuthorization();
                       app.UseEndpoints(endpoints =>
                       {
                           endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                       });
                   }))
            {
                var client = startup.Host.GetTestClient();
                var bb = new BasicAuthorizationHeaderBuilder()
                    .AddUserName("Agent")
                    .AddPassword("Test");

                client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

                var result = await client.GetAsync("/");
                var content = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(content);

                Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
                Assert.Equal("Basic realm=\"AuthenticationServer\"", result.Headers.WwwAuthenticate.ToString());
                if (sensitivityDetails == FaultSensitivityDetails.All)
                {
                    Assert.Equal("""
                                 Error:
                                   Status: 401
                                   Code: Unauthorized
                                   Message: The request has not been applied because it lacks valid authentication credentials for the target resource.
                                   Failure:
                                     Type: Cuemon.AspNetCore.Http.UnauthorizedException
                                     Message: The request has not been applied because it lacks valid authentication credentials for the target resource.
                                     StatusCode: 401
                                     ReasonPhrase: Unauthorized
                                     Inner:
                                       Type: System.Security.SecurityException
                                       Message: Unable to authenticate Agent.

                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
                }
                else
                {
                    Assert.Equal("""
                                 Error:
                                   Status: 401
                                   Code: Unauthorized
                                   Message: The request has not been applied because it lacks valid authentication credentials for the target resource.

                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
                }
            }
        }
    }
}
