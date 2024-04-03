﻿// <auto-generated />
using System;
using DemoSpiritsAPI.EntiryFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    [DbContext(typeof(MySQLContext))]
    partial class MySQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HabitatSpirit", b =>
                {
                    b.Property<int>("HabitatsId")
                        .HasColumnType("int");

                    b.Property<int>("SpiritsId")
                        .HasColumnType("int");

                    b.HasKey("HabitatsId", "SpiritsId");

                    b.HasIndex("SpiritsId");

                    b.ToTable("HabitatSpirit");
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.BorderPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("HabitatId")
                        .HasColumnType("int");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("HabitatId");

                    b.ToTable("BorderPoints");
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.Habitat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Habitats");
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.MarkerPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("SpiritId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpiritId")
                        .IsUnique();

                    b.ToTable("MarkerPoints");
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.Spirit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("CardImage")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("CardImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Classification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("MarkerImage")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("MarkerImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Spirits");
                });

            modelBuilder.Entity("HabitatSpirit", b =>
                {
                    b.HasOne("SpiritsClassLibrary.Models.Habitat", null)
                        .WithMany()
                        .HasForeignKey("HabitatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpiritsClassLibrary.Models.Spirit", null)
                        .WithMany()
                        .HasForeignKey("SpiritsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.BorderPoint", b =>
                {
                    b.HasOne("SpiritsClassLibrary.Models.Habitat", "Habitat")
                        .WithMany("Border")
                        .HasForeignKey("HabitatId");

                    b.Navigation("Habitat");
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.MarkerPoint", b =>
                {
                    b.HasOne("SpiritsClassLibrary.Models.Spirit", "spirit")
                        .WithOne("MarkerLocation")
                        .HasForeignKey("SpiritsClassLibrary.Models.MarkerPoint", "SpiritId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("spirit");
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.Habitat", b =>
                {
                    b.Navigation("Border");
                });

            modelBuilder.Entity("SpiritsClassLibrary.Models.Spirit", b =>
                {
                    b.Navigation("MarkerLocation");
                });
#pragma warning restore 612, 618
        }
    }
}
