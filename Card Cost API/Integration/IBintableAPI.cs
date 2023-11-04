namespace Card_Cost_API.Integration
{
    public interface IBintableAPI
    {
        Task<string?> GetCardCountryCode(string cardNumber);
    }
}
