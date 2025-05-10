using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Đặt tên bảng tùy chỉnh nếu muốn
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityUserClaim<string>>(e => { e.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(e => { e.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(e => { e.ToTable("UserTokens"); });
            builder.Entity<IdentityRole>(e => { e.ToTable("Roles"); });
            builder.Entity<IdentityRoleClaim<string>>(e => { e.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserRole<string>>(e => { e.ToTable("UserRoles"); });
        }
        //DbSet
        public DbSet<Prices> Prices { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Categorys> Categorys { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoicesDetail { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderStatistics> OrderStatistics { get; set; }
        public DbSet<PageVisitStatistics> PageVisitStatistics { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Pictures> Pictures { get; set; }
        public DbSet<ProductReviews> ProductReviews { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<SalesByCategoryStatistics> SalesByCategoryStatistics { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<UserStatistics> UserStatistics { get; set; }
        public DbSet<Vouchers> Vouchers { get; set; }
    }
}
