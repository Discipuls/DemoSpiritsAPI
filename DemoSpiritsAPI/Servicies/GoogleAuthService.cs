using DemoSpiritsAPI.Servicies.Interfaces;
using Google.Apis.Auth;

namespace DemoSpiritsAPI.Servicies
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public async Task<bool> ValidateAdminPermission(HttpRequest Request)
        {
            string BearerToken;
            try
            {
                BearerToken = Request.Headers["Authorization"].ToString().Remove(0, 7);
            }
            catch
            {
                return false;
                throw new Exception("Unauthorized request");
            }

            bool isAdmin = await CheckIsAdmin(BearerToken);
            if (!isAdmin)
            {
                return false;
                throw new Exception("Access violation");
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckIsAdmin(string token)
        {
            GoogleJsonWebSignature.Payload payload;
            payload = await ValidateToken(token);

            List<string> admins_emails = new List<string>{"ivan.hontarau@gmail.com", "20sonashek20@gmail.com", "stasyaxaritonova@gmail.com" }; //TODO move to database
            return admins_emails.Contains(payload.Email);
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateToken(string token)
        {

            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);
            if (payload == null)
            {
                throw new Exception("Token invalid!");
            }
            return payload;
        }
    }
}
