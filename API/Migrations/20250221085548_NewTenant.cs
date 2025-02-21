using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class NewTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: false,
                        defaultValueSql: "SYSDATETIMEOFFSET()"
                    ),
                    UpdatedTime = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: false,
                        defaultValueSql: "SYSDATETIMEOFFSET()"
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantConfigurations", x => x.Id);
                }
            );

            migrationBuilder.InsertData(
                table: "TenantConfigurations",
                columns: new[] { "Id", "CreatedTime", "Price", "Type" },
                values: new object[,]
                {
                    {
                        new Guid("0de9ff14-18f5-4550-8e6f-b73dadc4af30"),
                        new DateTimeOffset(
                            new DateTime(
                                2025,
                                2,
                                21,
                                8,
                                55,
                                48,
                                697,
                                DateTimeKind.Unspecified
                            ).AddTicks(5529),
                            new TimeSpan(0, 0, 0, 0, 0)
                        ),
                        0m,
                        0,
                    },
                    {
                        new Guid("6de87b5e-8a58-411c-b7bc-8f5469919b99"),
                        new DateTimeOffset(
                            new DateTime(
                                2025,
                                2,
                                21,
                                8,
                                55,
                                48,
                                697,
                                DateTimeKind.Unspecified
                            ).AddTicks(5547),
                            new TimeSpan(0, 0, 0, 0, 0)
                        ),
                        19.99m,
                        2,
                    },
                    {
                        new Guid("ac6d9448-00cb-4d69-b990-2e9f444ee254"),
                        new DateTimeOffset(
                            new DateTime(
                                2025,
                                2,
                                21,
                                8,
                                55,
                                48,
                                697,
                                DateTimeKind.Unspecified
                            ).AddTicks(5533),
                            new TimeSpan(0, 0, 0, 0, 0)
                        ),
                        9.99m,
                        1,
                    },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TenantConfigurations");
        }
    }
}
