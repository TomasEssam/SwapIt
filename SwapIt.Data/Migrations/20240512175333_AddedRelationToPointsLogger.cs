using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapIt.Data.Migrations
{
    public partial class AddedRelationToPointsLogger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceRequestId",
                table: "PointsLoggers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PointsLoggers_ServiceRequestId",
                table: "PointsLoggers",
                column: "ServiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointsLoggers_ServiceRequests_ServiceRequestId",
                table: "PointsLoggers",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointsLoggers_ServiceRequests_ServiceRequestId",
                table: "PointsLoggers");

            migrationBuilder.DropIndex(
                name: "IX_PointsLoggers_ServiceRequestId",
                table: "PointsLoggers");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "PointsLoggers");
        }
    }
}
