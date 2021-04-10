﻿// <auto-generated />
using System;
using Infracstructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infracstructure.Migrations
{
    [DbContext(typeof(PTContext))]
    [Migration("20210410205919_CrearTablas")]
    partial class CrearTablas
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Infracstructure.Entities.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("firstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("libroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("libroId");

                    b.ToTable("Autor");
                });

            modelBuilder.Entity("Infracstructure.Entities.Libro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("excerpt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("pageCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("publishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Libro");
                });

            modelBuilder.Entity("Infracstructure.Entities.Autor", b =>
                {
                    b.HasOne("Infracstructure.Entities.Libro", "libro")
                        .WithMany()
                        .HasForeignKey("libroId");

                    b.Navigation("libro");
                });
#pragma warning restore 612, 618
        }
    }
}
