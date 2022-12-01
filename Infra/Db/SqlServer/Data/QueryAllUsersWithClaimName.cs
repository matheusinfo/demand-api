using Dapper;
using IWantApp.Main.Endpoints.Employees.Dto;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Db.SqlServer.Data;

public class QueryAllUsersWithClaimName {
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration) {
        this.configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows) {
        var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);
        var query =
            @"select Email, ClaimValue as Name
            from AspNetUsers user inner
            join AspNetUserClaims claim
            on user.id = claim.UserId and claimtype = 'Name'
            order by name
            OFFSET (@page - 1) * @rows ROWS
            FETCH NEXT @rows ROWS ONLY";

        return db.Query<EmployeeResponse>(
            query,
            new { page, rows }
        );
    }
}
