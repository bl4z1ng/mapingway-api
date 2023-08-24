﻿// <auto-generated />
using System;
using Mapingway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mapingway.Infrastructure.Persistence.Migrations.PostgresqlMigrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230803083417_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mapingway.Domain.Auth.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permissions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "ReadUser"
                        },
                        new
                        {
                            Id = 2,
                            Name = "UpdateUser"
                        },
                        new
                        {
                            Id = 3,
                            Name = "DeleteUser"
                        });
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.RefreshToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<long?>("TokenFamilyId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("Value");

                    b.HasIndex("TokenFamilyId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.RefreshTokenFamily", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RefreshTokenFamilies", (string)null);

                    b.HasData(
                        new
                        {
                            Id = -1L,
                            UserId = -1L
                        });
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "User"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermissionId = 1
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 1
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 2
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 3
                        });
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.UserRole", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            UserId = -1L,
                            RoleId = 2
                        });
                });

            modelBuilder.Entity("Mapingway.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Created")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("text");

                    b.Property<string>("Updated")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = -1L,
                            Email = "admin.map@rambler.ru",
                            FirstName = "Admin",
                            LastName = "Super",
                            PasswordHash = "ODrNkGKssc+CWOvKQhJAQQNMocAsUaJ73pBaIfIufy4=",
                            PasswordSalt = "u4ya35ZFIvfkqC+ObHlNFQ=="
                        });
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.RefreshToken", b =>
                {
                    b.HasOne("Mapingway.Domain.Auth.RefreshTokenFamily", "TokenFamily")
                        .WithMany("Tokens")
                        .HasForeignKey("TokenFamilyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mapingway.Domain.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("Mapingway.Domain.Auth.RefreshToken", "UserId");

                    b.Navigation("TokenFamily");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.RefreshTokenFamily", b =>
                {
                    b.HasOne("Mapingway.Domain.User", "User")
                        .WithOne("UsedRefreshTokensFamily")
                        .HasForeignKey("Mapingway.Domain.Auth.RefreshTokenFamily", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.RolePermission", b =>
                {
                    b.HasOne("Mapingway.Domain.Auth.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mapingway.Domain.Auth.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.UserRole", b =>
                {
                    b.HasOne("Mapingway.Domain.Auth.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mapingway.Domain.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.RefreshTokenFamily", b =>
                {
                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("Mapingway.Domain.Auth.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Mapingway.Domain.User", b =>
                {
                    b.Navigation("RefreshToken");

                    b.Navigation("UsedRefreshTokensFamily")
                        .IsRequired();

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
