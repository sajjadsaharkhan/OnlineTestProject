using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineTest.Base;
using OnlineTest.Base.Extensions;
using OnlineTest.Base.Infra;
using OnlineTest.Bootstrapper.Jwt.Infra;
using Project.Bootstrapper.Jwt;
using Project.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTest.Bootstrapper
{
    public static class AddJwtAuthenticationService
    {
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddTransient<IJwtHandel, JwtHandel>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ValidateIssuer = true,
                        ValidIssuer = "3Gaam",

                        ValidateAudience = true,
                        ValidAudience = "3Gaam",

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3G@@mJWTSecret1234567")),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = async (context) =>
                        {
                            await authenticationErrorAsync(context.Exception?.Message, context.HttpContext);
                        },
                        OnTokenValidated = async (context) =>
                        {
                            try
                            {
                                var dbContext = context.HttpContext.RequestServices.GetRequiredService<OnlineTestContext>();

                                //List of claim types that must be in the token
                                var claimsList = new List<string>() { ClaimTypes.Name, ClaimTypes.Surname, ClaimTypes.SerialNumber, ClaimTypes.NameIdentifier };

                                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                                //Check for the presence of claim types in the claimIdentity
                                foreach (var claim in claimsList)
                                    if (claimsIdentity.Claims?.Any(x => x.Type == claim) != true)
                                        await authenticationErrorAsync(MessagesDictionary.InvalidTokenClaims, context.HttpContext);

                                //Get token stamp and check it's empty or no!
                                var stampCode = claimsIdentity?.FindFirst(ClaimTypes.SerialNumber)?.Value;
                                if (stampCode.IsNullOrEmpty())
                                    await authenticationErrorAsync(MessagesDictionary.EmptyStamCode, context.HttpContext);
                                //Get username from token and get main user from database
                                var username = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                                var user = await dbContext.Users.AsNoTracking()
                                                                .Where(x => x.Username == username)
                                                                .Select(x => new
                                                                {
                                                                    x.Id,
                                                                    x.Stamp,
                                                                }).FirstOrDefaultAsync();
                                //Check if user is deleted or is correct?
                                if (user == null)
                                    await authenticationErrorAsync(MessagesDictionary.UserNotFoundErrorMessage, context.HttpContext);

                                //Check last user stamp with token stamp! if their not equal user must login again!
                                if (user.Stamp.ToString() != stampCode)
                                    await authenticationErrorAsync(MessagesDictionary.InvalidStampCode, context.HttpContext);

                                var userIdentity = context.HttpContext.RequestServices.GetRequiredService<IUserIdentity>();

                                //Set userIdentity class properties!
                                userIdentity.IsLogin = true;
                                userIdentity.UserId = user.Id;
                            }
                            catch
                            {
                                await authenticationErrorAsync(MessagesDictionary.LoginIsRequired, context.HttpContext);
                            }
                        }
                    };
                });
        }
        private static async Task authenticationErrorAsync(string message, HttpContext context)
        {
            context.Response.StatusCode = (int)(HttpStatusCode.Unauthorized);
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(new ApiResult(false, message).Serialize());
        }
    }
}
