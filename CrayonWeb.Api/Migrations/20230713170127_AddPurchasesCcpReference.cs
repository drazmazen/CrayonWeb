using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrayonWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchasesCcpReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CcpReference",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CcpReference",
                table: "Purchases");
        }
    }
}
