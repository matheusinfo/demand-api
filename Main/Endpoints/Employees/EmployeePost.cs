using IWantApp.Main.Endpoints.Employees.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Main.Endpoints.Employees;

public class EmployeePost {
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static  Delegate Handle => Action;

    public static IResult Action([FromBody] EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager) {
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
               new Claim("Name", employeeRequest.Name)
        };

        var claimsResult = userManager.AddClaimsAsync(user, userClaims).Result;

        if (!claimsResult.Succeeded) {
            return Results.BadRequest(claimsResult.Errors.First());
        }

        return Results.Created($"/employee/{user.Id}", user.Id);
    }
}
