using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelationBetweenProblemAndBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Problems_ProblemId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_ProblemId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "Blogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContestId",
                table: "Submits",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ContestId",
                table: "Submits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProblemId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ProblemId",
                table: "Blogs",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Problems_ProblemId",
                table: "Blogs",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
