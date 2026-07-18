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

        [SetUp]
        public void SetUp()
        {
            var url = ConfigReader.GetStatisticsGrpcUrl();
            _channel = GrpcChannel.ForAddress(url);
            _scoreboardClient = new ScoreboardService.ScoreboardServiceClient(_channel);
            _simulationStatsClient = new SimulationStatsService.SimulationStatsServiceClient(_channel);
        }

        [TearDown]
        public void TearDown()
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


        


        // --- SimulationStatsService Tests ---

        [Test]
        public void GetSimulationStats_EmptyRequest_ThrowsRpcException()
        {
            var request = new GetSimulationStatsRequest();
            Assert.ThrowsAsync<RpcException>(async () => await _simulationStatsClient.GetSimulationStatsAsync(request));
        }



        [Test]
        public void CreateSimulationStats_EmptyRequest_ThrowsRpcException()
        {
            var request = new CreateSimulationStatsRequest();
            Assert.ThrowsAsync<RpcException>(async () => await _simulationStatsClient.CreateSimulationStatsAsync(request));
        }




    }
}
