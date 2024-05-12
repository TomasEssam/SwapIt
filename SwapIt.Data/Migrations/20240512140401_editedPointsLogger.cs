using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapIt.Data.Migrations
{
    public partial class editedPointsLogger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidFund",
                table: "ServiceRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "PointsLoggers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PaidFund",
                table: "ServiceRequests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "PointsLoggers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
