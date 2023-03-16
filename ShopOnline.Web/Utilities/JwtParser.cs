using System.Security.Claims;
using System.Text.Json;

namespace ShopOnline.Web.Utilities
{
    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            //take payload info = second after .
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            ExtractNameFromJWT(claims, keyValuePairs);
            ExtractUserIdFromJWT(claims, keyValuePairs);
            claims.AddRange(keyValuePairs.Select(kv => new Claim(kv.Key, kv.Value.ToString())));

            return claims;
        }
        private static void ExtractNameFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue("name", out var name);
            if (name is not null)
            {
                var parsedName = name.ToString().Trim();
                claims.Add(new Claim(ClaimTypes.Name, parsedName));
            }
            keyValuePairs.Remove("name");
        }
        private static void ExtractUserIdFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue("userId", out var userID);
            if (userID is not null)
            {
                var parsedUserId = userID.ToString().Trim();
                claims.Add(new Claim("userId", parsedUserId));
            }
            keyValuePairs.Remove("userId");
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch(base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
                
            }
            return Convert.FromBase64String(base64);
        }
    }
}
