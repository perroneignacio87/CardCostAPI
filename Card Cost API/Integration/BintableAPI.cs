using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Card_Cost_API.Integration
{
    public class BintableAPI : IBintableAPI
    {
        public async Task<string?> GetCardCountryCode(string cardNumber)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string apiUrl = configuration.GetSection("BintableApiUrl").Value;
            string apiKey = configuration.GetSection("BintableApiKey").Value;

            apiUrl = apiUrl.Replace("{cardNumber}", cardNumber).Replace("{apiKey}", apiKey);

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(content))
                        {
                            JsonElement root = doc.RootElement;

                            if (root.TryGetProperty("data", out JsonElement data) &&
                            data.TryGetProperty("country", out JsonElement country) &&
                            country.TryGetProperty("code", out JsonElement countryCode))
                            {
                                return await Task.FromResult(countryCode.GetString());
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (HttpRequestException e)
                {
                    throw new Exception("Bintable API Request Failed: " + e.Message);
                }

            }
        }
    }
}
