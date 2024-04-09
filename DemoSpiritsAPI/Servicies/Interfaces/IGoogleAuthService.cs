using Google.Apis.Auth;

namespace DemoSpiritsAPI.Servicies.Interfaces
{
    public interface IGoogleAuthService
    {
        public Task<GoogleJsonWebSignature.Payload> ValidateToken(string BearerToken);
        public Task<bool> CheckIsAdmin(string BearerToken);
        public Task<bool> ValidateAdminPermission(HttpRequest Request);
    }
}
