using Dapper;

namespace Reviewing.API.Extentions
{
    public static class DapperSetup
    {
        public static void Init()
        {
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
        }
    }
}