using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileJsonMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Certifications",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "UserProfiles",
                newName: "Metadata");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "Metadata",
                table: "UserProfiles",
                newName: "Experience");

            migrationBuilder.AddColumn<string>(
                name: "Certifications",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
