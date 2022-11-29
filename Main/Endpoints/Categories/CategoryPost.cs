using IWantApp.Domain.Product;
using IWantApp.Infra.Db.SqlServer.Data;
using IWantApp.Main.Endpoints.Categories.Dto;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Main.Endpoints.Categories;

public class CategoryPost {
    public static string Template => "/category";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static  Delegate Handle => Action;

    public static IResult Action([FromBody] CategoryRequest categoryRequest, ApplicationDbContext context) {
        var category = new Category(categoryRequest.Name, "Test", "Test");

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/category/{category.Id}", category.Id);
    }
}
