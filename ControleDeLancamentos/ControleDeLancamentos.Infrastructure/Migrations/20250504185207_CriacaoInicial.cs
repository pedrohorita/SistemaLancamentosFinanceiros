using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ControleDeLancamentos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lancamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    TipoId = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lancamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Tipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "Tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "DataAtualizacao", "DataCriacao", "Descricao", "Nome", "UsuarioAtualizacao", "UsuarioCriacao" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lançamentos relacionados a venda", "Vendas", null, "admin" },
                    { 2, null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lançamentos relacionados a empréstimos", "Emprestimos", null, "admin" },
                    { 3, null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lançamentos relacionados a despesas", "Despesas", null, "admin" },
                    { 4, null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lançamentos relacionados a investimentos", "Investimentos", null, "admin" },
                    { 5, null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lançamentos relacionados a outros", "Outros", null, "admin" }
                });

            migrationBuilder.InsertData(
                table: "Tipos",
                columns: new[] { "Id", "DataAtualizacao", "DataCriacao", "Descricao", "Nome", "UsuarioAtualizacao", "UsuarioCriacao" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lançamentos de entrada", "Entrada", null, "admin" },
                    { 2, null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lançamentos de saída", "Saída", null, "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_CategoriaId",
                table: "Lancamentos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_TipoId",
                table: "Lancamentos",
                column: "TipoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lancamentos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Tipos");
        }
    }
}
