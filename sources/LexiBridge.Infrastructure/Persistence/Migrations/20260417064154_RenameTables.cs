using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiBridge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Example",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "Example",
                schema: "Learning");

            migrationBuilder.DropTable(
                name: "Image",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "Definition",
                schema: "Dictionary");

            migrationBuilder.CreateTable(
                name: "EntryDefinition",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryDefinition_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryImage",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryImage_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningExample",
                schema: "Learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningExample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningExample_LearningItem_LearningItemId",
                        column: x => x.LearningItemId,
                        principalSchema: "Learning",
                        principalTable: "LearningItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryExample",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefinitionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryExample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryExample_EntryDefinition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalSchema: "Dictionary",
                        principalTable: "EntryDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntryDefinition_DictionaryEntryId",
                schema: "Dictionary",
                table: "EntryDefinition",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryExample_DefinitionId",
                schema: "Dictionary",
                table: "EntryExample",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryImage_DictionaryEntryId",
                schema: "Dictionary",
                table: "EntryImage",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningExample_LearningItemId",
                schema: "Learning",
                table: "LearningExample",
                column: "LearningItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryExample",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "EntryImage",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "LearningExample",
                schema: "Learning");

            migrationBuilder.DropTable(
                name: "EntryDefinition",
                schema: "Dictionary");

            migrationBuilder.CreateTable(
                name: "Definition",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Definition_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Example",
                schema: "Learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Example1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Example_LearningItem_LearningItemId",
                        column: x => x.LearningItemId,
                        principalSchema: "Learning",
                        principalTable: "LearningItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Example",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Example", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Example_Definition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalSchema: "Dictionary",
                        principalTable: "Definition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Definition_DictionaryEntryId",
                schema: "Dictionary",
                table: "Definition",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Example_DefinitionId",
                schema: "Dictionary",
                table: "Example",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Example_LearningItemId",
                schema: "Learning",
                table: "Example",
                column: "LearningItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_DictionaryEntryId",
                schema: "Dictionary",
                table: "Image",
                column: "DictionaryEntryId");
        }
    }
}
