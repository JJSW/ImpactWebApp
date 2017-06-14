﻿using ImpactWebsite.Data;
using ImpactWebsite.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.SampleSeedData
{
    public class ModuleSeedData
    {
        public static void Initialize(ApplicationDbContext db)
        {
            GetUnitPrice(db);
            GetModules(db);
            GetDiscounts(db);
        }

        public static void GetUnitPrice(ApplicationDbContext db)
        {
            if (!db.UnitPrices.Any())
            {
                db.UnitPrices.Add(new UnitPrice()
                {
                    Price = 0,
                    DateEffectFrom = new DateTime(2017, 01, 01),
                });
                db.UnitPrices.Add(new UnitPrice()
                {
                    Price = 10,
                    DateEffectFrom = new DateTime(2017, 01, 01),
                });
                db.UnitPrices.Add(new UnitPrice()
                {
                    Price = 15,
                    DateEffectFrom = new DateTime(2017, 01, 01),
                });
                db.UnitPrices.Add(new UnitPrice()
                {
                    Price = 20,
                    DateEffectFrom = new DateTime(2017, 01, 01),
                });
                db.UnitPrices.Add(new UnitPrice()
                {
                    Price = 25,
                    DateEffectFrom = new DateTime(2017, 01, 01),
                });
                db.SaveChanges();
            }
        }
        public static void GetModules(ApplicationDbContext db)
        {
            if (!db.Modules.Any())
            {
                db.Modules.Add(new Module()
                {
                    ModuleName = "Overview and Financials",
                    Description = "Overview and Financials",
                    LongDescription = "Long Overview and Financials",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 0).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Operational blueprint and asset-level data",
                    Description = "Operational blueprint and asset-level data",
                    LongDescription = "Long Operational blueprint and asset-level data",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Social Impact metrics",
                    Description = "Social Impact metrics",
                    LongDescription = "Long Social Impact metrics",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Environmental impact metrics",
                    Description = "Environmental impact metrics",
                    LongDescription = "Long Environmental impact metrics",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Governance and controversies",
                    Description = "Governance and controversies",
                    LongDescription = "Long Governance and controversies",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Upstream and downstream supplier analysis",
                    Description = "Upstream and downstream supplier analysis",
                    LongDescription = "Long Upstream and downstream supplier analysis",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Regulatory, climate-realted and other risk analysis",
                    Description = "Regulatory, climate-realted and other risk analysis",
                    LongDescription = "Long Regulatory, climate-realted and other risk analysis",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                }); db.Modules.Add(new Module()
                {
                    ModuleName = "Benchmarking and targets",
                    Description = "Benchmarking and targets",
                    LongDescription = "Long Benchmarking and targets",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.SaveChanges();
            }
        }

        public static void GetDiscounts(ApplicationDbContext db)
        {
            if (!db.Discounts.Any())
            {
                db.Discounts.Add(new Discount()
                {
                    DiscountName = "Discount1",
                    DiscountRate = 10,
                    SelectFrom = 3,
                    SelectTo = 5,
                    Description = "Discount $10 for selections of 3 to 5"
                });

                db.Discounts.Add(new Discount()
                {
                    DiscountName = "Discount2",
                    DiscountRate = 20,
                    SelectFrom = 6,
                    SelectTo = 8,
                    Description = "Discount $20 for selections of 6 to 8"
                });

                db.SaveChanges();
            }
        }
    }
}
