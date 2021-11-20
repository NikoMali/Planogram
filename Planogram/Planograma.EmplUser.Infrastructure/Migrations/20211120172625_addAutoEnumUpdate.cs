using Microsoft.EntityFrameworkCore.Migrations;

namespace Planograma.EmplUser.Infrastructure.Migrations
{
    public partial class addAutoEnumUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "testTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { (byte)4, true, "Test4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "testTypes",
                keyColumn: "Id",
                keyValue: (byte)4);
        }
    }
}
