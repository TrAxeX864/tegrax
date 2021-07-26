﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TgVozderzhansBot.Database;

namespace TgVozderzhansBot.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210707035758_JX1")]
    partial class JX1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("TgVozderzhansBot.Models.AbsGuardItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long?>("AbsItemId")
                        .HasColumnType("bigint");

                    b.Property<int>("ConfirmMs")
                        .HasColumnType("integer");

                    b.Property<bool>("IsConfirm")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastNotify")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AbsItemId");

                    b.HasIndex("UserId");

                    b.ToTable("AbsGuardItems");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.AbsItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AbsItemType")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Finished")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Started")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AbsItems");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PremiumActiveTo")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("PublicAccount")
                        .HasColumnType("boolean");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.AbsGuardItem", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.AbsItem", "AbsItem")
                        .WithMany()
                        .HasForeignKey("AbsItemId");

                    b.HasOne("TgVozderzhansBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("AbsItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.AbsItem", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
