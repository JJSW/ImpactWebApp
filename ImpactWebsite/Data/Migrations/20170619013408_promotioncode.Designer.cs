using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ImpactWebsite.Data;
using ImpactWebsite.Models.OrderModels;

namespace WebApplication1.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170619013408_promotioncode")]
    partial class promotioncode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImpactWebsite.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("BillingAddressId");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(160);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasMaxLength(160);

                    b.Property<bool>("IsTempUser");

                    b.Property<string>("LastName")
                        .HasMaxLength(160);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<bool>("NewsletterRequired");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int?>("PromotionId");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ImpactWebsite.Models.BillingModels.BillingAddress", b =>
                {
                    b.Property<int>("BillingAddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("BillingName");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("State");

                    b.Property<string>("UserId");

                    b.Property<string>("ZipCode");

                    b.HasKey("BillingAddressId");

                    b.ToTable("BillingAddresses");
                });

            modelBuilder.Entity("ImpactWebsite.Models.Investment", b =>
                {
                    b.Property<long>("InvestmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("EstimateValue");

                    b.Property<string>("ISIN")
                        .HasMaxLength(160);

                    b.Property<string>("InvestmentName")
                        .IsRequired()
                        .HasMaxLength(160);

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("ShareNumber");

                    b.HasKey("InvestmentId");

                    b.ToTable("Investments");
                });

            modelBuilder.Entity("ImpactWebsite.Models.NewsletterUser", b =>
                {
                    b.Property<long>("NewsletterUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<bool>("isSubscribed");

                    b.HasKey("NewsletterUserId");

                    b.ToTable("NewsletterUsers");
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.Discount", b =>
                {
                    b.Property<long>("DiscountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("DiscountName");

                    b.Property<int>("DiscountRate");

                    b.Property<bool>("IsActive");

                    b.Property<int>("SelectFrom");

                    b.Property<int>("SelectTo");

                    b.HasKey("DiscountId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.Module", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("LongDescription");

                    b.Property<string>("ModuleName");

                    b.Property<int>("UnitPriceId");

                    b.HasKey("ModuleId");

                    b.HasIndex("UnitPriceId");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("DeliveredDate");

                    b.Property<bool>("IsPromotionCodeApplied");

                    b.Property<string>("NoteFromAdmin");

                    b.Property<string>("NoteFromUser");

                    b.Property<int>("OrderNum");

                    b.Property<int>("OrderStatus");

                    b.Property<DateTime>("OrderedDate");

                    b.Property<int>("PromotionId");

                    b.Property<string>("SalesRep")
                        .HasMaxLength(160);

                    b.Property<int>("SelectionDiscount");

                    b.Property<int>("TotalAmount");

                    b.Property<string>("UserEmail")
                        .IsRequired();

                    b.Property<string>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("ModuleId");

                    b.Property<string>("ModuleName");

                    b.Property<int>("OrderId");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("ModuleId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.Promotion", b =>
                {
                    b.Property<int>("PromotionId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateFrom");

                    b.Property<DateTime>("DateTo");

                    b.Property<string>("Description");

                    b.Property<int>("DiscountMethod");

                    b.Property<int>("DiscountRate");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("PromotionCode")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("PromotionName")
                        .HasMaxLength(160);

                    b.HasKey("PromotionId");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("ImpactWebsite.Models.UnitPrice", b =>
                {
                    b.Property<int>("UnitPriceId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateEffectFrom");

                    b.Property<DateTime>("DateEffectTo");

                    b.Property<int>("Price");

                    b.HasKey("UnitPriceId");

                    b.ToTable("UnitPrices");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ImpactWebsite.Models.ApplicationUser", b =>
                {
                    b.HasOne("ImpactWebsite.Models.BillingModels.BillingAddress", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId");
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.Module", b =>
                {
                    b.HasOne("ImpactWebsite.Models.UnitPrice", "UnitPrice")
                        .WithMany()
                        .HasForeignKey("UnitPriceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.Order", b =>
                {
                    b.HasOne("ImpactWebsite.Models.ApplicationUser")
                        .WithMany("Orders")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("ImpactWebsite.Models.OrderModels.OrderDetail", b =>
                {
                    b.HasOne("ImpactWebsite.Models.OrderModels.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImpactWebsite.Models.OrderModels.Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ImpactWebsite.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ImpactWebsite.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImpactWebsite.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
