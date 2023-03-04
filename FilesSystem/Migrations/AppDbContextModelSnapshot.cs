﻿// <auto-generated />
using System;
using FilesSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FilesSystem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FilesSystem.Models.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Folders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Creating Digital Images",
                            Path = ""
                        },
                        new
                        {
                            Id = 2,
                            Name = "Resources",
                            ParentId = 1,
                            Path = ""
                        },
                        new
                        {
                            Id = 3,
                            Name = "Primary Sources",
                            ParentId = 2,
                            Path = ""
                        },
                        new
                        {
                            Id = 4,
                            Name = "Secondary Sources",
                            ParentId = 2,
                            Path = ""
                        },
                        new
                        {
                            Id = 5,
                            Name = "Evidence",
                            ParentId = 1,
                            Path = ""
                        },
                        new
                        {
                            Id = 6,
                            Name = "Graphic Products",
                            ParentId = 1,
                            Path = ""
                        },
                        new
                        {
                            Id = 7,
                            Name = "Process",
                            ParentId = 6,
                            Path = ""
                        },
                        new
                        {
                            Id = 8,
                            Name = "Final Product",
                            ParentId = 6,
                            Path = ""
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
