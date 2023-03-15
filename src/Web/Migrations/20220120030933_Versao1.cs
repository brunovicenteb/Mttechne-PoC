using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmbevWeb.Migrations
{
    public partial class Versao1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cerveja",
                columns: table => new
                {
                    IdCerveja = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Estoque = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Preco = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cerveja", x => x.IdCerveja);
                });

            migrationBuilder.CreateTable(
                name: "DiaDaSemana",
                columns: table => new
                {
                    IdDiaDaSemana = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaDaSemana", x => x.IdDiaDaSemana);
                });

            migrationBuilder.CreateTable(
                name: "SituacaoCashBack",
                columns: table => new
                {
                    IdSituacaoCashBack = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituacaoCashBack", x => x.IdSituacaoCashBack);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Senha = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now', 'localtime')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "CashBack",
                columns: table => new
                {
                    IdCachBack = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCerveja = table.Column<int>(type: "INTEGER", nullable: false),
                    Porcentagem = table.Column<double>(type: "REAL", nullable: false),
                    IdDiaDaSemana = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashBack", x => x.IdCachBack);
                    table.ForeignKey(
                        name: "FK_CashBack_Cerveja_IdCerveja",
                        column: x => x.IdCerveja,
                        principalTable: "Cerveja",
                        principalColumn: "IdCerveja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashBack_DiaDaSemana_IdDiaDaSemana",
                        column: x => x.IdDiaDaSemana,
                        principalTable: "DiaDaSemana",
                        principalColumn: "IdDiaDaSemana",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CPF = table.Column<string>(type: "char(14)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Cliente_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Venda",
                columns: table => new
                {
                    IdVenda = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InicioVenda = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now', 'localtime')"),
                    DataVenda = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true),
                    CashBack = table.Column<double>(type: "REAL", nullable: true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venda", x => x.IdVenda);
                    table.ForeignKey(
                        name: "FK_Venda_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemVenda",
                columns: table => new
                {
                    IdVenda = table.Column<int>(type: "INTEGER", nullable: false),
                    IdCerveja = table.Column<int>(type: "INTEGER", nullable: false),
                    IdCashBack = table.Column<int>(type: "INTEGER", nullable: false),
                    IdSituacaoCashBack = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorUnitario = table.Column<double>(type: "REAL", nullable: false),
                    FracaoCachBack = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemVenda", x => new { x.IdVenda, x.IdCerveja });
                    table.ForeignKey(
                        name: "FK_ItemVenda_Cerveja_IdCerveja",
                        column: x => x.IdCerveja,
                        principalTable: "Cerveja",
                        principalColumn: "IdCerveja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemVenda_Venda_IdVenda",
                        column: x => x.IdVenda,
                        principalTable: "Venda",
                        principalColumn: "IdVenda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cerveja",
                columns: new[] { "IdCerveja", "Estoque", "Nome", "Preco" },
                values: new object[] { 1, 2500, "Skol", 5.5 });

            migrationBuilder.InsertData(
                table: "Cerveja",
                columns: new[] { "IdCerveja", "Estoque", "Nome", "Preco" },
                values: new object[] { 2, 2250, "Brahma", 5.25 });

            migrationBuilder.InsertData(
                table: "Cerveja",
                columns: new[] { "IdCerveja", "Estoque", "Nome", "Preco" },
                values: new object[] { 3, 650, "Stella", 7.25 });

            migrationBuilder.InsertData(
                table: "Cerveja",
                columns: new[] { "IdCerveja", "Estoque", "Nome", "Preco" },
                values: new object[] { 4, 850, "Bohemia", 6.8899999999999997 });

            migrationBuilder.InsertData(
                table: "DiaDaSemana",
                columns: new[] { "IdDiaDaSemana", "Nome" },
                values: new object[] { 7, "Sábado" });

            migrationBuilder.InsertData(
                table: "DiaDaSemana",
                columns: new[] { "IdDiaDaSemana", "Nome" },
                values: new object[] { 6, "Sexta-Feira" });

            migrationBuilder.InsertData(
                table: "DiaDaSemana",
                columns: new[] { "IdDiaDaSemana", "Nome" },
                values: new object[] { 5, "Quinta-Feira" });

            migrationBuilder.InsertData(
                table: "DiaDaSemana",
                columns: new[] { "IdDiaDaSemana", "Nome" },
                values: new object[] { 4, "Quarta-Feira" });

            migrationBuilder.InsertData(
                table: "DiaDaSemana",
                columns: new[] { "IdDiaDaSemana", "Nome" },
                values: new object[] { 3, "Terça-Feira" });

            migrationBuilder.InsertData(
                table: "DiaDaSemana",
                columns: new[] { "IdDiaDaSemana", "Nome" },
                values: new object[] { 2, "Segunda-Feira" });

            migrationBuilder.InsertData(
                table: "DiaDaSemana",
                columns: new[] { "IdDiaDaSemana", "Nome" },
                values: new object[] { 1, "Domingo" });

            migrationBuilder.InsertData(
                table: "SituacaoCashBack",
                columns: new[] { "IdSituacaoCashBack", "Nome" },
                values: new object[] { 2, "Resgatado" });

            migrationBuilder.InsertData(
                table: "SituacaoCashBack",
                columns: new[] { "IdSituacaoCashBack", "Nome" },
                values: new object[] { 1, "Disponível" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 9, "odette.annable@com.br", "Odette Annable", "5f87be8e29c305ccd4ee19b0dd898f2b" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 10, "jennifer.lawrence@ambev.com.br", "Jennifer Lawrence", "c4ad31baad701357a17d7389aa68b7e8" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 7, "jennifer.lopez@ambev.com.br", "Jennifer Lopez", "d30579c85039820a61f8426c5a12704d" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 6, "liv.tyler@ambev.com.br", "Liv Tyler", "d2137db845f757d43f654f8eabc80762" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 5, "scarlett.johansson@ambev.com.br", "Scarlett Johansson", "318faad8da75364f28d3af4c01cf8783" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 4, "jennifer.connelly@ambev.com.br", "Jennifer Connelly", "fad05d338687a585d335d6d122034990" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 3, "sharon.stone@ambev.com.br", "Sharon Stone", "03d93326278650c09d0f825734230724" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 2, "penelope.cruz@ambev.com.br", "Penelope Cruz", "7688ab571b8c9112c74f634b0942c5f4" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 1, "natalie.portman@ambev.com.br", "Natalie Portman", "c157a6c950350e1cff1c37cd370782ce" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Email", "Nome", "Senha" },
                values: new object[] { 8, "gal.gadot@ambev.com.br", "Gal Gadot", "4fdf95910c5e7817123b3e09936e853f" });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 3, 1, 3, 6.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 24, 4, 3, 15.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 4, 1, 4, 2.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 11, 2, 4, 15.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 18, 3, 4, 8.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 25, 4, 4, 15.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 5, 1, 5, 10.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 12, 2, 5, 20.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 19, 3, 5, 13.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 26, 4, 5, 15.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 6, 1, 6, 15.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 13, 2, 6, 25.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 20, 3, 6, 18.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 27, 4, 6, 20.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 7, 1, 7, 20.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 14, 2, 7, 30.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 17, 3, 3, 5.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 10, 2, 3, 10.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 28, 4, 7, 40.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 23, 4, 2, 10.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 16, 3, 2, 3.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 9, 2, 2, 5.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 2, 1, 2, 7.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 22, 4, 1, 40.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 15, 3, 1, 35.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 8, 2, 1, 30.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 1, 1, 1, 25.0 });

            migrationBuilder.InsertData(
                table: "CashBack",
                columns: new[] { "IdCachBack", "IdCerveja", "IdDiaDaSemana", "Porcentagem" },
                values: new object[] { 21, 3, 7, 25.0 });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 2, "409.579.270-10", new DateTime(1981, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 3, "616.150.370-04", new DateTime(1958, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 4, "345.930.500-22", new DateTime(1970, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 5, "139.385.260-25", new DateTime(1984, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 9, "141.237.860-57", new DateTime(1985, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 7, "074.477.660-03", new DateTime(1969, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 8, "760.947.580-72", new DateTime(1985, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 10, "290.137.010-19", new DateTime(1990, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 6, "932.830.890-94", new DateTime(1977, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "IdUsuario", "CPF", "DataNascimento" },
                values: new object[] { 1, "791.647.240-69", new DateTime(1981, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_CashBack_IdCerveja",
                table: "CashBack",
                column: "IdCerveja");

            migrationBuilder.CreateIndex(
                name: "IX_CashBack_IdDiaDaSemana",
                table: "CashBack",
                column: "IdDiaDaSemana");

            migrationBuilder.CreateIndex(
                name: "IX_ItemVenda_IdCerveja",
                table: "ItemVenda",
                column: "IdCerveja");

            migrationBuilder.CreateIndex(
                name: "IX_Venda_IdCliente",
                table: "Venda",
                column: "IdCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashBack");

            migrationBuilder.DropTable(
                name: "ItemVenda");

            migrationBuilder.DropTable(
                name: "SituacaoCashBack");

            migrationBuilder.DropTable(
                name: "DiaDaSemana");

            migrationBuilder.DropTable(
                name: "Cerveja");

            migrationBuilder.DropTable(
                name: "Venda");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
