﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RR.DiceDuel.ExternalServices.EntityFramework;

#nullable disable

namespace RR.DiceDuel.ExternalServices.EntityFramework.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RR.DiceDuel.ExternalServices.EntityFramework.Entities.ConfigEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MaxGameRound")
                        .HasColumnType("integer");

                    b.Property<int>("RoomMaxPlayer")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Config");
                });

            modelBuilder.Entity("RR.DiceDuel.ExternalServices.EntityFramework.Entities.StatisticEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("Defeats")
                        .HasColumnType("bigint");

                    b.Property<long>("Draw")
                        .HasColumnType("bigint");

                    b.Property<long>("GamesCount")
                        .HasColumnType("bigint");

                    b.Property<long>("GotMaxScore")
                        .HasColumnType("bigint");

                    b.Property<long>("GotZeroScore")
                        .HasColumnType("bigint");

                    b.Property<long>("NormalRolled")
                        .HasColumnType("bigint");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<long>("SpecialRolled")
                        .HasColumnType("bigint");

                    b.Property<long>("TotalScores")
                        .HasColumnType("bigint");

                    b.Property<long>("Wins")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("RR.DiceDuel.ExternalServices.EntityFramework.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
