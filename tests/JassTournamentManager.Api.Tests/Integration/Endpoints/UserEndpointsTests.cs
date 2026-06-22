using FluentAssertions;
using JassTournamentManager.Api.Tests.Integration.Support;
using JassTournamentManager.Api.Tests.Users;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Users;
using System.Net;
using System.Net.Http.Json;

namespace JassTournamentManager.Api.Tests.Integration.Endpoints
{
    public class UserEndpointsTests
    {
        [Fact]
        public async Task GetClaimableUsers_WithoutAuthentication_ReturnsOk()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            IEnumerable<ClaimableUserResponse> responseBody = UsersControllerTestData.CreateClaimableUserResponses();
            factory.FakeServices.UserService.GetClaimableUsersResult = Result<IEnumerable<ClaimableUserResponse>>.Success(responseBody);

            HttpResponseMessage response = await client.GetAsync("/api/users/claimable");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            factory.FakeServices.UserService.GetClaimableUsersCallCount.Should().Be(1);
        }

        [Fact]
        public async Task CreateUser_WithoutAuthentication_ReturnsUnauthorized()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/users", UsersControllerTestData.CreateCreateUserRequest());

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            factory.FakeServices.UserService.CreateClaimableAsyncCallCount.Should().Be(0);
        }

        [Fact]
        public async Task GetById_WithoutAuthentication_ReturnsUnauthorized()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            Guid userId = Guid.NewGuid();

            HttpResponseMessage response = await client.GetAsync($"/api/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            factory.FakeServices.UserService.ReceivedGetByIdAsyncRequest.Should().BeNull();
        }

        [Fact]
        public async Task ResetPassword_WithAuthenticatedNonSysAdmin_ReturnsForbidden()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            var userId = Guid.NewGuid();
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/users/{userId}/password-reset")
            {
                Content = JsonContent.Create(UsersControllerTestData.CreateResetPasswordRequest())
            };
            request.AddAuthenticatedUser(isSysAdmin: false);

            HttpResponseMessage response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            factory.FakeServices.UserService.ResetPasswordCallCount.Should().Be(0);
        }

        [Fact]
        public async Task ResetPassword_WithSysAdmin_ReturnsOk()
        {
            await using var factory = new JtmApiFactory();
            using HttpClient client = factory.CreateClient();
            var userId = Guid.NewGuid();
            ResetPasswordRequest resetPasswordRequest = UsersControllerTestData.CreateResetPasswordRequest();
            factory.FakeServices.UserService.ResetPasswordResult = Result<bool>.Success(true);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/users/{userId}/password-reset")
            {
                Content = JsonContent.Create(resetPasswordRequest)
            };
            request.AddAuthenticatedUser(isSysAdmin: true);

            HttpResponseMessage response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            factory.FakeServices.UserService.ResetPasswordCallCount.Should().Be(1);
            factory.FakeServices.UserService.ReceivedResetPasswordUserId.Should().Be(userId);
            factory.FakeServices.UserService.ReceivedResetPasswordRequest.Should().Be(resetPasswordRequest);
        }
    }
}
