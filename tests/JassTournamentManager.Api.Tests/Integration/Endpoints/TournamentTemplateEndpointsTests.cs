using FluentAssertions;
using JassTournamentManager.Api.Tests.Integration.Support;
using JassTournamentManager.Api.Tests.TournamentTemplates;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.TournamentTemplates;
using System.Net;
using System.Net.Http.Json;

namespace JassTournamentManager.Api.Tests.Integration.Endpoints
{
    public class TournamentTemplateEndpointsTests
    {
        [Fact]
        public async Task GetForCurrentUser_WithoutAuthentication_ReturnsUnauthorized()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/api/tournament-templates/me");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            factory.FakeServices.TournamentTemplateService.GetForCurrentUserAsyncCallCount.Should().Be(0);
        }

        [Fact]
        public async Task GetForCurrentUser_WithAuthentication_ReturnsOk()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            TournamentTemplateResponse tournamentTemplateResponse = TournamentTemplatesControllerTestData.CreateTournamentTemplateResponse();
            factory.FakeServices.TournamentTemplateService.GetForCurrentUserAsyncResult = Result<TournamentTemplateResponse>.Success(tournamentTemplateResponse);
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/tournament-templates/me");
            request.AddAuthenticatedUser();

            HttpResponseMessage response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            factory.FakeServices.TournamentTemplateService.GetForCurrentUserAsyncCallCount.Should().Be(1);
        }

        [Fact]
        public async Task GetById_WithInvalidGuidRoute_ReturnsNotFound()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/tournament-templates/not-a-guid");
            request.AddAuthenticatedUser();

            HttpResponseMessage response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_WithoutAuthentication_ReturnsUnauthorized()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            CreateTournamentTemplateRequest request = TournamentTemplatesControllerTestData.CreateCreateTournamentTemplateRequest();

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/tournament-templates", request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            factory.FakeServices.TournamentTemplateService.CreateAsyncCallCount.Should().Be(0);
        }

        [Fact]
        public async Task Create_WithValidRequest_ReturnsCreatedWithLocationHeader()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            CreateTournamentTemplateRequest createRequest = TournamentTemplatesControllerTestData.CreateCreateTournamentTemplateRequest();
            TournamentTemplateResponse tournamentTemplateResponse = TournamentTemplatesControllerTestData.CreateTournamentTemplateResponse();
            factory.FakeServices.TournamentTemplateService.CreateAsyncResult = Result<TournamentTemplateResponse>.Success(tournamentTemplateResponse);
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/tournament-templates")
            {
                Content = JsonContent.Create(createRequest)
            };
            request.AddAuthenticatedUser();

            HttpResponseMessage response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location!.ToString().Should().EndWith($"/api/tournament-templates/{tournamentTemplateResponse.Id}");
            factory.FakeServices.TournamentTemplateService.CreateAsyncCallCount.Should().Be(1);
            factory.FakeServices.TournamentTemplateService.ReceivedCreateAsyncRequest.Should().Be(createRequest);
        }

        [Fact]
        public async Task Create_WithMissingConfig_ReturnsBadRequest()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/tournament-templates")
            {
                Content = JsonContent.Create(new { Location = TournamentTemplatesControllerTestData.CreateLocation() })
            };
            request.AddAuthenticatedUser();

            HttpResponseMessage response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            factory.FakeServices.TournamentTemplateService.CreateAsyncCallCount.Should().Be(0);
        }
    }
}
