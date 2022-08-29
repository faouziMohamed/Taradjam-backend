using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace km.Translate.DataLib.Migrations
{
    public partial class UpdatePropositionIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Propositions_TranslationHash",
                table: "Propositions");

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_TranslationHash_SentenceVoId_TranslationLangId",
                table: "Propositions",
                columns: new[] { "TranslationHash", "SentenceVoId", "TranslationLangId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Propositions_TranslationHash_SentenceVoId_TranslationLangId",
                table: "Propositions");

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_TranslationHash",
                table: "Propositions",
                column: "TranslationHash",
                unique: true);
        }
    }
}
