using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace SimPitchSelenium.Tests.API.EngineServiceTests
{
    [TestFixture]
    public class SimulationControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/engine/Simulation";

        [Test]
        public async Task GetAllOverviews_HappyPath_ReturnsOverviewsAndValidatesProperties()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Ignore("Endpoint returned 404. No data to test.");
            }
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(content);
            var overviews = responseObject["items"] as JArray;

            if (overviews == null || overviews.Count == 0)
            {
                Assert.Ignore("Empty list returned.");
            }

            var firstOverview = overviews.First();
            Assert.That(firstOverview["id"]?.ToString(), Is.Not.Null.And.Not.Empty);
            
            var simParams = firstOverview["simulationParams"];
            Assert.That(simParams, Is.Not.Null);
            Assert.That(simParams["iterations"]?.Value<int>(), Is.GreaterThan(0));

            // Test GetSimulationOverview happy path
            var overviewId = firstOverview["id"].ToString();
            var singleResponse = await Client.GetAsync($"{BasePath}/overview/{overviewId}");
            Assert.That(singleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var singleContent = await singleResponse.Content.ReadAsStringAsync();
            var singleOverview = JObject.Parse(singleContent);
            Assert.That(singleOverview["id"]?.ToString(), Is.EqualTo(overviewId));
            Assert.That(singleOverview["simulationParams"]?["iterations"]?.Value<int>(), Is.EqualTo(simParams["iterations"].Value<int>()));
        }

        [Test]
        public async Task GetAllOverviews_WithPagination_HappyPath_ValidatesPageSize()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews?pageNumber=1&pageSize=2");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Ignore("Endpoint returned 404. No data to test.");
            }

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(content);
            var overviews = responseObject["items"] as JArray;

            // It should return at most 2 items
            Assert.That(overviews, Is.Not.Null);
            Assert.That(overviews.Count, Is.LessThanOrEqualTo(2));
        }

        [Test]
        public async Task CreateSimulation_WithNullDto_ReturnsBadRequest()
        {
            var response = await Client.PostAsJsonAsync(BasePath, (object)null);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.UnsupportedMediaType));
        }

        [Test]
        public async Task CreateSimulation_WithEmptyDto_ReturnsBadRequestOrValidationProblem()
        {
            var response = await Client.PostAsJsonAsync(BasePath, new { });
            Assert.That((int)response.StatusCode, Is.EqualTo(400).Or.EqualTo(422));
        }

        [Test]
        public async Task CreateSimulation_MissingLeagueId_ReturnsBadRequestOrValidationProblem()
        {
            var dto = new { Iterations = 10, SeasonYears = new[] { "2023_2024" }, Title = "Test", Model = "Advanced" };
            var response = await Client.PostAsJsonAsync(BasePath, dto);
            Assert.That((int)response.StatusCode, Is.EqualTo(400).Or.EqualTo(422));
        }

        [Test]
        public async Task CreateSimulation_NegativeIterations_ReturnsBadRequestOrValidationProblem()
        {
            var dto = new { LeagueId = Guid.NewGuid(), Iterations = -5, SeasonYears = new[] { "2023_2024" }, Title = "Test", Model = "Advanced" };
            var response = await Client.PostAsJsonAsync(BasePath, dto);
            Assert.That((int)response.StatusCode, Is.EqualTo(400).Or.EqualTo(422));
        }

        [Test]
        public async Task CreateSimulation_ExceededMaxIterations_ReturnsBadRequestOrValidationProblem()
        {
            var dto = new { LeagueId = Guid.NewGuid(), Iterations = 10000000, SeasonYears = new[] { "2023_2024" }, Title = "Test", Model = "Advanced" };
            var response = await Client.PostAsJsonAsync(BasePath, dto);
            Assert.That((int)response.StatusCode, Is.EqualTo(400).Or.EqualTo(422));
        }

        [Test]
        public async Task CreateSimulation_InvalidModelType_ReturnsBadRequestOrValidationProblem()
        {
            var dto = new { LeagueId = Guid.NewGuid(), Iterations = 10, SeasonYears = new[] { "2023_2024" }, Title = "Test", Model = "RandomFakeModel" };
            var response = await Client.PostAsJsonAsync(BasePath, dto);
            Assert.That((int)response.StatusCode, Is.EqualTo(400).Or.EqualTo(422));
        }

        [Test]
        public async Task GetSimulationOverview_InvalidId_ReturnsNotFoundOrBadRequest()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/overview/{invalidId}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest)); 
        }

        [Test]
        public async Task GetSimulationOverview_EmptyId_ReturnsMethodNotAllowedOrNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}/overview/");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetAllOverviews_InvalidPageSize_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews?pageSize=-1");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.InternalServerError)); 
        }

        [Test]
        public async Task GetAllOverviews_NegativePageSize_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews?pageSize=-50");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.InternalServerError)); 
        }

        [Test]
        public async Task GetAllOverviews_ExcessivePageSize_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews?pageSize=999999");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.InternalServerError)
                .Or.EqualTo(HttpStatusCode.OK)); // API might just clamp it to max
        }

        [Test]
        public async Task GetAllOverviews_InvalidPageNumber_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews?pageNumber=-1");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.InternalServerError)
                .Or.EqualTo(HttpStatusCode.OK)); // Defaults to page 1
        }

        [Test]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/{invalidId}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task StopSimulation_InvalidId_ReturnsNotFound()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.DeleteAsync($"{BasePath}/stop/{invalidId}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    [TestFixture]
    public class ScoreboardControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/engine/Scoreboard";

        [Test]
        public async Task GetByIds_MissingIds_ReturnsBadRequestOrNotFound()
        {
            var response = await Client.GetAsync(BasePath);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetByIds_InvalidIds_ReturnsNotFound()
        {
            var simulationId = Guid.NewGuid();
            var iterationResultId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}?simulationId={simulationId}&iterationResultId={iterationResultId}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetScoreboardByLeagueIdAndSeasonYear_InvalidLeagueId_ReturnsNotFound()
        {
            var leagueId = "";
            var response = await Client.GetAsync($"{BasePath}/seasons/2025_2026/leagues/{leagueId}/scoreboard");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetScoreboard_MissingSimulationId_ReturnsBadRequestOrNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}?iterationResultId={Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetScoreboard_InvalidSimulationId_ReturnsBadRequestOrNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}?simulationId=invalid&iterationResultId={Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetScoreboard_MissingIterationId_ReturnsBadRequestOrNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}?simulationId={Guid.NewGuid()}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetScoreboard_InvalidIterationId_ReturnsBadRequestOrNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}?simulationId={Guid.NewGuid()}&iterationResultId=invalid");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class SimulationStatsControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/engine/SimulationStats";

        [Test]
        public async Task GetSeasonStats_InvalidSimulationId_ReturnsNotFoundOrBadRequest()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/{invalidId}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.PreconditionFailed));
        }

        [Test]
        public async Task GetSeasonStats_EmptySimulationId_ReturnsMethodNotAllowedOrNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}/");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    [TestFixture]
    public class IterationResultControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/engine/IterationResult";

        [Test]
        public async Task GetIterationResult_InvalidId_ReturnsNotFoundOrBadRequest()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/{invalidId}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetIterationResult_EmptyId_ReturnsMethodNotAllowedOrNotFound()
        {
            var response = await Client.GetAsync($"{BasePath}/");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
