using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Memberships",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TrainingClassId",
                table: "Bookings",
                column: "TrainingClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_TrainingClassId",
                table: "Bookings",
                columns: new[] { "UserId", "TrainingClassId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TrainingClasses_TrainingClassId",
                table: "Bookings",
                column: "TrainingClassId",
                principalTable: "TrainingClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TrainingClasses_TrainingClassId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TrainingClassId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId_TrainingClassId",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Memberships",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
