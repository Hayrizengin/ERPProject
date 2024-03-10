using ERPProject.Entity.Poco;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.DataAccess.Concrete.EntityFramework.Context
{
    public partial class ERPContext : DbContext
    {
        public ERPContext()
        {
        }

        public ERPContext(DbContextOptions<ERPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public virtual DbSet<Offer> Offers { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Stock> Stocks { get; set; }

        public virtual DbSet<StockDetail> StockDetails { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HSD1K26\\SQLEXPRESS; Initial Catalog=ErpDB; Integrated Security=true; TrustServerCertificate=True");
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {

                entity.ToTable("Brand");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Company).WithMany(p => p.Departments)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Department_Company");


                entity.HasMany(d => d.Users).WithOne(d => d.Department).HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Department");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasMany(x => x.InvoiceDetails).WithOne(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_Invoice");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.ToTable("InvoiceDetail");

                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("Offer");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PriceStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Status)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.SupplierName).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasOne(e => e.User).WithMany(e => e.Offers)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offer_User");

                entity.HasOne(e => e.Request).WithMany(e => e.Offers)
                .HasForeignKey(e => e.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offer_Request");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Brand");

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(511);
                entity.Property(e => e.AcceptedId).IsRequired(false);
                entity.Property(e => e.Title).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.User).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_User");
                entity.HasOne(d => d.AcceptedUser).WithMany(p => p.AcceptedRequests)
                    .HasForeignKey(d => d.AcceptedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_AcceptedRequests");

            });



            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Company).WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_Company");

                entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_Product");
            });

            modelBuilder.Entity<StockDetail>(entity =>
            {
                entity.ToTable("StockDetail");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                //entity.HasOne(d => d.Deliverer).WithMany(p => p.StockDetailDeliverers)
                //    .HasForeignKey(d => d.DelivererId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_StockDetail_User1");

                //entity.HasOne(d => d.Reciever).WithMany(p => p.StockDetailRecievers)
                //    .HasForeignKey(d => d.RecieverId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_StockDetail_User");
                entity.HasOne(e => e.User).WithMany(e => e.StockDetailDeliverers).HasForeignKey(e => e.DelivererId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_StockDetail_User");

                entity.HasOne(d => d.Stock).WithMany(p => p.StockDetails)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockDetail_Stock");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
                
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.UserRoleId).ValueGeneratedOnAdd();
                entity.ToTable("UserRole");
                entity.Property(e => e.AddedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("AddedIP4VAdress");
                entity.Property(e => e.AddedTime).HasColumnType("datetime"); 
                entity.Property(e => e.UpdatedIPV4Address)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("UpdatedIP4VAdress");
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
                entity.Property(e=>e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.HasOne(e => e.Role).WithMany(e => e.UserRoles).HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.User).WithMany(e => e.UserRoles).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.NoAction);

            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
