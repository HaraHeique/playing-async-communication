using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAC.RH.Data.Migrations
{
    public partial class CriacaoTabelaFuncionarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Rh");

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                schema: "Rh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Apelido = table.Column<string>(type: "VARCHAR(200)", nullable: true),
                    Cpf = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    Rg = table.Column<string>(type: "VARCHAR(9)", nullable: false),
                    EmailPessoal = table.Column<string>(type: "VARCHAR(254)", nullable: false),
                    EmailEmpresarial = table.Column<string>(type: "VARCHAR(254)", nullable: false),
                    Numero = table.Column<int>(type: "INT", nullable: true),
                    Rua = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Complemento = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    Cep = table.Column<string>(type: "VARCHAR(8)", nullable: false),
                    Bairro = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Cidade = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Estado = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    SalarioBruto = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Setor = table.Column<int>(type: "INT", nullable: false),
                    Desligado = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funcionarios",
                schema: "Rh");
        }
    }
}
