﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StackUnderflow.Data;

namespace StackUnderflow.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StackUnderflow.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<int>("Popularity");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("StackUnderflow.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("StackUnderflow.Entities.QuestionResponses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("QuestionId");

                    b.Property<int>("ResponseId");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("ResponseId");

                    b.ToTable("QuestionResponses");
                });

            modelBuilder.Entity("StackUnderflow.Entities.Response", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<bool>("IsSolution");

                    b.Property<int>("Popularity");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("StackUnderflow.Entities.ResponseComments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommentId");

                    b.Property<int>("ResponseId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("ResponseId");

                    b.ToTable("ResponseComments");
                });

            modelBuilder.Entity("StackUnderflow.Entities.QuestionResponses", b =>
                {
                    b.HasOne("StackUnderflow.Entities.Question", "Question")
                        .WithMany("QuestionResponses")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StackUnderflow.Entities.Response", "Response")
                        .WithMany()
                        .HasForeignKey("ResponseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StackUnderflow.Entities.ResponseComments", b =>
                {
                    b.HasOne("StackUnderflow.Entities.Comment", "Comment")
                        .WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StackUnderflow.Entities.Response", "Response")
                        .WithMany("ResponseComments")
                        .HasForeignKey("ResponseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
