﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Rapido.Saga.Persistence;

#nullable disable

namespace Rapido.Saga.Persistence.Migrations
{
    [DbContext(typeof(SagaDbContext))]
    [Migration("20241206021823_Rapido_Saga_RecipientCreatedState")]
    partial class Rapido_Saga_RecipientCreatedState
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Rapido.Saga.Sagas.AccountSetUpSagaData", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<bool>("AccountSetUpCompleted")
                        .HasColumnType("boolean");

                    b.Property<string>("AccountType")
                        .HasColumnType("text");

                    b.Property<string>("CurrentState")
                        .HasColumnType("text");

                    b.Property<bool>("CustomerCompleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("CustomerCreated")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("OwnerCreated")
                        .HasColumnType("boolean");

                    b.Property<bool>("RecipientCreated")
                        .HasColumnType("boolean");

                    b.Property<bool>("UserActivated")
                        .HasColumnType("boolean");

                    b.Property<bool>("WalletCreated")
                        .HasColumnType("boolean");

                    b.HasKey("CorrelationId");

                    b.ToTable("AccountSetUpSagaData");
                });
#pragma warning restore 612, 618
        }
    }
}