using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeliveryPersonRentalRemovedLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotorcycleRentals_DeliveryPersons_DeliveryPersonModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleRentals_DeliveryPersonModelId",
                table: "MotorcycleRentals");

            migrationBuilder.DropColumn(
                name: "DeliveryPersonModelId",
                table: "MotorcycleRentals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryPersonModelId",
                table: "MotorcycleRentals",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleRentals_DeliveryPersonModelId",
                table: "MotorcycleRentals",
                column: "DeliveryPersonModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MotorcycleRentals_DeliveryPersons_DeliveryPersonModelId",
                table: "MotorcycleRentals",
                column: "DeliveryPersonModelId",
                principalTable: "DeliveryPersons",
                principalColumn: "Id");
        }
    }
}
