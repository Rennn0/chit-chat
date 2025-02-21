using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class NewTenant2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TenantConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("0de9ff14-18f5-4550-8e6f-b73dadc4af30"));

            migrationBuilder.DeleteData(
                table: "TenantConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("6de87b5e-8a58-411c-b7bc-8f5469919b99"));

            migrationBuilder.DeleteData(
                table: "TenantConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("ac6d9448-00cb-4d69-b990-2e9f444ee254"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "TenantConfigurations",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "TenantConfigurations",
                columns: new[] { "Id", "CreatedTime", "Price", "Type" },
                values: new object[,]
                {
                    { new Guid("6bd9c7d4-6963-4c13-b07a-dd898a315ccb"), new DateTimeOffset(new DateTime(2025, 2, 21, 9, 51, 46, 490, DateTimeKind.Unspecified).AddTicks(409), new TimeSpan(0, 0, 0, 0, 0)), 0m, 0 },
                    { new Guid("e55c43c9-35a6-4b61-95d4-98bfc51a7425"), new DateTimeOffset(new DateTime(2025, 2, 21, 9, 51, 46, 490, DateTimeKind.Unspecified).AddTicks(412), new TimeSpan(0, 0, 0, 0, 0)), 9.99m, 1 },
                    { new Guid("e94c39c3-acb2-40bb-befc-debc16d2cda1"), new DateTimeOffset(new DateTime(2025, 2, 21, 9, 51, 46, 490, DateTimeKind.Unspecified).AddTicks(425), new TimeSpan(0, 0, 0, 0, 0)), 19.99m, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TenantConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("6bd9c7d4-6963-4c13-b07a-dd898a315ccb"));

            migrationBuilder.DeleteData(
                table: "TenantConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e55c43c9-35a6-4b61-95d4-98bfc51a7425"));

            migrationBuilder.DeleteData(
                table: "TenantConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e94c39c3-acb2-40bb-befc-debc16d2cda1"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "TenantConfigurations",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.InsertData(
                table: "TenantConfigurations",
                columns: new[] { "Id", "CreatedTime", "Price", "Type" },
                values: new object[,]
                {
                    { new Guid("0de9ff14-18f5-4550-8e6f-b73dadc4af30"), new DateTimeOffset(new DateTime(2025, 2, 21, 8, 55, 48, 697, DateTimeKind.Unspecified).AddTicks(5529), new TimeSpan(0, 0, 0, 0, 0)), 0m, 0 },
                    { new Guid("6de87b5e-8a58-411c-b7bc-8f5469919b99"), new DateTimeOffset(new DateTime(2025, 2, 21, 8, 55, 48, 697, DateTimeKind.Unspecified).AddTicks(5547), new TimeSpan(0, 0, 0, 0, 0)), 19.99m, 2 },
                    { new Guid("ac6d9448-00cb-4d69-b990-2e9f444ee254"), new DateTimeOffset(new DateTime(2025, 2, 21, 8, 55, 48, 697, DateTimeKind.Unspecified).AddTicks(5533), new TimeSpan(0, 0, 0, 0, 0)), 9.99m, 1 }
                });
        }
    }
}
