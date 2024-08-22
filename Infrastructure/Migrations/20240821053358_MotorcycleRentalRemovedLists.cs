using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MotorcycleRentalRemovedLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_Motorcycles_MotorcycleModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_Rentals_RentalModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_MotorcycleModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_RentalModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropColumn(
                name: "MotorcycleModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropColumn(
                name: "RentalModelId",
                table: "MotorcycleRentals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MotorcycleModelId",
                table: "MotorcycleRentals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RentalModelId",
                table: "MotorcycleRentals",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_MotorcycleModelId",
                table: "MotorcycleRentals",
                column: "MotorcycleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_RentalModelId",
                table: "MotorcycleRentals",
                column: "RentalModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MotorcycleRentals_Motorcycles_MotorcycleModelId",
                table: "MotorcycleRentals",
                column: "MotorcycleModelId",
                principalTable: "Motorcycles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MotorcycleRentals_Rentals_RentalModelId",
                table: "MotorcycleRentals",
                column: "RentalModelId",
                principalTable: "Rentals",
                principalColumn: "Id");
        }
    }
}
