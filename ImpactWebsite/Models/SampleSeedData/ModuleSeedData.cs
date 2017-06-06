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
                db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Overview and Financials",
                    DeliveryDays = 3,
                    Description = "Overview and Financials",
                    LongDescription = "Long Overview and Financials",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 0).UnitPriceId
                });
                db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Operational blueprint and asset-level data",
                    DeliveryDays = 3,
                    Description = "Operational blueprint and asset-level data",
                    LongDescription = "Long Operational blueprint and asset-level data",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Social Impact metrics",
                    DeliveryDays = 3,
                    Description = "Social Impact metrics",
                    LongDescription = "Long Social Impact metrics",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Environmental impact metrics",
                    DeliveryDays = 3,
                    Description = "Environmental impact metrics",
                    LongDescription = "Long Environmental impact metrics",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Governance and controversies",
                    DeliveryDays = 3,
                    Description = "Governance and controversies",
                    LongDescription = "Long Governance and controversies",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Upstream and downstream supplier analysis",
                    DeliveryDays = 3,
                    Description = "Upstream and downstream supplier analysis",
                    LongDescription = "Long Upstream and downstream supplier analysis",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Regulatory, climate-realted and other risk analysis",
                    DeliveryDays = 3,
                    Description = "Regulatory, climate-realted and other risk analysis",
                    LongDescription = "Long Regulatory, climate-realted and other risk analysis",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                }); db.Modules.Add(new OrderModule()
                {
                    ModuleName = "Benchmarking and targets",
                    DeliveryDays = 3,
                    Description = "Benchmarking and targets",
                    LongDescription = "Long Benchmarking and targets",
                    UnitPriceId = db.UnitPrices.FirstOrDefault(u => u.Price == 25).UnitPriceId
                });
                db.SaveChanges();
            }
        }
    }
}
