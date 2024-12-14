using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campussen",
                columns: table => new
                {
                    CampusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Straat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Huisnummer = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Gemeente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campussen", x => x.CampusId);
                });

            migrationBuilder.CreateTable(
                name: "Landen",
                columns: table => new
                {
                    LandCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landen", x => x.LandCode);
                });

            migrationBuilder.CreateTable(
                name: "Docenten",
                columns: table => new
                {
                    DocentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Familienaam = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Maandwedde = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InDienst = table.Column<DateOnly>(type: "date", nullable: false),
                    HeeftRijbewijs = table.Column<bool>(type: "bit", nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    LandCode = table.Column<string>(type: "nvarchar(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docenten", x => x.DocentId);
                    table.ForeignKey(
                        name: "FK_Docenten_Campussen_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campussen",
                        principalColumn: "CampusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Docenten_Landen_LandCode",
                        column: x => x.LandCode,
                        principalTable: "Landen",
                        principalColumn: "LandCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Idx_CampusNaam",
                table: "Campussen",
                column: "CampusNaam",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Idx_DocentNaam",
                table: "Docenten",
                columns: new[] { "Voornaam", "Familienaam" });

            migrationBuilder.CreateIndex(
                name: "IX_Docenten_CampusId",
                table: "Docenten",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Docenten_LandCode",
                table: "Docenten",
                column: "LandCode");

            migrationBuilder.CreateIndex(
                name: "Idx_LandNaam",
                table: "Landen",
                column: "Naam",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Docenten");

            migrationBuilder.DropTable(
                name: "Campussen");

            migrationBuilder.DropTable(
                name: "Landen");
        }
    }
}
