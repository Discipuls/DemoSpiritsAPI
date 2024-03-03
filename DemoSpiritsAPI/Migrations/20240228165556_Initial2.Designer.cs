﻿// <auto-generated />
using System;
using DemoSpiritsAPI.EntiryFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    [DbContext(typeof(MySQLContext))]
    [Migration("20240228165556_Initial2")]
    partial class Initial2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DemoSpiritsAPI.Models.GeoPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("HabitatId")
                        .HasColumnType("int");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("HabitatId");

                    b.ToTable("GeoPoints");
                });

            modelBuilder.Entity("DemoSpiritsAPI.Models.Habitat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("MarkerLocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("MarkerLocationId");

                    b.ToTable("Habitats");
                });

            modelBuilder.Entity("DemoSpiritsAPI.Models.Spirit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CardImageName")
                        .HasColumnType("longtext");

                    b.Property<string>("Classification")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MarkerImageName")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Spirits");
                });

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

            modelBuilder.Entity("DemoSpiritsAPI.Models.GeoPoint", b =>
                {
                    b.HasOne("DemoSpiritsAPI.Models.Habitat", null)
                        .WithMany("Border")
                        .HasForeignKey("HabitatId");
                });

            modelBuilder.Entity("DemoSpiritsAPI.Models.Habitat", b =>
                {
                    b.HasOne("DemoSpiritsAPI.Models.GeoPoint", "MarkerLocation")
                        .WithMany()
                        .HasForeignKey("MarkerLocationId");

                    b.Navigation("MarkerLocation");
                });

            modelBuilder.Entity("HabitatSpirit", b =>
                {
                    b.HasOne("DemoSpiritsAPI.Models.Habitat", null)
                        .WithMany()
                        .HasForeignKey("HabitatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DemoSpiritsAPI.Models.Spirit", null)
                        .WithMany()
                        .HasForeignKey("SpiritsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DemoSpiritsAPI.Models.Habitat", b =>
                {
                    b.Navigation("Border");
                });
#pragma warning restore 612, 618
        }
    }
}
