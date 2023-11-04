using Card_Cost_API;
using Card_Cost_API.Controllers;
using CardCostAPI.Tests.MockupDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.Entity;
using System.Text.Json.Nodes;
using Xunit;

namespace CardCostAPI.Tests
{
    public class CountryCostControllerTests
    {
        [Fact]
        public async Task UpdateCountryCost_WithValidData_ReturnsOk()
        {
            // Arrange
            var json = new JsonObject();
            json["country"] = "FR";
            json["cost"] = "17.0";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";

            var context = GetInMemoryDbContextWithCountryCosts("UpdateCountryCost_WithValidData");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.UpdateCountryCost(apiKey,json);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var countryCost = Assert.IsType<CountryCost>(okResult.Value);
            Assert.Equal("FR", countryCost.Country);
            Assert.Equal(17.0M, countryCost.Cost);
        }

        [Fact]
        public async Task UpdateCountryCost_WithInvalidCountryCode_ReturnsBadRequest()
        {
            // Arrange
            var json = new JsonObject();
            json["country"] = "USA";
            json["cost"] = "10.0";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";

            var context = GetInMemoryDbContextWithCountryCosts("UpdateCountryCost_WithInvalidCountryCode");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.UpdateCountryCost(apiKey, json);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Country Code", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCountryCost_WithInvalidCost_ReturnsBadRequest()
        {
            // Arrange
            var json = new JsonObject();
            json["country"] = "BR";
            json["cost"] = "invalidCost";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";

            var context = GetInMemoryDbContextWithCountryCosts("UpdateCountryCost_WithInvalidCost");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.UpdateCountryCost(apiKey, json);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Cost Format", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCountryCost_WithInvalidApiKey_ReturnsBadRequest()
        {
            // Arrange
            var json = new JsonObject();
            json["country"] = "AR";
            json["cost"] = "20.0";
            var apiKey = "invalid";

            var context = GetInMemoryDbContextWithCountryCosts("UpdateCountryCost_WithInvalidApiKey");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.UpdateCountryCost(apiKey, json);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid API Key", badRequestResult.Value);
        }


        [Fact]
        public async Task GetCountryCost_WithValidCountryCode_ReturnsOk()
        {
            // Arrange
            var validCountryCode = "US";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts("GetCountryCost_WithValidCountryCode");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.GetCountryCost(apiKey, validCountryCode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var countryCost = Assert.IsType<CountryCost>(okResult.Value);
            Assert.Equal(validCountryCode, countryCost.Country);
        }

        [Fact]
        public async Task GetCountryCost_WithInvalidCountryCode_ReturnsBadRequest()
        {
            // Arrange
            string invalidCountryCode = "USA";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts("GetCountryCost_WithInvalidCountryCode");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.GetCountryCost(apiKey, invalidCountryCode);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Country Code", badRequestResult.Value);
        }


        [Fact]
        public async Task GetCountryCost_WithNonExistentCountryCode_ReturnsNotFound()
        {
            // Arrange
            var nonExistentCountryCode = "CA";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts("GetCountryCost_WithNonExistentCountryCode");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.GetCountryCost(apiKey, nonExistentCountryCode);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetCountryCost_WithInvalidApiKey_ReturnsBadRequest()
        {
            // Arrange
            string countryCode = "GE";
            var apiKey = "invalid";
            var context = GetInMemoryDbContextWithCountryCosts("GetCountryCost_WithInvalidApiKey");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.GetCountryCost(apiKey, countryCode);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid API Key", badRequestResult.Value);
        }


        [Fact]
        public async Task DeleteCountryCost_WithValidCountryCode_ReturnsOk()
        {
            // Arrange
            var validCountryCode = "US";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts("DeleteCountryCost_WithValidCountryCode");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.DeleteCountryCost(apiKey, validCountryCode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Country cost deleted successfully.", okResult.Value);
        }


        [Fact]
        public async Task DeleteCountryCost_WithInvalidCountryCode_ReturnsBadRequest()
        {
            // Arrange
            string invalidCountryCode = "USA";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts("DeleteCountryCost_WithInvalidCountryCode");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.DeleteCountryCost(apiKey, invalidCountryCode);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Country Code", badRequestResult.Value);
        }


        [Fact]
        public async Task DeleteCountryCost_WithNonExistentCountryCode_ReturnsNotFound()
        {
            // Arrange
            var nonExistentCountryCode = "CA";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts("DeleteCountryCost_WithNonExistentCountryCode");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.DeleteCountryCost(apiKey, nonExistentCountryCode);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCountryCost_WithInvalidApiKey_ReturnsBadRequest()
        {
            // Arrange
            string countryCode = "AF";
            var apiKey = "invalid";
            var context = GetInMemoryDbContextWithCountryCosts("DeleteCountryCost_WithInvalidApiKey");

            var controller = new CountryCostController(context);

            // Act
            var result = await controller.DeleteCountryCost(apiKey, countryCode);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid API Key", badRequestResult.Value);
        }


        private InMemoryCardCostDBContext GetInMemoryDbContextWithCountryCosts(string databaseName)
        {
            var options = new DbContextOptionsBuilder<CardCostDBContext>()
        .UseInMemoryDatabase(databaseName: databaseName)
        .Options;

            var context = new InMemoryCardCostDBContext(options);

            foreach (var entity in context.CountryCost)
            {
                context.CountryCost.Remove(entity);
            }
            context.SaveChanges();

            var countryCost1 = new CountryCost("US", 10.0M);
            var countryCost2 = new CountryCost("GR", 18.0M);

            context.CountryCost.AddRange(countryCost1, countryCost2);
            context.SaveChanges();

            return context;
        }
    }
}