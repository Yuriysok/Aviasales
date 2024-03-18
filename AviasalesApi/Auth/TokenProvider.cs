using Microsoft.AspNetCore.Identity;

namespace AviasalesApi.Auth
{
    public class TokenProvider : TokenProviderDescriptor
    {
        public TokenProvider(Type type) : base(type)
        {

        }
    }
}
