using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Card_Cost_API
{
    public class CountryCost
    {
        [Key]
        public string? Country { get; set; }

        public decimal Cost { get; set; }

        public CountryCost(string country, decimal cost) 
        {
            this.Country = country;
            this.Cost = cost;
        }

    }
}