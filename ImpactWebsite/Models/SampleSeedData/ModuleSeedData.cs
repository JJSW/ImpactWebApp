using ImpactWebsite.Data;
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
            GetSavings(db);
            GetPromotions(db);
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
                    Description = "Description Overview and Financials",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 0).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Operational blueprint and asset-level data",
                    Description = "Description Operational blueprint and asset-level data",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Social Impact metrics",
                    Description = "Description Social Impact metrics",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Environmental impact metrics",
                    Description = "Description Environmental impact metrics",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Governance and controversies",
                    Description = "Description Governance and controversies",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Upstream and downstream supplier analysis",
                    Description = "Description Upstream and downstream supplier analysis",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Regulatory, climate-realted and other risk analysis",
                    Description = "Description Regulatory, climate-realted and other risk analysis",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                }); db.Modules.Add(new Module()
                {
                    ModuleName = "Benchmarking and targets",
                    Description = "Description Benchmarking and targets",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.SaveChanges();
            }
        }

        public static void GetSavings(ApplicationDbContext db)
        {
            if (!db.Savings.Any())
            {
                db.Savings.Add(new Saving()
                {
                    SavingName = "Discount1",
                    SavingRate = 10,
                    SelectFrom = 3,
                    SelectTo = 5,
                    Description = "Discount $10 for selections of 3 to 5"
                });

                db.Savings.Add(new Saving()
                {
                    SavingName = "Discount2",
                    SavingRate = 20,
                    SelectFrom = 6,
                    SelectTo = 8,
                    Description = "Discount $20 for selections of 6 to 8"
                });

                db.SaveChanges();
            }
        }

        public static void GetPromotions(ApplicationDbContext db)
        {
            if (!db.Promotions.Any())
            {
                db.Promotions.Add(new Promotion()
                {
                    PromotionName = "Promotion Sample - Fixed",
                    PromotionCode = "AAAAAAAA",
                    DiscountMethod = DiscountMethodList.Fixed,
                    DiscountRate = 10,
                    DateFrom = DateTime.Today,
                    DateTo = DateTime.Today.AddYears(1),
                    IsActive = true,
                    Description = "Sample Promotion - fixed discount rate - 10"
                });
            }
        }
    }
}
