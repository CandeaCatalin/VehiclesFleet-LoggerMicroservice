using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoggerMicroservice.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateModelForLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Logs",
                newName: "Source");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Source",
                table: "Logs",
                newName: "UserEmail");
        }
    }
}
