using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card_Cost_API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace CardCostAPI.Tests.MockupDBContext
{
    public class InMemoryCardCostDBContext : CardCostDBContext
    {
        public InMemoryCardCostDBContext(DbContextOptions<CardCostDBContext> options) : base(options)
        {
        }

        public static InMemoryCardCostDBContext Create()
        {
            var options = new DbContextOptionsBuilder<CardCostDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            return new InMemoryCardCostDBContext(options);
        }
    }
}
