using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WebApi.POC.Migrations
{
    public partial class V0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "key_kind",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key_kind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "crypto_keys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    KindId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crypto_keys", x => new { x.Id, x.KindId });
                    table.ForeignKey(
                        name: "FK_crypto_keys_key_kind_KindId",
                        column: x => x.KindId,
                        principalTable: "key_kind",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Password = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "demands",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false),
                    LastEdit = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    PicturePath = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_demands_user_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_demands_status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_crypto_keys_KindId",
                table: "crypto_keys",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_demands_OwnerId",
                table: "demands",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_demands_StatusId",
                table: "demands",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId",
                table: "user",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "crypto_keys");

            migrationBuilder.DropTable(
                name: "demands");

            migrationBuilder.DropTable(
                name: "key_kind");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
