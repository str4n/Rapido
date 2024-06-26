﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Rapido.Services.Wallets.Infrastructure.EF;

#nullable disable

namespace Rapido.Services.Wallets.Infrastructure.EF.Migrations
{
    [DbContext(typeof(WalletsDbContext))]
    [Migration("20240419153834_Rapido_Wallets_Init")]
    partial class Rapido_Wallets_Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Owners.Owner.Owner", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Owner");

                    b.HasDiscriminator<string>("Type").HasValue("Owner");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Wallets.Transfer.Transfer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Metadata")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)");

                    b.Property<Guid?>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("Transfers");

                    b.HasDiscriminator<string>("Type").HasValue("Transfer");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Wallets.Wallet.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId", "Currency")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Owners.Owner.CorporateOwner", b =>
                {
                    b.HasBaseType("Rapido.Services.Wallets.Domain.Owners.Owner.Owner");

                    b.Property<string>("TaxId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("CorporateOwner");
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Owners.Owner.IndividualOwner", b =>
                {
                    b.HasBaseType("Rapido.Services.Wallets.Domain.Owners.Owner.Owner");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("IndividualOwner");
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Wallets.Transfer.IncomingTransfer", b =>
                {
                    b.HasBaseType("Rapido.Services.Wallets.Domain.Wallets.Transfer.Transfer");

                    b.HasDiscriminator().HasValue("IncomingTransfer");
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Wallets.Transfer.OutgoingTransfer", b =>
                {
                    b.HasBaseType("Rapido.Services.Wallets.Domain.Wallets.Transfer.Transfer");

                    b.HasDiscriminator().HasValue("OutgoingTransfer");
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Wallets.Transfer.Transfer", b =>
                {
                    b.HasOne("Rapido.Services.Wallets.Domain.Wallets.Wallet.Wallet", null)
                        .WithMany("Transfers")
                        .HasForeignKey("WalletId");
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Wallets.Wallet.Wallet", b =>
                {
                    b.HasOne("Rapido.Services.Wallets.Domain.Owners.Owner.Owner", null)
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Rapido.Services.Wallets.Domain.Wallets.Wallet.Wallet", b =>
                {
                    b.Navigation("Transfers");
                });
#pragma warning restore 612, 618
        }
    }
}
