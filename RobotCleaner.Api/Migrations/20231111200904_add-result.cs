using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobotCleaner.Api.Migrations
{
    /// <inheritdoc />
    public partial class addresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "Executions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "Executions");
        }
    }
}
