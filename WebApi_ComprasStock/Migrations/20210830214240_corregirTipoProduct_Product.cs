using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_ComprasStock.Migrations
{
    public partial class corregirTipoProduct_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos");

            migrationBuilder.AlterColumn<int>(
                name: "TipoProductoId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos",
                column: "TipoProductoId",
                principalTable: "TipoProductos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos");

            migrationBuilder.AlterColumn<int>(
                name: "TipoProductoId",
                table: "Productos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos",
                column: "TipoProductoId",
                principalTable: "TipoProductos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
