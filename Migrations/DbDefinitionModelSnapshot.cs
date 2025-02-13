﻿// <auto-generated />
using File_Processor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace File_Processor.Migrations
{
    [DbContext(typeof(DbDefinition))]
    partial class DbDefinitionModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("File_Processor.Models.CategoryClassificationModel", b =>
                {
                    b.Property<string>("category")
                        .HasColumnType("TEXT");

                    b.Property<string>("pattern")
                        .HasColumnType("TEXT");

                    b.Property<int>("type")
                        .HasColumnType("INTEGER");

                    b.HasKey("category", "pattern");

                    b.ToTable("CategoriesClassification");
                });

            modelBuilder.Entity("File_Processor.Models.CategoryModel", b =>
                {
                    b.Property<string>("category")
                        .HasColumnType("TEXT");

                    b.Property<string>("filePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("parentCategory")
                        .HasColumnType("TEXT");

                    b.Property<string>("subCategory")
                        .HasColumnType("TEXT");

                    b.HasKey("category");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("File_Processor.Models.CategoryClassificationModel", b =>
                {
                    b.HasOne("File_Processor.Models.CategoryModel", "categoryModel")
                        .WithMany("classifications")
                        .HasForeignKey("category")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("categoryModel");
                });

            modelBuilder.Entity("File_Processor.Models.CategoryModel", b =>
                {
                    b.Navigation("classifications");
                });
#pragma warning restore 612, 618
        }
    }
}
