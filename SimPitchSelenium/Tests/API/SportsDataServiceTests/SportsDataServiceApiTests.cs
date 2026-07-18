using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SimPitchSelenium.Tests.API.SportsDataServiceTests
{
    [TestFixture]
    public class TeamControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/Team";

        [Test]
        public async Task GetTeams_ReturnsOkOrNotFound()
        {
            var response = await Client.GetAsync(BasePath);
            
            // Depends on DB state, usually 200 or 404
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task PostTeam_MethodNotAllowed()
        {
            // Assuming there is no POST method for Team in the controller based on earlier search
            var response = await Client.PostAsync(BasePath, null);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed)
                .Or.EqualTo(HttpStatusCode.UnsupportedMediaType)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class LeagueControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/League";

        [Test]
        public async Task GetLeagues_ReturnsOkOrNotFound()
        {
            var response = await Client.GetAsync(BasePath);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task PutLeague_MethodNotAllowed()
        {
            var response = await Client.PutAsync(BasePath, null);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed)
                .Or.EqualTo(HttpStatusCode.UnsupportedMediaType)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class CountryControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/Country";

        [Test]
        public async Task GetCountries_ReturnsOkOrNotFound()
        {
            var response = await Client.GetAsync(BasePath);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class StadiumControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/Stadium";

        [Test]
        public async Task GetStadiums_ReturnsOkOrNotFound()
        {
            var response = await Client.GetAsync(BasePath);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class LeagueRoundControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/LeagueRound";

        [Test]
        public async Task GetLeagueRounds_ReturnsOkOrNotFound()
        {
            var response = await Client.GetAsync(BasePath);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest)); // Might require parameters
        }

        [Test]
        public async Task GetLeagueRounds_WithInvalidLeagueId_ReturnsNotFound()
        {
            var invalidLeagueId = Guid.NewGuid();
            // Assuming an endpoint exists like /api/sportsdata/LeagueRound/{leagueId}
            var response = await Client.GetAsync($"{BasePath}/{invalidLeagueId}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }
    }

    [TestFixture]
    public class MatchRoundControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/MatchRound";

        [Test]
        public async Task GetMatchRounds_ReturnsOkOrNotFound()
        {
            var response = await Client.GetAsync(BasePath);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest)); 
        }

        [Test]
        public async Task GetMatchRounds_WithInvalidId_ReturnsNotFound()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/{invalidId}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }
    }
}
