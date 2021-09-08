using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_ComprasStock.Migrations
{
    public partial class CategoriaRubro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Rubros_RubrosId",
                table: "Categorias");

            migrationBuilder.RenameColumn(
                name: "RubrosId",
                table: "Categorias",
                newName: "RubroId");

            migrationBuilder.RenameIndex(
                name: "IX_Categorias_RubrosId",
                table: "Categorias",
                newName: "IX_Categorias_RubroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Rubros_RubroId",
                table: "Categorias",
                column: "RubroId",
                principalTable: "Rubros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Rubros_RubroId",
                table: "Categorias");

            migrationBuilder.RenameColumn(
                name: "RubroId",
                table: "Categorias",
                newName: "RubrosId");

            migrationBuilder.RenameIndex(
                name: "IX_Categorias_RubroId",
                table: "Categorias",
                newName: "IX_Categorias_RubrosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Rubros_RubrosId",
                table: "Categorias",
                column: "RubrosId",
                principalTable: "Rubros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
