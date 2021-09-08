using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_ComprasStock.Migrations
{
    public partial class Proveedor_Producto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProveedorProducto",
                columns: table => new
                {
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    DatosProveedoresId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProveedorProducto", x => new { x.ProveedorId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK_ProveedorProducto_DatosProveedores_DatosProveedoresId",
                        column: x => x.DatosProveedoresId,
                        principalTable: "DatosProveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProveedorProducto_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProveedorProducto_DatosProveedoresId",
                table: "ProveedorProducto",
                column: "DatosProveedoresId");

            migrationBuilder.CreateIndex(
                name: "IX_ProveedorProducto_ProductoId",
                table: "ProveedorProducto",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProveedorProducto");
        }
    }
}
