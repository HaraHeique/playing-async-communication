using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAC.Vendas.Data.Migrations
{
    public partial class AdicaoTabelaVendedores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Vendas");

            migrationBuilder.CreateTable(
                name: "Vendedores",
                schema: "Vendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    Ativo = table.Column<bool>(type: "BIT", nullable: false),
                    Funcao = table.Column<short>(type: "SMALLINT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendedores",
                schema: "Vendas");
        }
    }
}
