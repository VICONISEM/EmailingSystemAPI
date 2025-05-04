using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailingSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAbbreviationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Colleges");
        }
    }
}
