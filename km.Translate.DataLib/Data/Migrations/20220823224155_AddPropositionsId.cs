using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace km.Translate.DataLib.Data.Migrations
{
    public partial class AddPropositionsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropositionId",
                table: "Sentences",
                newName: "PropositionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropositionsId",
                table: "Sentences",
                newName: "PropositionId");
        }
    }
}
