using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcReadMe_Group4.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookReadWithOptionalRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BookReads",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookReads_UserId",
                table: "BookReads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReads_Users_UserId",
                table: "BookReads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReads_Users_UserId",
                table: "BookReads");

            migrationBuilder.DropIndex(
                name: "IX_BookReads_UserId",
                table: "BookReads");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookReads");
        }
    }
}
