using Card_Cost_API;
using Card_Cost_API.Controllers;
using Card_Cost_API.Integration;
using CardCostAPI.Tests.MockupDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net;
using System.Text.Json.Nodes;
using Xunit;
using FluentAssertions;

namespace CardCostAPI.Tests
{
    public class BintableAPITests
    {
        [Fact]
        public async Task GetCountryInfoFromThirdPartyApi_ShouldReturnCountryInfo()
        {
            // Arrange
            var cardCode = "455175";
            var apiKey = "bc6998a4771b057d07029b7e94afb32d5a5a7eae"; 
            var apiUrl = $"https://api.bintable.com/v1/{cardCode}?api_key={apiKey}";

            using (var httpClient = new HttpClient())
            {
                // Act
                var response = await httpClient.GetAsync(apiUrl);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
            }
        }

    }
}