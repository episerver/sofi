using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

//https://www.jerriepelser.com/blog/authenticate-oauth-aspnet-core-2/

// Add services to the container.
builder.Services.AddControllersWithViews();




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GitHub";
})
       .AddCookie()
       
       .AddOAuth("GitHub", options =>
       {
           options.ClientId = "postman-client";
           options.ClientSecret = "postman";
           options.CallbackPath = new PathString("/signin-github");

           options.AuthorizationEndpoint = "https://localhost:44397/api/episerver/connect/authorize";
           options.TokenEndpoint = "https://localhost:44397/api/episerver/connect/token";
           options.UserInformationEndpoint = "https://localhost:44397/api/episerver/user_info";
           options.SaveTokens = true;
   
           options.Events = new OAuthEvents
           {
               OnCreatingTicket = async context =>
               {
                   //var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                   //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                   //var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                  // response.EnsureSuccessStatusCode();

                   //var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                   context.RunClaimActions();
               }
           };
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
