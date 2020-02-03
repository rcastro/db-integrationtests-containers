namespace DbIntegrationTestsWithContainers
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Dapper;
    using Xunit;

    public class MovieTests : IClassFixture<MovieFixture>
    {
        private readonly MovieFixture fixture;

        public MovieTests(MovieFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task CountQuery_ShouldRunSuccessfully()
        {
            using (var conn = new SqlConnection(fixture.ConnectionString))
            {
                conn.Open();
                var total = await conn.ExecuteScalarAsync(@"SELECT COUNT(1) from Movie");
                Assert.Equal(9, total);
            }
        }

        [Theory]
        [InlineData("George Lucas", 4)]
        [InlineData("J.J. Abrams", 2)]
        [InlineData("Irvin Kershner", 1)]
        public async Task CountMoviesByDirector_ShouldRunSuccessfully(string director, int expectedTotal)
        {
            using (var conn = new SqlConnection(fixture.ConnectionString))
            {
                conn.Open();
                var total = await conn.ExecuteScalarAsync(@"SELECT COUNT(1) FROM Movie WHERE Director = @Director", new { Director = director });
                Assert.Equal(expectedTotal, total);
            }
        }
    }
}
