﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using VanderStack.HealthCatalystPeopleSearch.PersonFeature;

namespace VanderStack.HealthCatalystPeopleSearch.Migrations
{
    [DbContext(typeof(PersonDatabaseContext))]
    partial class PersonDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VanderStack.HealthCatalystPeopleSearch.PersonFeature.InterestModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PersonId");

                    b.Property<Guid?>("PersonModelId");

                    b.Property<string>("Summary");

                    b.HasKey("Id");

                    b.HasIndex("PersonModelId");

                    b.ToTable("InterestModel");
                });

            modelBuilder.Entity("VanderStack.HealthCatalystPeopleSearch.PersonFeature.PersonModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int>("Age");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PhotoUrl");

                    b.HasKey("Id");

                    b.ToTable("PersonSet");
                });

            modelBuilder.Entity("VanderStack.HealthCatalystPeopleSearch.PersonFeature.InterestModel", b =>
                {
                    b.HasOne("VanderStack.HealthCatalystPeopleSearch.PersonFeature.PersonModel")
                        .WithMany("InterestSet")
                        .HasForeignKey("PersonModelId");
                });
#pragma warning restore 612, 618
        }
    }
}
