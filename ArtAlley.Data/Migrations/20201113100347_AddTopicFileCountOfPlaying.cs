using Microsoft.EntityFrameworkCore.Migrations;

namespace ArtAlley.Data.Migrations
{
    public partial class AddTopicFileCountOfPlaying : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicFiles_Topics_TopicId",
                table: "TopicFiles");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicFiles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountOfPlaying",
                table: "TopicFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicFiles_Topics_TopicId",
                table: "TopicFiles",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicFiles_Topics_TopicId",
                table: "TopicFiles");

            migrationBuilder.DropColumn(
                name: "CountOfPlaying",
                table: "TopicFiles");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TopicFiles_Topics_TopicId",
                table: "TopicFiles",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
