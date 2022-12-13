using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWantApp.Main.Endpoints.Security;

public class TokenPost{
    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(LoginRequest loginRequest, IConfiguration configuration, UserManager<IdentityUser> userManager){
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;

        if(!userManager.checkPassawordAsync(user, loginRequest.Password).Result || user == null){
            return Results.BadRequest();
        }

        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Email, loginRequest.Email)
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
        };

        var tokenHandler = new JwtSecutiryTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
         
        return Results.Ok(new { 
            token = tokenHandler.WriteToken(token) 
        });
    }
}
