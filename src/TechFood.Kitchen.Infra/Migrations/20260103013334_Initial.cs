using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechFood.Kitchen.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preparations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preparations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Preparations",
                columns: new[] { "Id", "CancelledAt", "CreatedAt", "DeliveredAt", "IsDeleted", "OrderId", "ReadyAt", "StartedAt", "Status" },
                values: new object[,]
                {
                    { new Guid("83874d8f-0bc8-42ab-85d9-540a36dcccf4"), null, new DateTime(2025, 5, 13, 22, 2, 36, 0, DateTimeKind.Utc).AddTicks(6354), null, false, new Guid("f2b5f3a2-4c8e-4b7c-9f0e-5a2d6f3b8c1e"), null, null, 2 },
                    { new Guid("9b50f871-b829-4085-8ae5-118cd1198fbe"), null, new DateTime(2025, 5, 13, 22, 2, 36, 0, DateTimeKind.Utc).AddTicks(6053), null, false, new Guid("d1b5f3a2-4c8e-4b7c-9f0e-5a2d6f3b8c1e"), null, null, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preparations");
        }
    }
}
