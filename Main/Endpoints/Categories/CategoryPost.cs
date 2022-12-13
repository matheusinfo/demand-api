using System.Security.Claims;
using IWantApp.Domain.Product;
using IWantApp.Infra.Db.SqlServer.Data;
using IWantApp.Main.Endpoints.Categories.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Main.Endpoints.Categories;

public class CategoryPost {
    public static string Template => "/category";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static  Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action([FromBody] CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context) {
        var userId = http.User.Claims.First(claims => claims.Type == ClaimTypes.NameIdentifier).Value;
        var category = new Category(categoryRequest.Name, userId, userId);

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/category/{category.Id}", category.Id);
    }
}
