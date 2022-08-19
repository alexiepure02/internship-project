﻿// <auto-generated />
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Friends", b =>
                {
                    b.Property<int>("IDUser")
                        .HasColumnType("int");

                    b.Property<int>("IDFriend")
                        .HasColumnType("int");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.HasKey("IDUser", "IDFriend");

                    b.HasIndex("IDFriend");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("Domain.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("DateTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IDReceiver")
                        .HasColumnType("int");

                    b.Property<int>("IDSender")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("IDReceiver");

                    b.HasIndex("IDSender");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Friends", b =>
                {
                    b.HasOne("Domain.User", "Friend")
                        .WithMany("Friends1")
                        .HasForeignKey("IDFriend")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany("MainUserFriends")
                        .HasForeignKey("IDUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Message", b =>
                {
                    b.HasOne("Domain.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("IDReceiver")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "Sender")
                        .WithMany("Messages")
                        .HasForeignKey("IDSender")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Navigation("Friends1");

                    b.Navigation("MainUserFriends");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
