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
                    ModifiedDate = DateTime.Today,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Operational blueprint and asset-level data",
                    Description = "Description Operational blueprint and asset-level data",
                    UnitPrice = 25,
                    ModifiedDate = DateTime.Today,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Social Impact metrics",
                    Description = "Description Social Impact metrics",
                    UnitPrice = 25,
                    ModifiedDate = DateTime.Today,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Environmental impact metrics",
                    Description = "Description Environmental impact metrics",
                    UnitPrice = 25,
                    ModifiedDate = DateTime.Today,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Governance and controversies",
                    Description = "Description Governance and controversies",
                    UnitPrice = 25,
                    ModifiedDate = DateTime.Today,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Upstream and downstream supplier analysis",
                    Description = "Description Upstream and downstream supplier analysis",
                    UnitPrice = 25,
                    ModifiedDate = DateTime.Today,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Regulatory, climate-realted and other risk analysis",
                    Description = "Description Regulatory, climate-realted and other risk analysis",
                    UnitPrice = 25,
                    ModifiedDate = DateTime.Today,
                });
                db.Modules.Add(new Module()
                {
                    ModuleName = "Benchmarking and targets",
                    Description = "Description Benchmarking and targets",
                    UnitPrice = 25,
                    ModifiedDate = DateTime.Today,
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
                    DiscountMethod = SavingDiscountMethodList.Fixed,
                    SavingRate = 10,
                    SelectFrom = 2,
                    SelectTo = 4,
                    Description = "Discount $10 for selections of 2 to 4",
                    ModifiedDate = DateTime.Today,
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
                    DiscountMethod = PromotionDiscountMethodList.Fixed,
                    DiscountRate = 10,
                    DateFrom = DateTime.Today,
                    DateTo = DateTime.Today.AddYears(1),
                    IsActive = true,
                    Description = "Sample Promotion - fixed discount rate - 10",
                    ModifiedDate = DateTime.Today,
                });
            }
        }
    }
}
