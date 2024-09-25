using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCDMSServices.Migrations
{
    /// <inheritdoc />
    public partial class AddFilesAndFileDataTAble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryName = table.Column<string>(type: "TEXT", nullable: false),
                    FarmName = table.Column<string>(type: "TEXT", nullable: false),
                    CoopNumber = table.Column<string>(type: "TEXT", nullable: false),
                    GrowthStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataCollectionTime = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileId = table.Column<long>(type: "INTEGER", nullable: false),
                    Hours = table.Column<long>(type: "INTEGER", nullable: false),
                    Mean = table.Column<decimal>(type: "TEXT", nullable: false),
                    Median = table.Column<decimal>(type: "TEXT", nullable: false),
                    Std = table.Column<decimal>(type: "TEXT", nullable: false),
                    Count = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileData_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileData_FileId",
                table: "FileData",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileData");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
