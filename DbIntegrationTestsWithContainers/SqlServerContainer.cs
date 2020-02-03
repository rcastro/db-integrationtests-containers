namespace DbIntegrationTestsWithContainers
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Polly;
    using TestContainers.Core.Containers;

    public sealed class SqlServerContainer : DatabaseContainer
    {
        private string databaseName = "master";
        private string userName = "sa";
        private string password = "Password123";

        public override string DatabaseName => base.DatabaseName ?? databaseName;

        public override string UserName => base.UserName ?? userName;

        public override string Password => base.Password ?? password;

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
                    await cmd.ExecuteScalarAsync();
                });

            if (result.Outcome == OutcomeType.Failure)
            {
                connection.Dispose();
                throw new Exception(result.FinalException.Message);
            }
        }
    }
}