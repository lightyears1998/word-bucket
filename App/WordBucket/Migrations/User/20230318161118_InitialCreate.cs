using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordBucket.Migrations.User
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Spelling = table.Column<string>(type: "TEXT", nullable: false),
                    Definitions = table.Column<string>(type: "TEXT", nullable: false),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    LastVisit = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Corpuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    Uri = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LearningWordId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corpuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corpuses_LearningWords_LearningWordId",
                        column: x => x.LearningWordId,
                        principalTable: "LearningWords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Corpuses_LearningWordId",
                table: "Corpuses",
                column: "LearningWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Corpuses");

            migrationBuilder.DropTable(
                name: "LearningWords");
        }
    }
}
