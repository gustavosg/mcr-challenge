using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MotorcycleRentalsRemovedForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_DeliveryPersons_DeliveryPersonId",
                table: "MotorcycleRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_Motorcycles_MotorcycleId",
                table: "MotorcycleRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_Rentals_RentalId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_DeliveryPersonId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_MotorcycleId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_RentalId",
                table: "MotorcycleRentals");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryPersonModelId",
                table: "MotorcycleRentals",
                type: "uuid",
                nullable: true);

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
                name: "IX_MotorcycleRentals_DeliveryPersonModelId",
                table: "MotorcycleRentals",
                column: "DeliveryPersonModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_MotorcycleModelId",
                table: "MotorcycleRentals",
                column: "MotorcycleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_RentalModelId",
                table: "MotorcycleRentals",
                column: "RentalModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MotorcycleRentals_DeliveryPersons_DeliveryPersonModelId",
                table: "MotorcycleRentals",
                column: "DeliveryPersonModelId",
                principalTable: "DeliveryPersons",
                principalColumn: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_DeliveryPersons_DeliveryPersonModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_Motorcycles_MotorcycleModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_Rentals_RentalModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_DeliveryPersonModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_MotorcycleModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_RentalModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropColumn(
                name: "DeliveryPersonModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropColumn(
                name: "MotorcycleModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropColumn(
                name: "RentalModelId",
                table: "MotorcycleRentals");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_DeliveryPersonId",
                table: "MotorcycleRentals",
                column: "DeliveryPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_MotorcycleId",
                table: "MotorcycleRentals",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_RentalId",
                table: "MotorcycleRentals",
                column: "RentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_MotorcycleRentals_DeliveryPersons_DeliveryPersonId",
                table: "MotorcycleRentals",
                column: "DeliveryPersonId",
                principalTable: "DeliveryPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotorcycleRentals_Motorcycles_MotorcycleId",
                table: "MotorcycleRentals",
                column: "MotorcycleId",
                principalTable: "Motorcycles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotorcycleRentals_Rentals_RentalId",
                table: "MotorcycleRentals",
                column: "RentalId",
                principalTable: "Rentals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
