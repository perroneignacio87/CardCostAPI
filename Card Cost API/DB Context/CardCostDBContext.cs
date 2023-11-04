using System;
using Microsoft.EntityFrameworkCore;

namespace Card_Cost_API
{
    public class CardCostDBContext : DbContext
    {
        public CardCostDBContext() : base()
        {
        }
        public CardCostDBContext(DbContextOptions<CardCostDBContext> options) : base(options)
        {
        }
        public virtual DbSet<CountryCost> CountryCost { get; set; }

    }
}