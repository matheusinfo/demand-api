namespace IWantApp.Main.Endpoints.Employees;

public class EmployeePost {
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static  Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromBody] EmployeeRequest employeeRequest, HttpContext http, UserManager<IdentityUser> userManager) {
        var userId = http.User.Claims.First(claims => claims.Type == ClaimTypes.NameIdentifier).Value;
        var user = new IdentityUser {
            UserName = employeeRequest.Email,
            Email = employeeRequest.Email,
        };
        var response = await userManager.CreateAsync(user, employeeRequest.Password);

        if (!response.Succeeded) {
            return Results.ValidationProblem(response.Errors.ConvertToProblemDetails());
        }

        var userClaims = new List<Claim>{
               new Claim("EmployeeCode", employeeRequest.EmployeeCode),
               new Claim("Name", employeeRequest.Name),
               new Claim("CreatedBy", userId)
        };

        var claimsResult = await userManager.AddClaimsAsync(user, userClaims);

        if (!claimsResult.Succeeded) {
            return Results.BadRequest(claimsResult.Errors.First());
        }

        return Results.Created($"/employee/{user.Id}", user.Id);
    }
}
