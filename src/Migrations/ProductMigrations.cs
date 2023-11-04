using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YourNamespace.Migrations
{
    public partial class InitialCreate_Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Description", "Price", "CreatedAt", "IsActive" },
                values: new object[,]
                {
                    { 1, "Product 1", "Description for Product 1", 19.99m, DateTime.UtcNow, true },
                    { 2, "Product 2", "Description for Product 2", 29.99m, DateTime.UtcNow, true },
                    { 3, "Product 3", "Description for Product 3", 9.99m, DateTime.UtcNow, true },
                    { 4, "Product 4", "Description for Product 4", 49.99m, DateTime.UtcNow, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
