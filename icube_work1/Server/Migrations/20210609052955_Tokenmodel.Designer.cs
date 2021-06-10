﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server.Data;

namespace Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210609052955_Tokenmodel")]
    partial class Tokenmodel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "en_US.utf8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Server.Models.Factory", b =>
                {
                    b.Property<int>("FactoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("factory_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Factoryname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("factoryname");

                    b.HasKey("FactoryId");

                    b.ToTable("factorys");
                });

            modelBuilder.Entity("Server.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ExpiresTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Server.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("role_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool?>("IntegrationService")
                        .HasColumnType("boolean")
                        .HasColumnName("integration_service");

                    b.Property<bool?>("InterfaceService")
                        .HasColumnType("boolean")
                        .HasColumnName("interface_service");

                    b.Property<string>("Rolename")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("rolename");

                    b.Property<bool?>("TagService")
                        .HasColumnType("boolean")
                        .HasColumnName("tag_service");

                    b.HasKey("RoleId");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("FactoryId")
                        .HasColumnType("integer")
                        .HasColumnName("factory_id");

                    b.Property<byte[]>("Passwordhash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("passwordhash");

                    b.Property<byte[]>("Passwordsalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("passwordsalt");

                    b.Property<int?>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("FactoryId");

                    b.HasIndex("RoleId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Server.Models.RefreshToken", b =>
                {
                    b.HasOne("Server.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.HasOne("Server.Models.Factory", "Factory")
                        .WithMany("Users")
                        .HasForeignKey("FactoryId")
                        .HasConstraintName("factorys_fk");

                    b.HasOne("Server.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("users_fk");

                    b.Navigation("Factory");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Server.Models.Factory", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Server.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
