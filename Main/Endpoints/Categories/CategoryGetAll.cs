namespace IWantApp.Main.Endpoints.Categories;

public class CategoryGetAll {
    public static string Template => "/category";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static  Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(ApplicationDbContext context) {
        var categories = context.Categories.ToList();
        var response = categories.Select(category => new CategoryResponse(category.Id, category.Name, category.Active));
        return Results.Ok(response);
    }
}
