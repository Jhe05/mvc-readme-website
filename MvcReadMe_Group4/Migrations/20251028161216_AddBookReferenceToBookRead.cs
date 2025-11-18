using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcReadMe_Group4.Migrations
{
    /// <inheritdoc />
    public partial class AddBookReferenceToBookRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "BookReads",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookReads_BookId",
                table: "BookReads",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReads_Books_BookId",
                table: "BookReads",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReads_Books_BookId",
                table: "BookReads");

            migrationBuilder.DropIndex(
                name: "IX_BookReads_BookId",
                table: "BookReads");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookReads");
        }
    }
}
