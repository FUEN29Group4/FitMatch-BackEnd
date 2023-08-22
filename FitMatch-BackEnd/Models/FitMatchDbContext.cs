using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FitMatch_BackEnd.Models;

public partial class FitMatchDbContext : DbContext
{
    public FitMatchDbContext()
    {
    }

    public FitMatchDbContext(DbContextOptions<FitMatchDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassType> ClassTypes { get; set; }

    public virtual DbSet<CustomerService> CustomerServices { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Gym> Gyms { get; set; }

    public virtual DbSet<Member> Members { get; set; }  

    public virtual DbSet<MemberFavorite> MemberFavorites { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Robot> Robots { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }
   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=fuen29g04.database.windows.net;Initial Catalog=FitMatchDB;Persist Security Info=True;User ID=fuen29g04;Password=FUEN29gg04");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK_News");

            entity.ToTable("Article");

            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.ArticleTypeName).HasMaxLength(100);
            entity.Property(e => e.ContentText).HasMaxLength(1000);
            entity.Property(e => e.Photo).HasMaxLength(1000);
            entity.Property(e => e.PublicationDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK_course_information_1");

            entity.ToTable("Class");

            entity.Property(e => e.ClassId)
                .ValueGeneratedNever()
                .HasColumnName("ClassID");
            entity.Property(e => e.ClassTypeId).HasColumnName("ClassTypeID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.GymId).HasColumnName("GymID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");

            entity.HasOne(d => d.ClassType).WithMany(p => p.Classes)
                .HasForeignKey(d => d.ClassTypeId)
                .HasConstraintName("FK_Class_ClassTypes");

            entity.HasOne(d => d.TrainerID).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("FK_Course_Information_Trainers");
        });

        modelBuilder.Entity<ClassType>(entity =>
        {
            entity.HasKey(e => e.ClassTypeId).HasName("PK_course");

            entity.Property(e => e.ClassTypeId).HasColumnName("ClassTypeID");
            entity.Property(e => e.ClassName).HasMaxLength(100);
            entity.Property(e => e.Introduction).HasMaxLength(1000);
        });

        modelBuilder.Entity<CustomerService>(entity =>
        {
            entity.ToTable("CustomerService");

            entity.Property(e => e.CustomerServiceId)
                .ValueGeneratedNever()
                .HasColumnName("CustomerServiceID");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_employee");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Birth).HasColumnType("date");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployeeName).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.Photo).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(30);
        });

        modelBuilder.Entity<Gym>(entity =>
        {
            entity.Property(e => e.GymId)
                  .ValueGeneratedOnAdd()  // 這裡是關鍵的改動
                  .HasColumnName("GymID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.GymName).HasMaxLength(50);
            entity.Property(e => e.OpentimeEnd).HasColumnType("datetime");
            entity.Property(e => e.OpentimeStart).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(30);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Birth).HasColumnType("date");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MemberName).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.Photo).HasMaxLength(50);
        });

        modelBuilder.Entity<MemberFavorite>(entity =>
        {
            entity.HasKey(e => e.MemberFavoriteId).HasName("PK_member_detail");

            entity.ToTable("MemberFavorite");

            entity.Property(e => e.MemberFavoriteId)
                .ValueGeneratedNever()
                .HasColumnName("MemberFavoriteID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");

            entity.HasOne(d => d.Class).WithMany(p => p.MemberFavorites)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_MemberFavorite_Class");

            entity.HasOne(d => d.ClassNavigation).WithMany(p => p.MemberFavorites)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Member_Favorite_Member");

            entity.HasOne(d => d.Product).WithMany(p => p.MemberFavorites)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Member_Favorite_Product");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.ShippingMethod).HasMaxLength(50);

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Order_Member");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK_orderdetail");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId)
                .ValueGeneratedNever()
                .HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Orderdetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Orderdetail_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_product");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Photo).HasMaxLength(50);
            entity.Property(e => e.ProductDescription).HasMaxLength(1000);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Product_ProductType");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK_productlist");

            entity.ToTable("ProductType");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantsId).HasName("PK_Meals");

            entity.Property(e => e.RestaurantsId).HasColumnName("RestaurantsID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.RestaurantsDescription).HasMaxLength(1000);
            entity.Property(e => e.RestaurantsName).HasMaxLength(50);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.Property(e => e.ReviewId)
                .ValueGeneratedNever()
                .HasColumnName("ReviewID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ReviewDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Member).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Reviews_Trainers");
        });

        modelBuilder.Entity<Robot>(entity =>
        {
            entity.ToTable("Robot");

            entity.Property(e => e.RobotId)
                .ValueGeneratedNever()
                .HasColumnName("RobotID");
            entity.Property(e => e.DefaultQuestion).HasMaxLength(1000);
            entity.Property(e => e.DefaultResponse).HasMaxLength(1000);
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Birth).HasColumnType("date");
            entity.Property(e => e.Certificate).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Experience).HasMaxLength(100);
            entity.Property(e => e.Expertise).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.Photo).HasMaxLength(50);
            entity.Property(e => e.TrainerName).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
