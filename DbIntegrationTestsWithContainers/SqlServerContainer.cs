using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Polly;
using TestContainers.Core.Containers;

namespace DbIntegrationTestsWithContainers
{
    public sealed class SqlServerContainer : DatabaseContainer
    {
        private string _databaseName = "master";
        private string _userName = "sa";
        private string _password = "Password123";

        public override string DatabaseName => base.DatabaseName ?? _databaseName;

        public override string UserName => base.UserName ?? _userName;

        public override string Password => base.Password ?? _password;

        public override string ConnectionString => $"Data Source={GetDockerHostIpAddress()};Initial Catalog={DatabaseName};User ID={UserName};Password={Password}";

        protected override string TestQueryString => "SELECT 1";

        protected override async Task WaitUntilContainerStarted()
        {
            await base.WaitUntilContainerStarted();

            var connection = new SqlConnection(ConnectionString);

            var result = await Policy
                .TimeoutAsync(TimeSpan.FromMinutes(2))
                .WrapAsync(Policy
                    .Handle<SqlException>()
                    .WaitAndRetryForeverAsync(
                        iteration => TimeSpan.FromSeconds(10)))
                .ExecuteAndCaptureAsync(async () =>
                {
                    await connection.OpenAsync();

                    var cmd = new SqlCommand(TestQueryString, connection);
                    var reader = (await cmd.ExecuteScalarAsync());
                });

            if (result.Outcome == OutcomeType.Failure)
            {
                connection.Dispose();
                throw new Exception(result.FinalException.Message);
            }
        }
    }
}