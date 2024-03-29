﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WordBucket.Contexts;

#nullable disable

namespace WordBucket.Migrations.User
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("CorpusLearningWord", b =>
                {
                    b.Property<int>("CorpusesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LearningWordsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CorpusesId", "LearningWordsId");

                    b.HasIndex("LearningWordsId");

                    b.ToTable("CorpusLearningWord");
                });

            modelBuilder.Entity("WordBucket.Models.Corpus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Corpuses");
                });

            modelBuilder.Entity("WordBucket.Models.LearningWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Definitions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastVisit")
                        .HasColumnType("TEXT");

                    b.Property<int>("Progress")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Spelling")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LearningWords");
                });

            modelBuilder.Entity("CorpusLearningWord", b =>
                {
                    b.HasOne("WordBucket.Models.Corpus", null)
                        .WithMany()
                        .HasForeignKey("CorpusesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WordBucket.Models.LearningWord", null)
                        .WithMany()
                        .HasForeignKey("LearningWordsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
