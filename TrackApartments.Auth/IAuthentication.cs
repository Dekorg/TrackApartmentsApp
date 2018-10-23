using System;
using System.Threading.Tasks;
using Refit;

namespace TrackApartments.Auth
{
    public interface IAuthentication
    {
        [Get("/.auth/me")]
        Task<AuthenticationModel[]> GetCurrentAuthentication();
    }
}
