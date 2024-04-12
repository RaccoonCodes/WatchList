using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchList.Migrations
{
    public partial class Initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersDBModels",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDBModels", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "SeriesInfos",
                columns: table => new
                {
                    SeriesInfoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TitleWatched = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeasonWatched = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderWatched = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesInfos", x => x.SeriesInfoID);
                    table.ForeignKey(
                        name: "FK_SeriesInfos_UsersDBModels_UserID",
                        column: x => x.UserID,
                        principalTable: "UsersDBModels",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeriesInfos_UserID",
                table: "SeriesInfos",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeriesInfos");

            migrationBuilder.DropTable(
                name: "UsersDBModels");
        }
    }
}
