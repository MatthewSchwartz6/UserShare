using System;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
public class CustomToken 
{

    public CustomToken()
    {

    }
    public static string getToken(string profileName){

        var claims = new[]
        {
            new Claim(ClaimTypes.Name,profileName)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom security key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "usershare-bd442.firebase.com",
            audience:"usershare-bd442.firebase.com",
            claims: claims,
            expires: DateTime.Now.AddHours(168),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}