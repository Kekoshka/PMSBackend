using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PMSBackend.Common
{
    public static class AuthOptions
    {
        public static readonly string Issuer = "PMSServer";
        private static readonly string Key = "1234567890qwertyuiopasdfghjkl;zxcvbnm,./";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
