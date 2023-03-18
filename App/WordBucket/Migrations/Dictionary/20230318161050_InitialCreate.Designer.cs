﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WordBucket.Contexts;

#nullable disable

namespace WordBucket.Migrations
{
    [DbContext(typeof(DictionaryContext))]
    [Migration("20230318161050_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("WordBucket.Models.CollinsWordFrequency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FrequencyLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Spelling")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Collins");
                });

            modelBuilder.Entity("WordBucket.Models.Dictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Dictionaries");
                });

            modelBuilder.Entity("WordBucket.Models.DictionaryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Definitions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("DictionaryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PhoneticSymbols")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Spelling")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.ToTable("DictionaryEntries");
                });

            modelBuilder.Entity("WordBucket.Models.DictionaryEntry", b =>
                {
                    b.HasOne("WordBucket.Models.Dictionary", "Dictionary")
                        .WithMany("Entries")
                        .HasForeignKey("DictionaryId");

                    b.Navigation("Dictionary");
                });

            modelBuilder.Entity("WordBucket.Models.Dictionary", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
