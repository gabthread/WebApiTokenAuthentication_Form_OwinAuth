using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApiTokenAuthentication.Security;

[assembly: OwinStartup(typeof(WebApiTokenAuthentication.Startup))]

namespace WebApiTokenAuthentication
{
    //This class is use to configue the OAuthorization Server
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll); //enable cors origin request
           

            var myProvider = new MyAuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"), //url where we get the signed token from (i.e. localhost:1236/token)
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1), //We’ve specified the expiry for token to be 24 hours, so if the user tried to use the same token for authentication after 24 hours from the issue time, his request will be rejected and HTTP status code 401 is returned
                Provider = myProvider
            };



            //Token Generation            
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()).UseStageMarker(PipelineStage.Authenticate);
            app.UseOAuthAuthorizationServer(options); //we tell the app user our configuration

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);


        }
    }
}
