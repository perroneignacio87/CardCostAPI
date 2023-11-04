using Card_Cost_API;
using Card_Cost_API.Controllers;
using Card_Cost_API.Integration;
using CardCostAPI.Tests.MockupDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text.Json.Nodes;
using Xunit;

namespace CardCostAPI.Tests
{
    public class CardCostControllerTests
    {

        [Fact]
        public async Task GetCardCost_Returns_OkResult_When_CountryCode_Is_Found_US()
        {
            var inputJson = new JsonObject { ["card_number"] = "403244" }; 
            var countryCode = "EG";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts();
            var controller = new CardCostController(context);

            // Act
            var result = await controller.GetCardCost(apiKey, inputJson);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<CountryCost>(okResult.Value);
            Assert.Equal(countryCode, model.Country);
        }

        [Fact]
        public async Task GetCardCost_Returns_OkResult_When_CountryCode_Is_Found_Other()
        {
            var inputJson = new JsonObject { ["card_number"] = "455175" }; 
            var countryCode = "Others";
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts();

            var controller = new CardCostController(context);

            // Act
            var result = await controller.GetCardCost(apiKey, inputJson);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<CountryCost>(okResult.Value);
            Assert.Equal(countryCode, model.Country);
        }

        [Fact]
        public async Task GetCardCost_Returns_BadRequest_When_CardNumber_Is_Invalid()
        {
            var inputJson = new JsonObject { ["card_number"] = "45517" };
            var apiKey = "he8k39p2l29lisj30s1b4zoal20pi4kk3bvs8l";
            var context = GetInMemoryDbContextWithCountryCosts();

            var controller = new CardCostController(context);

            // Act
            var result = await controller.GetCardCost(apiKey, inputJson);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Card Number needs to have at least 6 digits", badRequestResult.Value);
        }

        [Fact]
        public async Task GetCardCost_Returns_BadRequest_When_ApiKey_Is_Invalid()
        {
            var inputJson = new JsonObject { ["card_number"] = "455175" };
            var apiKey = "invalid";
            var context = GetInMemoryDbContextWithCountryCosts();

            var controller = new CardCostController(context);

            // Act
            var result = await controller.GetCardCost(apiKey, inputJson);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid API Key", badRequestResult.Value);
        }


        private InMemoryCardCostDBContext GetInMemoryDbContextWithCountryCosts()
        {
            var options = new DbContextOptionsBuilder<CardCostDBContext>()
        .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
        .Options;

            var context = new InMemoryCardCostDBContext(options);

            foreach (var entity in context.CountryCost)
            {
                context.CountryCost.Remove(entity);
            }
            context.SaveChanges();

            var countryCost1 = new CountryCost("EG", 10.0M);
            var countryCost2 = new CountryCost("GB", 18.0M);
            var countryCost3 = new CountryCost("Others", 15.0M);

            context.CountryCost.AddRange(countryCost1, countryCost2, countryCost3);
            context.SaveChanges();

            return context;
        }
    }
}