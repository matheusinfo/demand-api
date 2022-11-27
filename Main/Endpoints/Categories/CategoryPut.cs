using IWantApp.Domain.Product;
using IWantApp.Infra.Db.SqlServer.Data;
using IWantApp.Main.Endpoints.Categories.Dto;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Main.Endpoints.Categories;

public class CategoryPut {
    public static string Template => "/category{id}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static  Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, [FromBody] CategoryRequest categoryRequest, ApplicationDbContext context) {
        var category = context.Categories.Where(category => category.Id == id).FirstOrDefault();
        category.Name = categoryRequest.Name;
        category.Active = categoryRequest.Active;
        context.SaveChanges();

        return Results.Ok();
    }
}
