using Microsoft.EntityFrameworkCore.Migrations;

namespace Planograma.EmplUser.Infrastructure.Migrations
{
    public partial class addAutoEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "testTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "testTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { (byte)1, true, "Test" });

            migrationBuilder.InsertData(
                table: "testTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { (byte)2, true, "Test2" });

            migrationBuilder.InsertData(
                table: "testTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { (byte)3, true, "Test3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "testTypes");
        }
    }
}
