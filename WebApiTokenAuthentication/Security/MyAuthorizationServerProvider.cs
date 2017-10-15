using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;

namespace WebApiTokenAuthentication.Security
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //This method is used to validate clients in the application
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //client has been validated
        }

        //In this method we will validate the credentials of the user. If credentials are valid we create the signed token
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            if (context.UserName == "admin" && context.Password == "admin") //static validation of credentials (should use database)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim("username", "admin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Gabriel"));
                context.Validated(identity);
            }
            else if(context.UserName == "user" && context.Password == "user")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("username", "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Peter"));
                context.Validated(identity);
            }
            else
            {
                context.SetError("Invalid Grant", "Provided username and password incorrect");
                return;
            }

        }
    }
}