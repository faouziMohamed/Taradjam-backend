using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace km.Translate.DataLib.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LanguageShortName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpVotes = table.Column<long>(type: "bigint", nullable: false),
                    DownVotes = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sentences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SrcLanguageId = table.Column<int>(type: "int", nullable: false),
                    SentenceVo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sentences_Languages_SrcLanguageId",
                        column: x => x.SrcLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserDetailsId = table.Column<int>(type: "int", nullable: false),
                    ApprovedPropositionId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_UserDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Propositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SentenceVoId = table.Column<int>(type: "int", nullable: false),
                    TargetLanguageId = table.Column<int>(type: "int", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    VotesId = table.Column<int>(type: "int", nullable: true),
                    TranslatedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslationHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AcceptationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TranslationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TranslatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Propositions_Languages_TargetLanguageId",
                        column: x => x.TargetLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Propositions_Sentences_SentenceId",
                        column: x => x.SentenceId,
                        principalTable: "Sentences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Propositions_Users_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Propositions_Votes_VotesId",
                        column: x => x.VotesId,
                        principalTable: "Votes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Languages_LanguageName_LanguageShortName",
                table: "Languages",
                columns: new[] { "LanguageName", "LanguageShortName" },
                unique: true,
                filter: "[LanguageShortName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_ApprovedById",
                table: "Propositions",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_SentenceId",
                table: "Propositions",
                column: "SentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_TargetLanguageId",
                table: "Propositions",
                column: "TargetLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_TranslationHash",
                table: "Propositions",
                column: "TranslationHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_VotesId",
                table: "Propositions",
                column: "VotesId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true,
                filter: "[RoleName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_SrcLanguageId",
                table: "Sentences",
                column: "SrcLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_Email",
                table: "UserDetails",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_Username",
                table: "UserDetails",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserDetailsId",
                table: "Users",
                column: "UserDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Propositions");

            migrationBuilder.DropTable(
                name: "Sentences");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserDetails");
        }
    }
}
