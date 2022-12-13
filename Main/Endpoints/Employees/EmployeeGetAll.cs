using IWantApp.Infra.Db.SqlServer.Data;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Main.Endpoints.Employees;

public class EmployeeGetAll {
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static  Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(QueryAllUsersWithClaimName query, int page = 1, int rows = 10) {
        return Results.Ok(query.Execute(page, rows));
    }
}
