using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordBucket.Migrations.User
{
    /// <inheritdoc />
    public partial class SetupCorpusWordRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Corpuses_LearningWords_LearningWordId",
                table: "Corpuses");

            migrationBuilder.DropIndex(
                name: "IX_Corpuses_LearningWordId",
                table: "Corpuses");

            migrationBuilder.DropColumn(
                name: "LearningWordId",
                table: "Corpuses");

            migrationBuilder.CreateTable(
                name: "CorpusLearningWord",
                columns: table => new
                {
                    CorpusesId = table.Column<int>(type: "INTEGER", nullable: false),
                    LearningWordsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorpusLearningWord", x => new { x.CorpusesId, x.LearningWordsId });
                    table.ForeignKey(
                        name: "FK_CorpusLearningWord_Corpuses_CorpusesId",
                        column: x => x.CorpusesId,
                        principalTable: "Corpuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorpusLearningWord_LearningWords_LearningWordsId",
                        column: x => x.LearningWordsId,
                        principalTable: "LearningWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorpusLearningWord_LearningWordsId",
                table: "CorpusLearningWord",
                column: "LearningWordsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorpusLearningWord");

            migrationBuilder.AddColumn<int>(
                name: "LearningWordId",
                table: "Corpuses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Corpuses_LearningWordId",
                table: "Corpuses",
                column: "LearningWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Corpuses_LearningWords_LearningWordId",
                table: "Corpuses",
                column: "LearningWordId",
                principalTable: "LearningWords",
                principalColumn: "Id");
        }
    }
}
