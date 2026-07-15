using System;
using AnkiBridge.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnkiBridge.Infrastructure.Persistence.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260713000000_AddLearningEntries")]
public partial class AddLearningEntries : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "Learning");

        migrationBuilder.CreateTable(
            name: "LearningEntry",
            schema: "Learning",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Headword = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                PartOfSpeech = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Cloze = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Definition = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                Translation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                TranslationSource = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Accent = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Ipa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                AudioSource = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                AudioPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                AudioDownloadStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                ImageSource = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                ImagePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                ImageDownloadStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LearningEntry", entry => entry.Id);
            });

        migrationBuilder.CreateTable(
            name: "LearningExample",
            schema: "Learning",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                OrderIndex = table.Column<int>(type: "integer", nullable: false),
                LearningEntryId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LearningExample", example => example.Id);
                table.ForeignKey(
                    name: "FK_LearningExample_LearningEntry_LearningEntryId",
                    column: example => example.LearningEntryId,
                    principalSchema: "Learning",
                    principalTable: "LearningEntry",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_LearningEntry_CreatedAt",
            schema: "Learning",
            table: "LearningEntry",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_LearningEntry_DictionaryEntryId",
            schema: "Learning",
            table: "LearningEntry",
            column: "DictionaryEntryId");

        migrationBuilder.CreateIndex(
            name: "IX_LearningExample_LearningEntryId",
            schema: "Learning",
            table: "LearningExample",
            column: "LearningEntryId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "LearningExample", schema: "Learning");
        migrationBuilder.DropTable(name: "LearningEntry", schema: "Learning");
    }
}
