﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SerenBot.Entities;

namespace SerenBot.Migrations
{
    [DbContext(typeof(SerenDbContext))]
    [Migration("20181215103808_Changed NotificationChannel Type")]
    partial class ChangedNotificationChannelType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SerenBot.Entities.Models.Guild", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GuildName");

                    b.Property<ulong>("NotificationChannel");

                    b.Property<string>("Prefix")
                        .HasMaxLength(4);

                    b.HasKey("GuildId");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("SerenBot.Entities.Models.NotificationUser", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("NotificationValue");

                    b.HasKey("UserId");

                    b.ToTable("NotificationUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
