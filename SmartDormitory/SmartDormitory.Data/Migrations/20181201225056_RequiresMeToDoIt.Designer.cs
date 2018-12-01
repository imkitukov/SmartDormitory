﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartDormitory.App.Data;

namespace SmartDormitory.Data.Migrations
{
    [DbContext(typeof(SmartDormitoryContext))]
    [Migration("20181201225056_RequiresMeToDoIt")]
    partial class RequiresMeToDoIt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SmartDormitory.Data.Models.IcbSensor", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<float>("CurrentValue");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdateOn");

                    b.Property<float>("MaxRangeValue");

                    b.Property<string>("MeasureTypeId");

                    b.Property<float>("MinRangeValue");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("PollingInterval");

                    b.Property<string>("Tag")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MeasureTypeId");

                    b.ToTable("IcbSensors");
                });

            modelBuilder.Entity("SmartDormitory.Data.Models.MeasureType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("MeasureUnit");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("SuitableSensorType");

                    b.HasKey("Id");

                    b.ToTable("MeasureTypes");

                    b.HasData(
                        new { Id = "b5ed44ed-a38d-4dc8-ad8a-796e42d018da", CreatedOn = new DateTime(2018, 12, 2, 0, 50, 56, 188, DateTimeKind.Local), IsDeleted = false, MeasureUnit = "°C", SuitableSensorType = "Temperature" },
                        new { Id = "d80a9fa0-36db-4565-b4b2-e17e46887127", CreatedOn = new DateTime(2018, 12, 2, 0, 50, 56, 190, DateTimeKind.Local), IsDeleted = false, MeasureUnit = "%", SuitableSensorType = "Humidity" },
                        new { Id = "d60b36ae-20e9-4b7b-afc5-f4fead61cce7", CreatedOn = new DateTime(2018, 12, 2, 0, 50, 56, 190, DateTimeKind.Local), IsDeleted = false, MeasureUnit = "W", SuitableSensorType = "Electric power consumtion" },
                        new { Id = "c81a1be2-dec4-47ce-b4d9-1000491b843f", CreatedOn = new DateTime(2018, 12, 2, 0, 50, 56, 190, DateTimeKind.Local), IsDeleted = false, MeasureUnit = "(true/false)", SuitableSensorType = "Boolean switch (door/occupancy/etc)" },
                        new { Id = "6e6af1b4-2d82-4381-a5cf-03aed3f7664e", CreatedOn = new DateTime(2018, 12, 2, 0, 50, 56, 190, DateTimeKind.Local), IsDeleted = false, MeasureUnit = "dB", SuitableSensorType = "Noise" }
                    );
                });

            modelBuilder.Entity("SmartDormitory.Data.Models.Sensor", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("AlarmMaxRangeValue");

                    b.Property<float>("AlarmMinRangeValue");

                    b.Property<bool>("AlarmOn");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("IcbSensorId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPublic");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("OwnerId");

                    b.Property<int>("UserPollingInterval");

                    b.HasKey("Id");

                    b.HasIndex("IcbSensorId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("SmartDormitory.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SmartDormitory.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SmartDormitory.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartDormitory.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SmartDormitory.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartDormitory.Data.Models.IcbSensor", b =>
                {
                    b.HasOne("SmartDormitory.Data.Models.MeasureType", "MeasureType")
                        .WithMany("IcbSensors")
                        .HasForeignKey("MeasureTypeId");
                });

            modelBuilder.Entity("SmartDormitory.Data.Models.Sensor", b =>
                {
                    b.HasOne("SmartDormitory.Data.Models.IcbSensor", "IcbSensor")
                        .WithMany("Sensors")
                        .HasForeignKey("IcbSensorId");

                    b.HasOne("SmartDormitory.Data.Models.User", "Owner")
                        .WithMany("Sensors")
                        .HasForeignKey("OwnerId");

                    b.OwnsOne("SmartDormitory.Data.Models.Coordinates", "Coordinates", b1 =>
                        {
                            b1.Property<string>("SensorId");

                            b1.Property<double>("Latitude")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnName("Longitude");

                            b1.ToTable("Sensors");

                            b1.HasOne("SmartDormitory.Data.Models.Sensor")
                                .WithOne("Coordinates")
                                .HasForeignKey("SmartDormitory.Data.Models.Coordinates", "SensorId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
