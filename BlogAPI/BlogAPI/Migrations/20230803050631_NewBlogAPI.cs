using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewBlogAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionDetails",
                columns: table => new
                {
                    ActionUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionFor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLikeUnlikeComment = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionDetails", x => new { x.ActionUserID, x.ActionBy, x.ActionFor, x.IsLikeUnlikeComment, x.Comment, x.CreateDate, x.IsActive });
                });

            migrationBuilder.CreateTable(
                name: "BlogMaster",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountLikes = table.Column<int>(type: "int", nullable: false),
                    CountUnLikes = table.Column<int>(type: "int", nullable: false),
                    CountViews = table.Column<int>(type: "int", nullable: false),
                    CountComments = table.Column<int>(type: "int", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogMaster", x => new { x.UserID, x.Title, x.Description, x.Filename, x.CountViews, x.CountLikes, x.CountUnLikes, x.CountComments, x.CreateBy, x.IsActive });
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Geneder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProfileImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.UserID, x.FirstName, x.LastName, x.Email, x.MobileNumber, x.Geneder, x.Password, x.IsActive });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionDetails");

            migrationBuilder.DropTable(
                name: "BlogMaster");

            migrationBuilder.DropTable(
                name: "UserLogin");
        }
    }
}
