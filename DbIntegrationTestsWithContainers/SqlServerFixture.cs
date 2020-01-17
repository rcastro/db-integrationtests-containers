using TestContainers.Core.Builders;
using System.Threading.Tasks;
using Xunit;

namespace DbIntegrationTestsWithContainers
{
    public class SqlServerFixture : IAsyncLifetime
    {
        private const string password = "Passw0rd";

        public string ConnectionString => _container.ConnectionString;

        private readonly SqlServerContainer _container;

        public SqlServerFixture()
        {
            _container = new DatabaseContainerBuilder<SqlServerContainer>()
                .Begin()
                .WithPassword(password)
                .WithImage("mcr.microsoft.com/mssql/server:2017-latest")
                .WithEnv(("SA_PASSWORD", password), ("ACCEPT_EULA", "Y"))
                .WithPortBindings((1433, 1433))
                .Build();
        }

        public Task InitializeAsync() => _container.Start();

        public Task DisposeAsync() => _container.Stop();
    }
}