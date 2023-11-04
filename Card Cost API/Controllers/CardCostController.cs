using Card_Cost_API.Integration;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace Card_Cost_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardCostController : ControllerBase
    {
        private readonly CardCostDBContext _dbContext;

        public CardCostController(CardCostDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("get/{apiKey}")]
        public async Task<IActionResult> GetCardCost(string apiKey, [FromBody] JsonObject inputJson)
        {
            try
            {
                var bintableAPI = new BintableAPI();
                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                string? configApiKey = configuration.GetSection("ApiKey").Value;

                if (configApiKey != null)
                {
                    if (apiKey == configApiKey)
                    {
                        if (inputJson == null)
                        {
                            return BadRequest("Invalid JSON data");
                        }

                        if (inputJson["card_number"] == null)
                        {
                            return BadRequest("Invalid JSON data");
                        }

                        if (inputJson["card_number"].ToString().Length < 6)
                        {
                            return BadRequest("Card Number needs to have at least 6 digits");
                        }

                        string cardNumber = inputJson["card_number"].ToString();
                        string issuerIdentificationNumber = cardNumber.Substring(0, 6);

                        string? countryCode = null;

                        countryCode = await bintableAPI.GetCardCountryCode(issuerIdentificationNumber);

                        if (countryCode == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            countryCode = countryCode.ToUpper();
                            var countryCost = _dbContext.CountryCost.FirstOrDefault(cc => cc.Country == countryCode);
                            if (countryCost == null)
                            {
                                countryCost = _dbContext.CountryCost.FirstOrDefault(cc => cc.Country == "Others");
                            }
                            if (countryCost == null)
                            {
                                return NotFound();
                            }
                            return Ok(countryCost);
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid API Key");
                    }
                }
                else
                {
                    return BadRequest("Invalid API Key");
                }

               
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred calling the service: " + ex.Message);
            }
            
        }

    }
}