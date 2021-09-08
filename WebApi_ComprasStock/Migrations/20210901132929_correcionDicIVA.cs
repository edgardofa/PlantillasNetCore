using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_ComprasStock.Migrations
{
    public partial class correcionDicIVA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CIVA",
                table: "DatosProveedores",
                newName: "CIVAId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosProveedores_CIVAId",
                table: "DatosProveedores",
                column: "CIVAId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatosProveedores_Diccionario_CIVA_CIVAId",
                table: "DatosProveedores",
                column: "CIVAId",
                principalTable: "Diccionario_CIVA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatosProveedores_Diccionario_CIVA_CIVAId",
                table: "DatosProveedores");

            migrationBuilder.DropIndex(
                name: "IX_DatosProveedores_CIVAId",
                table: "DatosProveedores");

            migrationBuilder.RenameColumn(
                name: "CIVAId",
                table: "DatosProveedores",
                newName: "CIVA");
        }
    }
}
