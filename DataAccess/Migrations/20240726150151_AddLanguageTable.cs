using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocaleId",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "SystemModulesID",
                table: "Resources",
                newName: "ModulesID");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Resources",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsShard",
                table: "Resources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LangID",
                table: "Resources",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "languageID",
                table: "Resources",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_languageID",
                table: "Resources",
                column: "languageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Languages_languageID",
                table: "Resources",
                column: "languageID",
                principalTable: "Languages",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Languages_languageID",
                table: "Resources");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Resources_languageID",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "IsShard",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "LangID",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "languageID",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "ModulesID",
                table: "Resources",
                newName: "SystemModulesID");

            migrationBuilder.AddColumn<string>(
                name: "LocaleId",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
