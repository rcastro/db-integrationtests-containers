using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;
using Dapper;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System;

namespace DbIntegrationTestsWithContainers
{
    public class DbTests : IClassFixture<SqlServerFixture>
    {
        private readonly SqlServerFixture sqlServerFixture;

        public DbTests(SqlServerFixture fixture)
        {
            sqlServerFixture = fixture;
            CreateDb();
        }

        [Fact]
        public async Task SqlQuery_ShouldRunSuccessfully()
        {
            using (var conn = new SqlConnection(sqlServerFixture.ConnectionString)) {
                conn.Open();
                var total = await conn.ExecuteScalarAsync(@"SELECT COUNT(1) from Product");
                Assert.Equal(3, total);    
            }
        }

        private void CreateDb() {
            var script = File.ReadAllText("setup.sql");
            using (SqlConnection connection = new SqlConnection(sqlServerFixture.ConnectionString))
            {
                Server server = new Server(new ServerConnection(connection));
                try
                {
                    server.ConnectionContext.BeginTransaction();
                    server.ConnectionContext.ExecuteNonQuery(script);
                    server.ConnectionContext.CommitTransaction();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    server.ConnectionContext.RollBackTransaction();
                }
            }
        }
    }
}
