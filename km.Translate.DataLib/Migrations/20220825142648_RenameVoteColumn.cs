using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace km.Translate.DataLib.Migrations
{
    public partial class RenameVoteColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownVotes",
                table: "Propositions");

            migrationBuilder.RenameColumn(
                name: "UpVotes",
                table: "Propositions",
                newName: "Votes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Votes",
                table: "Propositions",
                newName: "UpVotes");

            migrationBuilder.AddColumn<long>(
                name: "DownVotes",
                table: "Propositions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
