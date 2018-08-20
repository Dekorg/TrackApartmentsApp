using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Refit;

namespace TrackApartments.Auth
{
    public static class AuthenticationExtensions
    {
        public static readonly HttpClientHandler CachedHandler = new HttpClientHandler();

        /// <summary>
        /// ClaimsPrincipal.Current is currently always null in Functions v2 on dotnet core. 
        /// using this to solve the problem as a temp solution, should be fixed bythe end of 2018
        /// https://github.com/Azure/azure-functions-host/issues/33
        /// </summary>
        public static bool TryAuthenticate(this HttpRequest item, out AuthenticationModel model)
        {
            bool isAuthenticated = false;
            model = null;

            try
            {
                var client = new HttpClient(CachedHandler)
                {
                    BaseAddress = new Uri(Environment.GetEnvironmentVariable("AuthenticationBaseAddress")
                                          ?? new Uri(item.GetDisplayUrl()).GetLeftPart(UriPartial.Authority))
                };

                CachedHandler.CookieContainer.Add(client.BaseAddress,
                    new Cookie("AppServiceAuthSession", item.Cookies["AppServiceAuthSession"]
                                                        ?? Environment.GetEnvironmentVariable("AuthenticationToken")));

                var service = RestService.For<IAuthentication>(client);

                model = service.GetCurrentAuthentication().Result.SingleOrDefault();
                isAuthenticated = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return isAuthenticated;
        }
    }
}
