﻿// <auto-generated />
using System;
using EventService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventService.Infrastructure.Migrations
{
    [DbContext(typeof(EventContext))]
    [Migration("20240811092656_rename")]
    partial class rename
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventService.Domain.AggregateModels.BrandAggregate.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Field")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("field");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("brand_pkey");

                    b.ToTable("brand", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventAggregate.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BrandId")
                        .HasColumnType("integer")
                        .HasColumnName("brand_id");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<int?>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("NoVoucher")
                        .HasColumnType("integer")
                        .HasColumnName("no_voucher");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.HasKey("Id")
                        .HasName("event_pkey");

                    b.HasIndex("BrandId");

                    b.ToTable("event", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventVoucher", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("VoucherId")
                        .HasColumnType("integer");

                    b.HasKey("EventId", "VoucherId");

                    b.HasIndex("VoucherId");

                    b.ToTable("brand_voucher", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.VoucherAggregate.Voucher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BrandId")
                        .HasColumnType("integer")
                        .HasColumnName("brand_id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("ExpireDate")
                        .HasColumnType("integer")
                        .HasColumnName("expire_date");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("voucher_pkey");

                    b.HasIndex("BrandId");

                    b.ToTable("voucher", (string)null);
                });

            modelBuilder.Entity("EventService.Infrastructure.Idempotency.ClientRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("requests", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.BrandAggregate.Brand", b =>
                {
                    b.OwnsOne("EventService.Domain.AggregateModels.BrandAggregate.Location", "Location", b1 =>
                        {
                            b1.Property<int>("BrandId")
                                .HasColumnType("integer");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address");

                            b1.Property<string>("Gps")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("gps");

                            b1.HasKey("BrandId");

                            b1.ToTable("brand");

                            b1.WithOwner()
                                .HasForeignKey("BrandId");
                        });

                    b.Navigation("Location")
                        .IsRequired();
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventAggregate.Event", b =>
                {
                    b.HasOne("EventService.Domain.AggregateModels.BrandAggregate.Brand", "Brand")
                        .WithMany("Events")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventVoucher", b =>
                {
                    b.HasOne("EventService.Domain.AggregateModels.EventAggregate.Event", "Event")
                        .WithMany("Vouchers")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventService.Domain.AggregateModels.VoucherAggregate.Voucher", "Voucher")
                        .WithMany("Events")
                        .HasForeignKey("VoucherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.VoucherAggregate.Voucher", b =>
                {
                    b.HasOne("EventService.Domain.AggregateModels.BrandAggregate.Brand", "Brand")
                        .WithMany("Vouchers")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.BrandAggregate.Brand", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventAggregate.Event", b =>
                {
                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.VoucherAggregate.Voucher", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
