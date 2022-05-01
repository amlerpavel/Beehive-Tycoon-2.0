using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeehiveTycoon.Migrations
{
    public partial class DohraneHry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DokonceneHry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObtiznostId = table.Column<int>(type: "int", nullable: false),
                    Rok = table.Column<int>(type: "int", nullable: false),
                    Mesic = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UzivatelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DokonceneHry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DokonceneHry_Uzivatele_UzivatelId",
                        column: x => x.UzivatelId,
                        principalTable: "Uzivatele",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DokonceneHry_UzivatelId",
                table: "DokonceneHry",
                column: "UzivatelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DokonceneHry");
        }
    }
}
