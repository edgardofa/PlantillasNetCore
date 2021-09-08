using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_ComprasStock.Migrations
{
    public partial class UM_EnProductos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Productos_UnidadMedida",
                table: "Productos",
                column: "UnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UnidadDeMedida_UnidadMedida",
                table: "Productos",
                column: "UnidadMedida",
                principalTable: "UnidadDeMedida",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_UnidadDeMedida_UnidadMedida",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_UnidadMedida",
                table: "Productos");
        }
    }
}
