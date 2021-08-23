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
    [Migration("20210823034727_AddQuotes")]
    partial class AddQuotes
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

                    b.Property<DateTime?>("ConfirmDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ConfirmMs")
                        .HasColumnType("integer");

                    b.Property<bool>("Flag")
                        .HasColumnType("boolean");

                    b.Property<bool>("Flag2")
                        .HasColumnType("boolean");

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

                    b.Property<long?>("UserId1")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserId2")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.HasIndex("UserId2");

                    b.ToTable("AbsItems");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.ChangeAbsWaitItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AbsItemType")
                        .HasColumnType("integer");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ChangeAbsWaitItems");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.FriendItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("FriendId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsConfirm")
                        .HasColumnType("boolean");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.HasIndex("UserId");

                    b.ToTable("FriendItems");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.InputFriendNickname", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("InputFriendNicknames");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.InputMessageToFriend", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("FriendId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.HasIndex("UserId");

                    b.ToTable("InputMessageToFriends");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.MailingItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("FinishedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("TotalUsers")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("MailingItems");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.PollItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("FinishedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Options")
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("Question")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("TotalUsers")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("PollItems");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.Quote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("character varying(3000)");

                    b.HasKey("Id");

                    b.ToTable("Quotes");
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

                    b.Property<long?>("InviterChatId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFriendGotPremium")
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
                        .WithMany("AbsItems")
                        .HasForeignKey("UserId");

                    b.HasOne("TgVozderzhansBot.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId1");

                    b.HasOne("TgVozderzhansBot.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId2");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.ChangeAbsWaitItem", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.FriendItem", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendId");

                    b.HasOne("TgVozderzhansBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.InputFriendNickname", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.InputMessageToFriend", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendId");

                    b.HasOne("TgVozderzhansBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.MailingItem", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.PollItem", b =>
                {
                    b.HasOne("TgVozderzhansBot.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TgVozderzhansBot.Models.User", b =>
                {
                    b.Navigation("AbsItems");
                });
#pragma warning restore 612, 618
        }
    }
}
