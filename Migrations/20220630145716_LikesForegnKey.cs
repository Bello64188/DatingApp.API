using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class LikesForegnKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3159e6be-35cf-4617-9326-653b5f21bee3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ec7fad4-9e8b-4d84-be10-a268a7924a69");

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    likerId = table.Column<string>(type: "TEXT", nullable: false),
                    likeeId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => new { x.likeeId, x.likerId });
                    table.ForeignKey(
                        name: "FK_Like_AspNetUsers_likeeId",
                        column: x => x.likeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Like_AspNetUsers_likerId",
                        column: x => x.likerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5f44ed1-7721-4449-8c59-a468fd1c6dbd", "43ec1c0c-1aca-4d9d-a87c-1bde2c2402ce", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9c338139-48db-4a99-af85-06e344a733ba", "a7de63b6-ce0f-4129-ac48-3fbffb5641eb", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_Like_likerId",
                table: "Like",
                column: "likerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c338139-48db-4a99-af85-06e344a733ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5f44ed1-7721-4449-8c59-a468fd1c6dbd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7ec7fad4-9e8b-4d84-be10-a268a7924a69", "037889b7-3304-4a91-b6bc-594e9dff59d1", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3159e6be-35cf-4617-9326-653b5f21bee3", "0277c678-fcd4-42b8-a2d7-0552a7fe6ca9", "Administrator", "ADMINISTRATOR" });
        }
    }
}
