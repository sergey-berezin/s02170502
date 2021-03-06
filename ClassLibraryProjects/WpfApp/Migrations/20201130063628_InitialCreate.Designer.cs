﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpfApp;

namespace WpfApp.Migrations
{
    [DbContext(typeof(ModelContext))]
    [Migration("20201130063628_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("WpfApp.Blob", b =>
                {
                    b.Property<int>("BlobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ImageContext")
                        .HasColumnType("BLOB");

                    b.HasKey("BlobId");

                    b.ToTable("ImageContext");
                });

            modelBuilder.Entity("WpfApp.ClassLabel", b =>
                {
                    b.Property<int>("ClassLabelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClassLabelImagesNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StringClassLabel")
                        .HasColumnType("TEXT");

                    b.HasKey("ClassLabelId");

                    b.ToTable("ClassLabels");
                });

            modelBuilder.Entity("WpfApp.ImageInformation", b =>
                {
                    b.Property<int>("ImageInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ClassLabelId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ImageContextBlobId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<float>("Probability")
                        .HasColumnType("REAL");

                    b.HasKey("ImageInformationId");

                    b.HasIndex("ClassLabelId");

                    b.HasIndex("ImageContextBlobId");

                    b.ToTable("ImagesInformation");
                });

            modelBuilder.Entity("WpfApp.ImageInformation", b =>
                {
                    b.HasOne("WpfApp.ClassLabel", "ClassLabel")
                        .WithMany("ImagesInformation")
                        .HasForeignKey("ClassLabelId");

                    b.HasOne("WpfApp.Blob", "ImageContext")
                        .WithMany()
                        .HasForeignKey("ImageContextBlobId");

                    b.Navigation("ClassLabel");

                    b.Navigation("ImageContext");
                });

            modelBuilder.Entity("WpfApp.ClassLabel", b =>
                {
                    b.Navigation("ImagesInformation");
                });
#pragma warning restore 612, 618
        }
    }
}
