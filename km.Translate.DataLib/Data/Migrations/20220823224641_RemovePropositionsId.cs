using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace km.Translate.DataLib.Data.Migrations
{
    public partial class RemovePropositionsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropositionsId",
                table: "Sentences");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropositionsId",
                table: "Sentences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
