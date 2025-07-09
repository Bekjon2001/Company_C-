using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;


namespace Company.Dost
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler
            (
               IOptionsMonitor<AuthenticationSchemeOptions> options,
               ILoggerFactory logger,
               UrlEncoder encoder,
               ISystemClock clock

            ): base(options,logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("No Authorization Header"));
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            var credentials = Encoding.UTF8
                .GetString(Convert.FromBase64String(authHeader.Parameter ?? ""))
            .Split(':');

            var username = credentials[0];
            var password = credentials[1];

            if (username == "admin" && password == "1234")
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        }


    }

    public class PasswordHashHandlar
    {
        private static int _iterationCount = 100000;
        private static RandomNumberGenerator _randomNumberGenerator =  RandomNumberGenerator.Create();

        public static string HashPaswword(string paswword)
        {
            int saltSiz = 128 / 8;
            var salt = new byte[saltSiz];
            _randomNumberGenerator.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(paswword, salt, KeyDerivationPrf.HMACSHA512, _iterationCount, 256/8);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01;
            WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
            WriteNetworkByteOrder(outputBytes, 5, (uint)_iterationCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSiz);
            Buffer.BlockCopy(salt, 0, outputBytes, 13,salt.Length);
            Buffer.BlockCopy(subkey,0, outputBytes, 13+ saltSiz, subkey.Length);

            return Convert.ToBase64String(outputBytes);

        }

        public static bool VerifyPaswword(string password,string hash)
        {
            try
            {
                var hashedPassword = Convert.FromBase64String(hash);
                var keyDerivationPrf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                var iterationCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);
                if (saltLength < 128 /8) return false;

                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

                int subkeyLength = hashedPassword.Length - 13 - saltLength;
                if (subkeyLength < 128 / 8) return false;

                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 13 + saltLength, expectedSubkey, 0, expectedSubkey.Length);

                byte[] actualSubkey = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: keyDerivationPrf,
                    iterationCount: iterationCount,
                    numBytesRequested: subkeyLength
                );

                return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
            }
            catch
            {
                return false;
            }
            
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }
        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                 | ((uint)(buffer[offset + 1]) << 16)
                 | ((uint)(buffer[offset + 2]) << 8)
                 | ((uint)(buffer[offset + 3]));
        }
    }
    
}
