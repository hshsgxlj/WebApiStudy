using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiStudy.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shirts",
                columns: table => new
                {
                    ShirtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shirts", x => x.ShirtId);
                });

            migrationBuilder.InsertData(
                table: "shirts",
                columns: new[] { "ShirtId", "Brand", "Color", "Gender", "Price", "Size" },
                values: new object[,]
                {
                    { 1, "Nike", "Red", "men", 29.989999999999998, 9 },
                    { 2, "Adidas", "Blue", "women", 25.989999999999998, 6 },
                    { 3, "Puma", "Black", "men", 19.989999999999998, 10 },
                    { 4, "Uniqlo", "White", "women", 15.99, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shirts");
        }
    }
}
