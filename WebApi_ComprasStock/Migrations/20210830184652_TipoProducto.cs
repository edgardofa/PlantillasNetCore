using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_ComprasStock.Migrations
{
    public partial class TipoProducto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CodigoTipo = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProductos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoProductos_CodigoTipo",
                table: "TipoProductos",
                column: "CodigoTipo",
                unique: true,
                filter: "[CodigoTipo] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoProductos");
        }
    }
}
