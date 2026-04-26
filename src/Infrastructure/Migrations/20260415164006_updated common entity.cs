using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedcommonentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "GalleryItems");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "GalleryItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "GalleryItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "GalleryItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedOn",
                table: "GalleryItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "GalleryItems");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "GalleryItems",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "GalleryItems",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
