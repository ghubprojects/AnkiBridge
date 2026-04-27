using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiBridge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Exporting");

            migrationBuilder.EnsureSchema(
                name: "Dictionary");

            migrationBuilder.EnsureSchema(
                name: "Learning");

            migrationBuilder.CreateTable(
                name: "CardTemplate",
                schema: "Exporting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Css = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deck",
                schema: "Exporting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryEntry",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Headword = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PartOfSpeech = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LearningItem",
                schema: "Learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Headword = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PartOfSpeech = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Accent = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Ipa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Cloze = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Definition = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AudioUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LearningItemExport",
                schema: "Exporting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LearningItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeckId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Destination = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Attempts = table.Column<int>(type: "integer", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Error = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExportedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningItemExport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardType",
                schema: "Exporting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FrontHtml = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    BackHtml = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardType_CardTemplate_CardTemplateId",
                        column: x => x.CardTemplateId,
                        principalSchema: "Exporting",
                        principalTable: "CardTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Definition",
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
                name: "Image",
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
                name: "Pronunciation",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ipa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Accent = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AudioUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AudioSource = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pronunciation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pronunciation_DictionaryEntry_DictionaryEntryId",
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
                name: "Example",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefinitionId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "IX_CardTemplate_CreatedAt",
                schema: "Exporting",
                table: "CardTemplate",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CardTemplate_Name",
                schema: "Exporting",
                table: "CardTemplate",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_CardTemplateId",
                schema: "Exporting",
                table: "CardType",
                column: "CardTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_CardTemplateId_Name",
                schema: "Exporting",
                table: "CardType",
                columns: new[] { "CardTemplateId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Deck_CreatedAt",
                schema: "Exporting",
                table: "Deck",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Deck_Name",
                schema: "Exporting",
                table: "Deck",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Definition_DictionaryEntryId",
                schema: "Dictionary",
                table: "Definition",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntry_Headword",
                schema: "Dictionary",
                table: "DictionaryEntry",
                column: "Headword");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntry_Headword_PartOfSpeech",
                schema: "Dictionary",
                table: "DictionaryEntry",
                columns: new[] { "Headword", "PartOfSpeech" });

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

            migrationBuilder.CreateIndex(
                name: "IX_LearningItem_CreatedAt",
                schema: "Learning",
                table: "LearningItem",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LearningItem_Headword",
                schema: "Learning",
                table: "LearningItem",
                column: "Headword");

            migrationBuilder.CreateIndex(
                name: "IX_LearningItem_Headword_PartOfSpeech",
                schema: "Learning",
                table: "LearningItem",
                columns: new[] { "Headword", "PartOfSpeech" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningItemExport_CardTemplateId",
                schema: "Exporting",
                table: "LearningItemExport",
                column: "CardTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningItemExport_DeckId",
                schema: "Exporting",
                table: "LearningItemExport",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningItemExport_Destination_Status",
                schema: "Exporting",
                table: "LearningItemExport",
                columns: new[] { "Destination", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningItemExport_LearningItemId",
                schema: "Exporting",
                table: "LearningItemExport",
                column: "LearningItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Pronunciation_DictionaryEntryId",
                schema: "Dictionary",
                table: "Pronunciation",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pronunciation_Ipa_Accent",
                schema: "Dictionary",
                table: "Pronunciation",
                columns: new[] { "Ipa", "Accent" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardType",
                schema: "Exporting");

            migrationBuilder.DropTable(
                name: "Deck",
                schema: "Exporting");

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
                name: "LearningItemExport",
                schema: "Exporting");

            migrationBuilder.DropTable(
                name: "Pronunciation",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "CardTemplate",
                schema: "Exporting");

            migrationBuilder.DropTable(
                name: "Definition",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "LearningItem",
                schema: "Learning");

            migrationBuilder.DropTable(
                name: "DictionaryEntry",
                schema: "Dictionary");
        }
    }
}
