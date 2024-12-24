﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wise.BlobStorage.Infrastructure.Context;

#nullable disable

namespace Wise.BlobStorage.Infrastructure.Migrations
{
    [DbContext(typeof(WiseDbContext))]
    [Migration("20241224200703_addBlobTypeColumnInBlobsTable")]
    partial class addBlobTypeColumnInBlobsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Wise.BlobStorage.Domain.Entities.Blob", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ActionUser")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnOrder(102);

                    b.Property<string>("BlobName")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnOrder(3);

                    b.Property<int>("BlobType")
                        .HasColumnType("int")
                        .HasColumnOrder(5);

                    b.Property<string>("ContainerName")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedDate")
                        .HasColumnType("bigint")
                        .HasColumnOrder(103);

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)")
                        .HasColumnOrder(4);

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnOrder(105);

                    b.Property<long?>("ModifiedDate")
                        .HasColumnType("bigint")
                        .HasColumnOrder(104);

                    b.Property<DateTime>("RecordDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnOrder(100)
                        .HasDefaultValueSql("getdate()");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnOrder(101);

                    b.HasKey("Id");

                    b.ToTable("Blobs", null, t =>
                        {
                            t.HasTrigger("Blobs_DEL");

                            t.HasTrigger("Blobs_INS");

                            t.HasTrigger("Blobs_UPD");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });
#pragma warning restore 612, 618
        }
    }
}
