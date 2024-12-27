using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailingSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Attachments");
        }
    }
}
