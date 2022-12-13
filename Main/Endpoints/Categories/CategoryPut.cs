using System.Security.Claims;
using IWantApp.Infra.Db.SqlServer.Data;
using IWantApp.Main.Endpoints.Categories.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Main.Endpoints.Categories;

public class CategoryPut {
    public static string Template => "/category{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static  Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id, [FromBody] CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context) {
        var userId = http.User.Claims.First(claims => claims.Type == ClaimTypes.NameIdentifier).Value;
        var category = context.Categories.Where(category => category.Id == id).FirstOrDefault();

        if(category == null) {
            return Results.NotFound();
        }

        category.EditInfo(categoryRequest.Name, categoryRequest.Active, userId);

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
