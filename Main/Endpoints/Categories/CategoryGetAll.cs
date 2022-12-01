using IWantApp.Domain.Product;
using IWantApp.Infra.Db.SqlServer.Data;
using IWantApp.Main.Endpoints.Categories.Dto;

namespace IWantApp.Main.Endpoints.Categories;

public class CategoryGetAll {
    public static string Template => "/category";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static  Delegate Handle => Action;

    public static IResult Action(ApplicationDbContext context) {
        var categories = context.Categories.ToList();
        var response = categories.Select(category => new CategoryResponse(category.Id, category.Name, category.Active));
        return Results.Ok(response);
    }
}
