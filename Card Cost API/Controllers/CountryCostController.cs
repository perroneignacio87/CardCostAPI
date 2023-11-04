
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json.Nodes;

namespace Card_Cost_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryCostController : ControllerBase
    {
        private readonly CardCostDBContext _dbContext;

        public CountryCostController(CardCostDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("update/{apiKey}")]
        public async Task<IActionResult> UpdateCountryCost(string apiKey, [FromBody] JsonObject inputJson)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                string? configApiKey = configuration.GetSection("ApiKey").Value;

                if(configApiKey != null)
                {
                    if(apiKey == configApiKey)
                    {
                        if (inputJson == null)
                        {
                            return BadRequest("Invalid JSON data");
                        }

                        if (inputJson["country"] == null || inputJson["cost"] == null)
                        {
                            return BadRequest("Invalid JSON data");
                        }

                        string countryCode = inputJson["country"].ToString();
                        countryCode = countryCode.ToUpper();
                        string stringCost = inputJson["cost"].ToString();
                        decimal cost;

                        if (decimal.TryParse(stringCost, out cost))
                        {
                            if (countryCode.Length == 2 || countryCode == "Others")
                            {
                                var countryCost = await _dbContext.CountryCost.FirstOrDefaultAsync(cc => cc.Country == countryCode);
                                if (countryCost == null)
                                {
                                    countryCost = new CountryCost(countryCode, cost);
                                    _dbContext.CountryCost.Add(countryCost);
                                }
                                else
                                {
                                    countryCost.Cost = cost;
                                }

                                await _dbContext.SaveChangesAsync();
                                return Ok(countryCost);
                            }
                            else
                            {
                                return BadRequest("Invalid Country Code");
                            }

                        }
                        else
                        {
                            return BadRequest("Invalid Cost Format");
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

        [HttpGet]
        [Route("get/{apiKey}/{countryCode}")]
        public async Task<IActionResult> GetCountryCost(string apiKey, string countryCode)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                string? configApiKey = configuration.GetSection("ApiKey").Value;

                if (configApiKey != null)
                {
                    if (apiKey == configApiKey)
                    {

                        if (countryCode == null)
                        {
                            return BadRequest("Invalid Country Code");
                        }
                        if (countryCode.Length > 2)
                        {
                            return BadRequest("Invalid Country Code");
                        }

                        countryCode = countryCode.ToUpper();
                        var countryCost = await _dbContext.CountryCost.FirstOrDefaultAsync(cc => cc.Country == countryCode);

                        if (countryCost == null)
                        {
                            return NotFound();
                        }
                        else
                        {
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

        [HttpDelete]
        [Route("delete/{apiKey}/{countryCode}")]
        public async Task<IActionResult> DeleteCountryCost(string apiKey, string countryCode)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                string? configApiKey = configuration.GetSection("ApiKey").Value;

                if (configApiKey != null)
                {
                    if (apiKey == configApiKey)
                    {
                        if (countryCode == null)
                        {
                            return BadRequest("Invalid Country Code");
                        }
                        if (countryCode.Length > 2)
                        {
                            return BadRequest("Invalid Country Code");
                        }

                        countryCode = countryCode.ToUpper();
                        var countryCost = await _dbContext.CountryCost.FirstOrDefaultAsync(cc => cc.Country == countryCode);

                        if (countryCost == null)
                        {
                            return NotFound();
                        }

                        _dbContext.CountryCost.Remove(countryCost);
                        await _dbContext.SaveChangesAsync();

                        return Ok("Country cost deleted successfully.");
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