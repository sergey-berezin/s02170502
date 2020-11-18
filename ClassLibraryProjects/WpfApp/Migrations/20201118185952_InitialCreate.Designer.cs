﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpfApp;

namespace WpfApp.Migrations
{
    [DbContext(typeof(ModelContext))]
    [Migration("20201118185952_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("WpfApp.ImageInformation", b =>
                {
                    b.Property<int>("ImageInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Confidence")
                        .HasColumnType("REAL");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.HasKey("ImageInformationId");

                    b.ToTable("ImagesInformation");
                });
#pragma warning restore 612, 618
        }
    }
}
