using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailingSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DraftConversationAndAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Messages_LastMessageId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftConversations_AspNetUsers_ReceiverId",
                table: "DraftConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftConversations_AspNetUsers_SenderId",
                table: "DraftConversations");

            migrationBuilder.DropTable(
                name: "UserInboxes");

            migrationBuilder.DropTable(
                name: "UserSents");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_LastMessageId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Conversations");

            migrationBuilder.CreateTable(
                name: "DraftAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DraftConversationId = table.Column<long>(type: "bigint", nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftAttachments_DraftConversations_DraftConversationId",
                        column: x => x.DraftConversationId,
                        principalTable: "DraftConversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftAttachments_DraftConversationId",
                table: "DraftAttachments",
                column: "DraftConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftConversations_AspNetUsers_ReceiverId",
                table: "DraftConversations",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftConversations_AspNetUsers_SenderId",
                table: "DraftConversations",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftConversations_AspNetUsers_ReceiverId",
                table: "DraftConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftConversations_AspNetUsers_SenderId",
                table: "DraftConversations");

            migrationBuilder.DropTable(
                name: "DraftAttachments");

            migrationBuilder.AddColumn<long>(
                name: "LastMessageId",
                table: "Conversations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserInboxes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConversationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInboxes", x => new { x.UserId, x.ConversationId });
                    table.ForeignKey(
                        name: "FK_UserInboxes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInboxes_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSents",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConversationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSents", x => new { x.UserId, x.ConversationId });
                    table.ForeignKey(
                        name: "FK_UserSents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSents_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_LastMessageId",
                table: "Conversations",
                column: "LastMessageId",
                unique: true,
                filter: "[LastMessageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserInboxes_ConversationId",
                table: "UserInboxes",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSents_ConversationId",
                table: "UserSents",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Messages_LastMessageId",
                table: "Conversations",
                column: "LastMessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftConversations_AspNetUsers_ReceiverId",
                table: "DraftConversations",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftConversations_AspNetUsers_SenderId",
                table: "DraftConversations",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
