using System;
using Microsoft.EntityFrameworkCore;

namespace Card_Cost_API
{
    public class CardCostDBContext : DbContext
    {
        public DBSet<ClearingCostMatrix> Users { get; set; }

    }
}