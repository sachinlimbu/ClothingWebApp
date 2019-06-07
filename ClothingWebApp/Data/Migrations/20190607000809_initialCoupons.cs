using Microsoft.EntityFrameworkCore.Migrations;

namespace ClothingWebApp.Data.Migrations
{
    public partial class initialCoupons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "coupons",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "coupons");
        }
    }
}
