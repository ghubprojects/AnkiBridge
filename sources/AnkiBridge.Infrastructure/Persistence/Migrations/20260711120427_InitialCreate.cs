using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnkiBridge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Dictionary");

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
                name: "DictionaryDefinition",
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
                    table.PrimaryKey("PK_DictionaryDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryDefinition_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryImage",
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
                    table.PrimaryKey("PK_DictionaryImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryImage_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryPronunciation",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ipa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Accent = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AudioUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AudioSource = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryPronunciation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryPronunciation_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryTranslation",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DictionaryEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryTranslation_DictionaryEntry_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryExample",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefinitionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryExample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryExample_DictionaryDefinition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryDefinition_DictionaryEntryId",
                schema: "Dictionary",
                table: "DictionaryDefinition",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntry_Headword_PartOfSpeech",
                schema: "Dictionary",
                table: "DictionaryEntry",
                columns: new[] { "Headword", "PartOfSpeech" });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryExample_DefinitionId",
                schema: "Dictionary",
                table: "DictionaryExample",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryImage_DictionaryEntryId",
                schema: "Dictionary",
                table: "DictionaryImage",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryPronunciation_DictionaryEntryId",
                schema: "Dictionary",
                table: "DictionaryPronunciation",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryTranslation_DictionaryEntryId",
                schema: "Dictionary",
                table: "DictionaryTranslation",
                column: "DictionaryEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryExample",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "DictionaryImage",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "DictionaryPronunciation",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "DictionaryTranslation",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "DictionaryDefinition",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "DictionaryEntry",
                schema: "Dictionary");
        }
    }
}
