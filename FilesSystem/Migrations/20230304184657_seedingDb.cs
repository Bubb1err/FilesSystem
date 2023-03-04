using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilesSystem.Migrations
{
    public partial class seedingDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Name", "ParentId", "Path" },
                values: new object[,]
                {
                    { 1, "Creating Digital Images", null, "" },
                    { 2, "Resources", 1, "" },
                    { 3, "Primary Sources", 2, "" },
                    { 4, "Secondary Sources", 2, "" },
                    { 5, "Evidence", 1, "" },
                    { 6, "Graphic Products", 1, "" },
                    { 7, "Process", 6, "" },
                    { 8, "Final Product", 6, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
