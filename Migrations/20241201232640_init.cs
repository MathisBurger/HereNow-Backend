using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresenceBackend.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserRoles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Protocols",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Protocols_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClockIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClockOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatuses_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProtocolUsers",
                columns: table => new
                {
                    InvolvedInId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvolvedUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolUsers", x => new { x.InvolvedInId, x.InvolvedUsersId });
                    table.ForeignKey(
                        name: "FK_ProtocolUsers_Protocols_InvolvedInId",
                        column: x => x.InvolvedInId,
                        principalTable: "Protocols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtocolUsers_Users_InvolvedUsersId",
                        column: x => x.InvolvedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_CreatorId",
                table: "Protocols",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolUsers_InvolvedUsersId",
                table: "ProtocolUsers",
                column: "InvolvedUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefreshToken",
                table: "Users",
                column: "RefreshToken",
                unique: true,
                filter: "[RefreshToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_OwnerId",
                table: "UserStatuses",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_UserId",
                table: "UserStatuses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtocolUsers");

            migrationBuilder.DropTable(
                name: "UserStatuses");

            migrationBuilder.DropTable(
                name: "Protocols");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
