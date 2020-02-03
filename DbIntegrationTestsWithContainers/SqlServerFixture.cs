namespace DbIntegrationTestsWithContainers
{
    using System.Threading.Tasks;
    using TestContainers.Core.Builders;
    using Xunit;

    public class SqlServerFixture : IAsyncLifetime
    {
        private const string Password = "Passw0rd";

        private readonly SqlServerContainer container;

        public SqlServerFixture()
        {
            container = new DatabaseContainerBuilder<SqlServerContainer>()
                .Begin()
                .WithPassword(Password)
                .WithImage("mcr.microsoft.com/mssql/server:2017-latest")
                .WithEnv(("SA_PASSWORD", Password), ("ACCEPT_EULA", "Y"))
                .WithPortBindings((1433, 1433))
                .Build();
        }

        public string ConnectionString => container.ConnectionString;

        public virtual Task InitializeAsync() => container.Start();

        public virtual Task DisposeAsync() => container.Stop();
    }
}