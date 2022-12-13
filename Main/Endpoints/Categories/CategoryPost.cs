﻿namespace IWantApp.Main.Endpoints.Categories;

public class CategoryPost {
    public static string Template => "/category";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static  Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromBody] CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context) {
        var userId = http.User.Claims.First(claims => claims.Type == ClaimTypes.NameIdentifier).Value;
        var category = new Category(categoryRequest.Name, userId, userId);

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return Results.Created($"/category/{category.Id}", category.Id);
    }
}
