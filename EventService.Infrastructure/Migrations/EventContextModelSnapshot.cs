﻿// <auto-generated />
using System;
using EventService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventService.Infrastructure.Migrations
{
    [DbContext(typeof(EventContext))]
    partial class EventContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("RedeemVoucherCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.HasKey("Id")
                        .HasName("event_pkey");

                    b.HasIndex("BrandId");

                    b.HasIndex("GameId");

                    b.ToTable("event", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventAggregate.EventPlayer", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.HasKey("EventId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("event_player", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventAggregate.EventVoucher", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("VoucherId")
                        .HasColumnType("integer");

                    b.HasKey("EventId", "VoucherId");

                    b.HasIndex("VoucherId");

                    b.ToTable("event_voucher", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.GameAggregate.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("game_pkey");

                    b.ToTable("game", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.PlayerAggregate.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime>("LastAccessed")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_accessed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("player_pkey");

                    b.ToTable("player", (string)null);
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.VoucherAggregate.RedeemVoucher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BaseVoucherId")
                        .HasColumnType("integer");

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expire_date");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<string>("RedeemCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<DateTime>("RedeemTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("redeem_time");

                    b.HasKey("Id")
                        .HasName("redeem_voucher_pkey");

                    b.HasIndex("BaseVoucherId");

                    b.HasIndex("EventId");

                    b.HasIndex("PlayerId");

                    b.ToTable("redeem_voucher", (string)null);
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

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

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

                    b.HasOne("EventService.Domain.AggregateModels.GameAggregate.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.Navigation("Brand");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventAggregate.EventPlayer", b =>
                {
                    b.HasOne("EventService.Domain.AggregateModels.EventAggregate.Event", "Event")
                        .WithMany("Players")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventService.Domain.AggregateModels.PlayerAggregate.Player", "Player")
                        .WithMany("Events")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.EventAggregate.EventVoucher", b =>
                {
                    b.HasOne("EventService.Domain.AggregateModels.EventAggregate.Event", "Event")
                        .WithMany("Vouchers")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventService.Domain.AggregateModels.VoucherAggregate.Voucher", "Voucher")
                        .WithMany()
                        .HasForeignKey("VoucherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.VoucherAggregate.RedeemVoucher", b =>
                {
                    b.HasOne("EventService.Domain.AggregateModels.VoucherAggregate.Voucher", "BaseVoucher")
                        .WithMany()
                        .HasForeignKey("BaseVoucherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventService.Domain.AggregateModels.EventAggregate.Event", "Event")
                        .WithMany("RedeemVouchers")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventService.Domain.AggregateModels.PlayerAggregate.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseVoucher");

                    b.Navigation("Event");

                    b.Navigation("Player");
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
                    b.Navigation("Players");

                    b.Navigation("RedeemVouchers");

                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("EventService.Domain.AggregateModels.PlayerAggregate.Player", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
