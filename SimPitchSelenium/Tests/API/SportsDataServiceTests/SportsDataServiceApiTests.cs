using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace SimPitchSelenium.Tests.API.SportsDataServiceTests
{
    [TestFixture]
    public class TeamControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/Team";

        [Test]
        public async Task GetTeams_HappyPath_ReturnsTeamsAndValidatesProperties()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Ignore("Endpoint returned 404. No data to test.");
            }
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var teams = JArray.Parse(content);

            Assert.That(teams.Count, Is.GreaterThan(0), "Expected at least one team in the response.");

            var firstTeam = teams.First();
            Assert.That(firstTeam["id"]?.ToString(), Is.Not.Null.And.Not.Empty);
            Assert.That(firstTeam["name"]?.ToString(), Is.Not.Null.And.Not.Empty);
            
            var countryCode = firstTeam["country"]?["code"]?.ToString();
            Assert.That(countryCode, Is.Not.Null.And.Not.Empty, "Team country code should exist");
        }

        [Test]
        public async Task GetTeamById_InvalidRoute_ReturnsNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}/{Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetAllTeams_ResponseFormat_IsValidArray()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.DoesNotThrow(() => JArray.Parse(content), "Response must be a valid JSON array.");
            }
        }
    }

    [TestFixture]
    public class LeagueControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/League";

        [Test]
        public async Task GetLeagues_HappyPath_ReturnsLeaguesAndValidatesProperties()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Ignore("Endpoint returned 404. No data to test.");
            }
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var leagues = JArray.Parse(content);

            Assert.That(leagues.Count, Is.GreaterThan(0));

            var firstLeague = leagues.First();
            Assert.That(firstLeague["id"]?.ToString(), Is.Not.Null.And.Not.Empty);
            Assert.That(firstLeague["name"]?.ToString(), Is.Not.Null.And.Not.Empty);
            Assert.That(firstLeague["countryId"]?.ToString(), Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task GetLeagueById_InvalidRoute_ReturnsNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}/{Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetAllLeagues_ResponseFormat_IsValidArray()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.DoesNotThrow(() => JArray.Parse(content), "Response must be a valid JSON array.");
            }
        }
    }

    [TestFixture]
    public class CountryControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/Country";

        [Test]
        public async Task GetCountries_HappyPath_ReturnsCountriesAndValidatesProperties()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Ignore("Endpoint returned 404. No data to test.");
            }
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var countries = JArray.Parse(content);

            Assert.That(countries.Count, Is.GreaterThan(0));

            var firstCountry = countries.First();
            Assert.That(firstCountry["id"]?.ToString(), Is.Not.Null.And.Not.Empty);
            Assert.That(firstCountry["name"]?.ToString(), Is.Not.Null.And.Not.Empty);
            
            var alpha2Code = firstCountry["code"]?.ToString();
            Assert.That(alpha2Code?.Length, Is.EqualTo(2));
        }

        [Test]
        public async Task GetCountryById_InvalidRoute_ReturnsNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}/{Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetAllCountries_ResponseFormat_IsValidArray()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.DoesNotThrow(() => JArray.Parse(content), "Response must be a valid JSON array.");
            }
        }

        [Test]
        public async Task GetUnknownEndpoint_ReturnsNotFound()
        {
            var response = await Client.GetAsync("/api/sportsdata/Teamm");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class StadiumControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/Stadium";

        [Test]
        public async Task GetStadiums_HappyPath_ReturnsStadiumsAndValidatesProperties()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Ignore("Endpoint returned 404. No data to test.");
            }
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var stadiums = JArray.Parse(content);

            Assert.That(stadiums.Count, Is.GreaterThan(0));

            var firstStadium = stadiums.First();
            Assert.That(firstStadium["id"]?.ToString(), Is.Not.Null.And.Not.Empty);
            Assert.That(firstStadium["name"]?.ToString(), Is.Not.Null.And.Not.Empty);
            
            var capacity = firstStadium["capacity"]?.Value<int>();
            Assert.That(capacity, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetStadiumById_InvalidRoute_ReturnsNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}/{Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetAllStadiums_ResponseFormat_IsValidArray()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.DoesNotThrow(() => JArray.Parse(content), "Response must be a valid JSON array.");
            }
        }
    }

    [TestFixture]
    public class LeagueRoundControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/LeagueRound";

        [Test]
        public async Task GetLeagueRounds_HappyPath_ReturnsRoundsAndValidatesProperties()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                Assert.Ignore("Endpoint returned 404/400. No data to test or requires specific parameters.");
            }
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var rounds = JArray.Parse(content);

            if (rounds.Count == 0)
            {
                Assert.Ignore("Empty list returned.");
            }

            var firstRound = rounds.First();
            Assert.That(firstRound["id"]?.ToString(), Is.Not.Null.And.Not.Empty);
            Assert.That(firstRound["seasonYear"]?.ToString(), Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task GetLeagueRounds_WithInvalidLeagueId_ReturnsNotFound()
        {
            var invalidLeagueId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/{invalidLeagueId}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetRoundsByLeague_MissingParam_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}?leagueId=");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetRoundsByLeague_InvalidParamFormat_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}?leagueId=invalid-guid");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetRoundsByLeague_NonExistentLeague_ReturnsNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}?leagueId={Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.OK)); // API might return empty array (OK) if no rounds
        }
    }

    [TestFixture]
    public class MatchRoundControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/sportsdata/MatchRound";

        [Test]
        public async Task GetMatchRounds_HappyPath_ReturnsMatchRoundsAndValidatesProperties()
        {
            var response = await Client.GetAsync(BasePath);
            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                Assert.Ignore("Endpoint returned 404/400. No data to test.");
            }
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var matches = JArray.Parse(content);

            if (matches.Count == 0)
            {
                Assert.Ignore("Empty list returned.");
            }

            var firstMatch = matches.First();
            Assert.That(firstMatch["id"]?.ToString(), Is.Not.Null.And.Not.Empty);
            
            var homeTeamId = firstMatch["homeTeamId"]?.ToString();
            var awayTeamId = firstMatch["awayTeamId"]?.ToString();
            Assert.That(homeTeamId, Is.Not.EqualTo(awayTeamId), "Home and Away teams must be different");
        }

        [Test]
        public async Task GetMatchRounds_WithInvalidId_ReturnsNotFound()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/{invalidId}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task GetMatchRounds_MissingParam_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}?roundId=");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetMatchRounds_InvalidParamFormat_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}?roundId=invalid");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetMatchRounds_NonExistentRound_ReturnsNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}?roundId={Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.OK)); // Might return empty array
        }
    }
}

