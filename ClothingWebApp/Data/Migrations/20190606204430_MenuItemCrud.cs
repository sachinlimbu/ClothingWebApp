using Microsoft.EntityFrameworkCore.Migrations;

namespace ClothingWebApp.Data.Migrations
{
    public partial class MenuItemCrud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ItemWeight",
                table: "MenuItems",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ItemWeight",
                table: "MenuItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
