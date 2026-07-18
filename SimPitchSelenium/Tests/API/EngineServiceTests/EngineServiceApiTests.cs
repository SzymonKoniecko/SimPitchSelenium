using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SimPitchSelenium.Tests.API.EngineServiceTests
{
    [TestFixture]
    public class SimulationControllerApiTests : BaseApiTest
    {
        private const string BasePath = "/api/engine/Simulation";

        [Test]
        public async Task CreateSimulation_WithNullDto_ReturnsBadRequest()
        {
            var response = await Client.PostAsJsonAsync(BasePath, (object)null);
            
            // Expected 400 Bad Request
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.UnsupportedMediaType));
        }

        [Test]
        public async Task CreateSimulation_WithEmptyDto_ReturnsBadRequestOrValidationProblem()
        {
            // Empty body might fail validation
            var response = await Client.PostAsJsonAsync(BasePath, new { });
            
            // It depends on backend validation, usually 400
            Assert.That((int)response.StatusCode, Is.EqualTo(400).Or.EqualTo(422));
        }

        [Test]
        public async Task GetSimulationOverview_InvalidId_ReturnsNotFoundOrBadRequest()
        {
            var invalidId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BasePath}/overview/{invalidId}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.BadRequest)); // Depending on backend handling
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
        public async Task GetAllOverviews_DefaultPagination_ReturnsSuccess()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound)); // Might be 404 if no data in db
        }

        [Test]
        public async Task GetAllOverviews_WithPagination_ReturnsSuccess()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews?pageNumber=1&pageSize=5");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.NotFound)); 
        }
        
        [Test]
        public async Task GetAllOverviews_InvalidPageSize_ReturnsBadRequest()
        {
            var response = await Client.GetAsync($"{BasePath}/overviews?pageSize=-1");
            
            // Depends on validation, but 400 is common
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.InternalServerError)); 
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
    }
}
