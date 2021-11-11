using Microsoft.EntityFrameworkCore.Migrations;

namespace Planograma.EmplUser.Infrastructure.Migrations
{
    public partial class InitialUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "EmployeeParams",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "EmployeeParams");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
