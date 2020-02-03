namespace DbIntegrationTestsWithContainers
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    public class MovieFixture : SqlServerFixture
    {
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            PopulateDb();
        }

        private void PopulateDb()
        {
            var script = File.ReadAllText("movies.sql");
            using (var connection = new SqlConnection(ConnectionString))
            {
                var server = new Server(new ServerConnection(connection));
                try
                {
                    server.ConnectionContext.ExecuteNonQuery(script);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
