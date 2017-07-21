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
            GetModules(db);
            GetSavings(db);
            GetPromotions(db);
        }

        public static void GetModules(ApplicationDbContext db)
        {
            if (!db.Modules.Any())
            {
                db.Modules.Add(new Module()
                {
                    ModuleName = "Overview and Financials",
                    Description = "Description Overview and Financials",
                    UnitPrice = 0,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Operational blueprint and asset-level data",
                    Description = "Description Operational blueprint and asset-level data",
                    UnitPrice = 25,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Social Impact metrics",
                    Description = "Description Social Impact metrics",
                    UnitPrice = 25,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Environmental impact metrics",
                    Description = "Description Environmental impact metrics",
                    UnitPrice = 25,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Governance and controversies",
                    Description = "Description Governance and controversies",
                    UnitPrice = 25,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Upstream and downstream supplier analysis",
                    Description = "Description Upstream and downstream supplier analysis",
                    UnitPrice = 25,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Regulatory, climate-realted and other risk analysis",
                    Description = "Description Regulatory, climate-realted and other risk analysis",
                    UnitPrice = 25,
                }); db.Modules.Add(new Module()
                {
                    ModuleName = "Benchmarking and targets",
                    Description = "Description Benchmarking and targets",
                    UnitPrice = 25,
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
