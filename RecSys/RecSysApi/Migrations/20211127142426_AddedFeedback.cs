using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecSysApi.Presentation.Migrations
{
    public partial class AddedFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QueryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedback_queries_QueryId",
                        column: x => x.QueryId,
                        principalTable: "queries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feedback_videosUPV_VideoId",
                        column: x => x.VideoId,
                        principalTable: "videosUPV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedback_QueryId",
                table: "feedback",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_VideoId",
                table: "feedback",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback");
        }
    }
}
