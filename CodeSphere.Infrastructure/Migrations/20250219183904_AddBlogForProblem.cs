using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogForProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContestStatus",
                table: "Contests");

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Problems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_BlogId",
                table: "Problems",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Blogs_BlogId",
                table: "Problems",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Blogs_BlogId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_BlogId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Problems");

            migrationBuilder.AddColumn<int>(
                name: "ContestStatus",
                table: "Contests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
