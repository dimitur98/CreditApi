using Credit.Api.Db;
using Credit.Api.Db.Models;
using Credit.Api.Db.Repositories;
using Dapper.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;
using System.Net.Http.Json;
namespace Credit.Api.Tests
{
    public class CreditsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<ICreditRepository> _mockCreditRepo;
        private readonly Mock<IDbInitializer> _mockDbInitializer;

        public CreditsEndpointsTests(WebApplicationFactory<Program> factory)
        {
            _mockCreditRepo = new Mock<ICreditRepository>();
            _mockDbInitializer = new Mock<IDbInitializer>();

            this.SetupMockData();

            var appFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<ICreditRepository>();
                    services.RemoveAll<IDbInitializer>();

                    services.AddSingleton(_mockCreditRepo.Object);
                    services.AddSingleton(_mockDbInitializer.Object);
                });
            });

            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task GetCredits_ShouldReturnMockedCredits()
        {
            var response = await _client.GetAsync("/api/Credits/All");
            var credits = await response.Content.ReadFromJsonAsync<List<Db.Models.Credit>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(credits);
            Assert.Equal(3, credits.Count);
        }

        [Fact]
        public async Task GetStatusReport_ShouldReturnMockedStatusReport()
        {
            var response = await _client.GetAsync("/api/Credits/StatusReport");
            var statusReports = await response.Content.ReadFromJsonAsync<List<CreditStatusReport>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(statusReports);
            Assert.Equal(2, statusReports.Count);
        }

        private void SetupMockData()
        {
            var mockCredits = new List<Db.Models.Credit>
            {
                new Db.Models.Credit { Id = 1, CreditNumber = "CRD001", CustomerName = "John Doe", Amount = 5000, DateRequested = new DateTime(2024,01,01), StatusId = 1 },
                new Db.Models.Credit { Id = 2, CreditNumber = "CRD002", CustomerName = "Jane Smith", Amount = 3000, DateRequested = new DateTime(2024,03,10), StatusId = 2 },
                new Db.Models.Credit { Id = 3, CreditNumber = "CRD003", CustomerName = "Kael Draven", Amount = 5000 , DateRequested = new DateTime(2024, 05, 11), StatusId = 3 },
            };

            var mockStatusReports = new List<CreditStatusReport>
            {
                new CreditStatusReport { Status = "Paid", TotalAmount = 8000, Percentage = 50.0 },
                new CreditStatusReport { Status = "AwaitingPayment", TotalAmount = 8000, Percentage = 50.0 }
            };

            _mockCreditRepo.Setup(repo => repo.AllAsync()).ReturnsAsync(mockCredits);
            _mockCreditRepo.Setup(repo => repo.GetStatusReportAsync(It.IsAny<IEnumerable<uint>>()))
                           .ReturnsAsync(mockStatusReports);
            _mockDbInitializer.Setup(db => db.InitializeAsync()).Returns(Task.CompletedTask);
        }
    }
}