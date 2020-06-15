/*============================================================================
   Namespace        : WebAPI
   Class            : TokenGenerator
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : generate token using jwt and validate
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace WebAPI
{
    using BusinessModule;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    public static class TokenGenerator
    {
        private const int skeycomlength = 20;
        
        public static string GenerateToken(UserInfoEntity userInfo, string Issure, int ExpiredInMinute)
        {
            byte[] key = GetSecretKey();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime dtExpired = DateTime.Now.AddMinutes(ExpiredInMinute);
            //Create Security Token Descriptor object by giving required parameters    
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetClaim(userInfo, dtExpired)),
                Expires = dtExpired,
                SigningCredentials = credentials,
                Issuer = Issure,
                Audience = Issure
            };

            //Create Security Token object by Security Token Descriptor object    
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return token;
        }

        public static byte[] GetSecretKey()
        {
            string secretKey = string.Empty;
            secretKey = ConfigurationManager.AppSettings["SecretKey"].ToString();
            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < skeycomlength)
                secretKey = new Guid().ToString();
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            return key;
        }

        public static List<Claim> GetClaim(UserInfoEntity userInfo, DateTime dtExpired)
        {
            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim(ClaimTypes.Version, "1"));
            permClaims.Add(new Claim(ClaimTypes.Expired, dtExpired.ToString()));
            permClaims.Add(new Claim(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()));
            permClaims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));
            permClaims.Add(new Claim(ClaimTypes.Role, userInfo.Role));

            return permClaims;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);

                if (jwtSecurityToken == null)
                    return null;
                byte[] key = GetSecretKey();
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = securityKey

                };

                SecurityToken securityToken;
                ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }

        public static string validateToken(string token)
        {
            string claimuser = null;
            try
            {
                ClaimsPrincipal claimsPrincipal = GetPrincipal(token);

                if (claimsPrincipal == null)
                    return null;

                IEnumerable<ClaimsIdentity> claimsIdentity = claimsPrincipal.Identities;

                if (claimsIdentity == null)
                    return null;
                Claim claimuserName = claimsIdentity.FirstOrDefault().FindFirst(ClaimTypes.Name);
                //Claim claimuserRole = claimsIdentity.FirstOrDefault().FindFirst(ClaimTypes.Role);
                //Claim claimuserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                claimuser = claimuserName.Value;
            }
            catch (Exception ex)
            {
                return null;
            }
            return claimuser;
        }
    }
}