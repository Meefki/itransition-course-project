using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IdentityServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly string connectionString;

        public UserProfileController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Authorization")!;
        }

        [HttpGet]
        [Route("names")]
        public async Task<dynamic> GetUserNames([FromQuery] List<string> ids)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var timeout = TimeSpan.FromSeconds(10);

            if (ids.Any())
            {
                object param = new { };
                string sql =
    @$"select u.Id as id, u.UserName as userName
    from [dbo].[users] as u
    where u.id in ({string.Join(',', ids.Select(x => $"'{x}'"))})";
                IEnumerable<dynamic> result = await connection.QueryAsync<dynamic>(sql, param, commandTimeout: timeout.Seconds);
                return result;
            }

            return Array.Empty<dynamic>();
        }

        [HttpGet]
        public async Task<dynamic> GetUserProfile(string id)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var timeout = TimeSpan.FromSeconds(10);

            if (!string.IsNullOrEmpty(id))
            {
                object param = new { };
                string sql =
    @$"select
       u.Id                   as id
      ,u.UserName             as userName
      ,u.Email                as email
      ,u.[NormalizedUserName] as [name]
      ,r.[Name]               as [role]
    from [dbo].[users]          as u
    left join [dbo].[UserRoles] as ur on ur.UserId = u.Id
    left join [dbo].[Roles]     as r  on r.Id = ur.RoleId
    where u.id in ('{id}')";
                dynamic result = await connection.QueryAsync<dynamic>(sql, param, commandTimeout: timeout.Seconds);
                return result is null ? new { } : (result[0] ?? new { });
            }

            return new { };
        }
    }
}
