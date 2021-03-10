﻿// <auto-generated />
using System;
using Inlamn2WebbShop_MLarsson.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inlamn2WebbShop_MLarsson.Migrations
{
    [DbContext(typeof(WebbShopContext))]
    [Migration("20210308135014_AddedDatabaseName")]
    partial class AddedDatabaseName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SoldBookId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("SoldBookId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.SoldBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<DateTime>("PurchasedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SoldBooks");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SessionTimer")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.Category", b =>
                {
                    b.HasOne("Inlamn2WebbShop_MLarsson.Models.Book", null)
                        .WithMany("Categories")
                        .HasForeignKey("BookId");

                    b.HasOne("Inlamn2WebbShop_MLarsson.Models.SoldBook", null)
                        .WithMany("Categories")
                        .HasForeignKey("SoldBookId");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.User", b =>
                {
                    b.HasOne("Inlamn2WebbShop_MLarsson.Models.Category", null)
                        .WithMany("Users")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.Book", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.Category", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Inlamn2WebbShop_MLarsson.Models.SoldBook", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}