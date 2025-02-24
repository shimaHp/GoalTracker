using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsersAddedToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssigneeId",
                table: "WorkItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "WorkItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql("UPDATE WorkItems SET CreatorId = (SELECT TOP 1 Id FROM AspNetUsers)");

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "WorkItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Goals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql("UPDATE Goals SET UserId = '7bca4b98-ae49-4d7d-84d2-e14dab20519d'");


            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_AssigneeId",
                table: "WorkItems",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_CreatorId",
                table: "WorkItems",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_LastUpdatedById",
                table: "WorkItems",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                table: "Goals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_AspNetUsers_AssigneeId",
                table: "WorkItems",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_AspNetUsers_CreatorId",
                table: "WorkItems",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_AspNetUsers_LastUpdatedById",
                table: "WorkItems",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_AspNetUsers_UserId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_AspNetUsers_AssigneeId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_AspNetUsers_CreatorId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_AspNetUsers_LastUpdatedById",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_AssigneeId",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_CreatorId",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_LastUpdatedById",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_Goals_UserId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Goals");
        }
    }
}
