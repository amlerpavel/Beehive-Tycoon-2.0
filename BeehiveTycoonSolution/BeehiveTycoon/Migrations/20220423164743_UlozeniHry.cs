using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeehiveTycoon.Migrations
{
    public partial class UlozeniHry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UlozeneHry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Postup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pozice = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UzivatelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UlozeneHry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UlozeneHry_Uzivatele_UzivatelId",
                        column: x => x.UzivatelId,
                        principalTable: "Uzivatele",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UlozeneHry_UzivatelId",
                table: "UlozeneHry",
                column: "UzivatelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UlozeneHry");
        }
    }
}
