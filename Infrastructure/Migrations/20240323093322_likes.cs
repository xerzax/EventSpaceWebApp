using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
	public partial class likes : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Likes",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					PostId = table.Column<int>(type: "int", nullable: false),
					PostType = table.Column<string>(type: "nvarchar(max)", nullable: false),
					BlogId = table.Column<int>(type: "int", nullable: true),
					PhotoId = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Likes", x => x.Id);
					table.ForeignKey(
						name: "FK_Likes_Blog_BlogId",
						column: x => x.BlogId,
						principalTable: "Blog",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Likes_Photo_PhotoId",
						column: x => x.PhotoId,
						principalTable: "Photo",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Likes_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Likes_BlogId",
				table: "Likes",
				column: "BlogId");

			migrationBuilder.CreateIndex(
				name: "IX_Likes_PhotoId",
				table: "Likes",
				column: "PhotoId");

			migrationBuilder.CreateIndex(
				name: "IX_Likes_UserId",
				table: "Likes",
				column: "UserId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Likes");
		}
	}
}