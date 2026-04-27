using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiBridge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAudioAndImageColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "Learning",
                table: "LearningItem",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "AudioUrl",
                schema: "Learning",
                table: "LearningItem",
                newName: "AudioPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                schema: "Learning",
                table: "LearningItem",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "AudioPath",
                schema: "Learning",
                table: "LearningItem",
                newName: "AudioUrl");
        }
    }
}
