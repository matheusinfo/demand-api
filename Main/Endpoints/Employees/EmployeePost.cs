using IWantApp.Main.Endpoints.Employees.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Main.Endpoints.Employees;

public class EmployeePost {
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static  Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action([FromBody] EmployeeRequest employeeRequest, HttpContext http, UserManager<IdentityUser> userManager) {
        var userId = http.User.Claims.First(claims => claims.Type == ClaimTypes.NameIdentifier).Value;
        var user = new IdentityUser {
            UserName = employeeRequest.Email,
            Email = employeeRequest.Email,
        };
        var response = userManager.CreateAsync(user, employeeRequest.Password).Result;

        if (!response.Succeeded) {
            return Results.ValidationProblem(response.Errors.ConvertToProblemDetails());
        }

        var userClaims = new List<Claim>{
               new Claim("EmployeeCode", employeeRequest.EmployeeCode),
               new Claim("Name", employeeRequest.Name),
               new Claim("CreatedBy", userId)
        };

        var claimsResult = userManager.AddClaimsAsync(user, userClaims).Result;

        if (!claimsResult.Succeeded) {
            return Results.BadRequest(claimsResult.Errors.First());
        }

        return Results.Created($"/employee/{user.Id}", user.Id);
    }
}
