using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassLabels",
                columns: table => new
                {
                    ClassLabelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StringClassLabel = table.Column<string>(type: "TEXT", nullable: true),
                    ClassLabelImagesNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLabels", x => x.ClassLabelId);
                });

            migrationBuilder.CreateTable(
                name: "ImageContext",
                columns: table => new
                {
                    BlobId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageContext = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageContext", x => x.BlobId);
                });

            migrationBuilder.CreateTable(
                name: "ImagesInformation",
                columns: table => new
                {
                    ImageInformationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    Probability = table.Column<float>(type: "REAL", nullable: false),
                    ImageContextBlobId = table.Column<int>(type: "INTEGER", nullable: true),
                    ClassLabelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesInformation", x => x.ImageInformationId);
                    table.ForeignKey(
                        name: "FK_ImagesInformation_ClassLabels_ClassLabelId",
                        column: x => x.ClassLabelId,
                        principalTable: "ClassLabels",
                        principalColumn: "ClassLabelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesInformation_ImageContext_ImageContextBlobId",
                        column: x => x.ImageContextBlobId,
                        principalTable: "ImageContext",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagesInformation_ClassLabelId",
                table: "ImagesInformation",
                column: "ClassLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesInformation_ImageContextBlobId",
                table: "ImagesInformation",
                column: "ImageContextBlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagesInformation");

            migrationBuilder.DropTable(
                name: "ClassLabels");

            migrationBuilder.DropTable(
                name: "ImageContext");
        }
    }
}
