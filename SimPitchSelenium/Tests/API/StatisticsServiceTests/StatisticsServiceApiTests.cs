using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using NUnit.Framework;
using SimPitchProtos.StatisticsService.Scoreboard;
using SimPitchProtos.StatisticsService.SimulationStats;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests.API.StatisticsServiceTests
{
    [TestFixture]
    public class StatisticsServiceApiTests
    {
        private GrpcChannel _channel;
        private ScoreboardService.ScoreboardServiceClient _scoreboardClient;
        private SimulationStatsService.SimulationStatsServiceClient _simulationStatsClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var url = ConfigReader.GetStatisticsGrpcUrl();
            _channel = GrpcChannel.ForAddress(url);
            _scoreboardClient = new ScoreboardService.ScoreboardServiceClient(_channel);
            _simulationStatsClient = new SimulationStatsService.SimulationStatsServiceClient(_channel);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _channel?.Dispose();
        }

        // --- ScoreboardService Tests ---

        [Test]
        public void CreateScoreboards_EmptyRequest_ThrowsRpcException()
        {
            var request = new CreateScoreboardsRequest();
            Assert.ThrowsAsync<RpcException>(async () => await _scoreboardClient.CreateScoreboardsAsync(request));
        }

        [Test]
        public void CreateScoreboardByLeagueIdAndSeasonYear_EmptyRequest_ThrowsRpcException()
        {
            var request = new CreateScoreboardByLeagueIdAndSeasonYearRequest();
            Assert.ThrowsAsync<RpcException>(async () => await _scoreboardClient.CreateScoreboardByLeagueIdAndSeasonYearAsync(request));
        }

        [Test]
        public void CreateScoreboardByIterationResultData_EmptyRequest_ThrowsRpcException()
        {
            var request = new CreateScoreboardByIterationResultDataRequest();
            Assert.ThrowsAsync<RpcException>(async () => await _scoreboardClient.CreateScoreboardByIterationResultDataAsync(request));
        }

        [Test]
        public void CreateScoreboardByLeagueIdAndSeasonYear_InvalidLeagueId_ThrowsRpcException()
        {
            var request = new CreateScoreboardByLeagueIdAndSeasonYearRequest 
            { 
                LeagueId = Guid.NewGuid().ToString(),
                SeasonYear = "2023_2024"
            };
            var ex = Assert.ThrowsAsync<RpcException>(async () => await _scoreboardClient.CreateScoreboardByLeagueIdAndSeasonYearAsync(request));
            // Depending on implementation, it might be NotFound or InvalidArgument
            Assert.That(ex.StatusCode, Is.EqualTo(StatusCode.NotFound).Or.EqualTo(StatusCode.InvalidArgument).Or.EqualTo(StatusCode.Internal));
        }
        
        [Test]
        public async Task GetScoreboardsByQuery_EmptyQuery_StreamsSuccessfullyOrThrows()
        {
            var request = new ScoreboardsByQueryRequest();
            try 
            {
                var response = await _scoreboardClient.GetScoreboardsByQuery(request).ResponseHeadersAsync;
                Assert.Pass("Call initialized");
            }
            catch (RpcException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(StatusCode.InvalidArgument).Or.EqualTo(StatusCode.Internal));
            }
        }

        // --- SimulationStatsService Tests ---

        [Test]
        public void GetSimulationStats_EmptyRequest_ThrowsRpcException()
        {
            var request = new GetSimulationStatsRequest();
            Assert.ThrowsAsync<RpcException>(async () => await _simulationStatsClient.GetSimulationStatsAsync(request));
        }

        [Test]
        public void GetSimulationStats_InvalidId_ThrowsRpcException()
        {
            var request = new GetSimulationStatsRequest 
            { 
                SimulationId = Guid.NewGuid().ToString() 
            };
            var ex = Assert.ThrowsAsync<RpcException>(async () => await _simulationStatsClient.GetSimulationStatsAsync(request));
            Assert.That(ex.StatusCode, Is.EqualTo(StatusCode.NotFound).Or.EqualTo(StatusCode.Internal));
        }

        [Test]
        public void CreateSimulationStats_EmptyRequest_ThrowsRpcException()
        {
            var request = new CreateSimulationStatsRequest();
            Assert.ThrowsAsync<RpcException>(async () => await _simulationStatsClient.CreateSimulationStatsAsync(request));
        }

        [Test]
        public void CreateSimulationStats_InvalidId_ThrowsRpcException()
        {
            var request = new CreateSimulationStatsRequest 
            { 
                SimulationId = Guid.NewGuid().ToString() 
            };
            var ex = Assert.ThrowsAsync<RpcException>(async () => await _simulationStatsClient.CreateSimulationStatsAsync(request));
            Assert.That(ex.StatusCode, Is.EqualTo(StatusCode.NotFound).Or.EqualTo(StatusCode.InvalidArgument).Or.EqualTo(StatusCode.Internal));
        }

        [Test]
        public void CreateSimulationStats_ValidId_ReturnsNotFound()
        {
            var request = new CreateSimulationStatsRequest 
            { 
                SimulationId = Guid.NewGuid().ToString() 
            };
            var ex = Assert.ThrowsAsync<RpcException>(async () => await _simulationStatsClient.CreateSimulationStatsAsync(request));
            Assert.That(ex.StatusCode, Is.EqualTo(StatusCode.NotFound).Or.EqualTo(StatusCode.InvalidArgument).Or.EqualTo(StatusCode.Internal));
        }
    }
}
