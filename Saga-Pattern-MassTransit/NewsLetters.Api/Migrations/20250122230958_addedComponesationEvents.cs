using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsLetters.Api.Migrations
{
    /// <inheritdoc />
    public partial class addedComponesationEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompensating",
                table: "SagaData",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastErrorMessages",
                table: "SagaData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFailureTime",
                table: "SagaData",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "SagaData",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompensating",
                table: "SagaData");

            migrationBuilder.DropColumn(
                name: "LastErrorMessages",
                table: "SagaData");

            migrationBuilder.DropColumn(
                name: "LastFailureTime",
                table: "SagaData");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "SagaData");
        }
    }
}
