using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_ComprasStock.Migrations
{
    public partial class RelacRubrosHastaProductos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriasId",
                table: "TipoProductos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RubrosId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoProductos_CategoriasId",
                table: "TipoProductos",
                column: "CategoriasId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_RubrosId",
                table: "Categorias",
                column: "RubrosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Rubros_RubrosId",
                table: "Categorias",
                column: "RubrosId",
                principalTable: "Rubros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TipoProductos_Categorias_CategoriasId",
                table: "TipoProductos",
                column: "CategoriasId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Rubros_RubrosId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_TipoProductos_Categorias_CategoriasId",
                table: "TipoProductos");

            migrationBuilder.DropIndex(
                name: "IX_TipoProductos_CategoriasId",
                table: "TipoProductos");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_RubrosId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "CategoriasId",
                table: "TipoProductos");

            migrationBuilder.DropColumn(
                name: "RubrosId",
                table: "Categorias");
        }
    }
}
