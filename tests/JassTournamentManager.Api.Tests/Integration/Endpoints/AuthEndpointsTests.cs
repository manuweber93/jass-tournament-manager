using FluentAssertions;
using JassTournamentManager.Api.Tests.Auth;
using JassTournamentManager.Api.Tests.Integration.Support;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Auth;
using System.Net;
using System.Net.Http.Json;

namespace JassTournamentManager.Api.Tests.Integration.Endpoints
{
    public class AuthEndpointsTests
    {
        [Fact]
        public async Task Login_WithoutAuthentication_ReturnsOk()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            AuthResponse authResponse = AuthControllerTestData.CreateAuthResponse();
            LoginRequest request = AuthControllerTestData.CreateLoginRequest();
            factory.FakeServices.AuthService.LoginAsyncResult = Result<AuthResponse>.Success(authResponse);

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            factory.FakeServices.AuthService.ReceivedLoginAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task Register_WithoutAuthentication_ReturnsOk()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            AuthResponse authResponse = AuthControllerTestData.CreateAuthResponse();
            RegisterRequest request = AuthControllerTestData.CreateRegisterRequest();
            factory.FakeServices.AuthService.RegisterAsyncResult = Result<AuthResponse>.Success(authResponse);

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/register", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            factory.FakeServices.AuthService.ReceivedRegisterAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task RefreshSession_WithoutAuthentication_ReturnsOk()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            AuthResponse authResponse = AuthControllerTestData.CreateAuthResponse();
            RefreshSessionRequest request = AuthControllerTestData.CreateRefreshSessionRequest();
            factory.FakeServices.AuthService.RefreshSessionAsyncResult = Result<AuthResponse>.Success(authResponse);

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/refresh", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            factory.FakeServices.AuthService.ReceivedRefreshSessionAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task Logout_WithoutAuthentication_ReturnsUnauthorized()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            var request = new LogoutRequest("refresh-token");

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/logout", request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            factory.FakeServices.AuthService.LogoutAsyncCallCount.Should().Be(0);
        }

        [Fact]
        public async Task Logout_WithAuthentication_ReturnsOk()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/logout")
            {
                Content = JsonContent.Create(new LogoutRequest("refresh-token"))
            };
            request.AddAuthenticatedUser();

            HttpResponseMessage response = await client.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, responseBody);
            factory.FakeServices.AuthService.LogoutAsyncCallCount.Should().Be(1);
            factory.FakeServices.AuthService.ReceivedLogoutAsyncRequest.Should().Be(new LogoutRequest("refresh-token"));
        }
    }
}
