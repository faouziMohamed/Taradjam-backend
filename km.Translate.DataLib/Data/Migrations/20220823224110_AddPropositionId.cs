using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace km.Translate.DataLib.Data.Migrations
{
    public partial class AddPropositionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropositionId",
                table: "Sentences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropositionId",
                table: "Sentences");
        }
    }
}
