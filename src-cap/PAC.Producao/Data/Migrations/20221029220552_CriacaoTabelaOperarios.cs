using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAC.Producao.Data.Migrations
{
    public partial class CriacaoTabelaOperarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Producao");

            migrationBuilder.CreateTable(
                name: "Operarios",
                schema: "Producao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Apelido = table.Column<string>(type: "VARCHAR(128)", nullable: true),
                    Cargo = table.Column<byte>(type: "TINYINT", nullable: true),
                    PeriodoTrabalho = table.Column<byte>(type: "TINYINT", nullable: true),
                    Ativo = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operarios",
                schema: "Producao");
        }
    }
}
