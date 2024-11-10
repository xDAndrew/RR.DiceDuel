using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.DiceDuel.ExternalServices.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddStatisticAndConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomMaxPlayer = table.Column<int>(type: "integer", nullable: false),
                    MaxGameRound = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.Id);
                });
            
            migrationBuilder.InsertData(
                table: "Config",
                columns: new[] { "RoomMaxPlayer", "MaxGameRound" },
                values: new object[] { 2, 3 }
            );
            
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GamesCount = table.Column<long>(type: "bigint", nullable: false),
                    Wins = table.Column<long>(type: "bigint", nullable: false),
                    Draw = table.Column<long>(type: "bigint", nullable: false),
                    Defeats = table.Column<long>(type: "bigint", nullable: false),
                    NormalRolled = table.Column<long>(type: "bigint", nullable: false),
                    SpecialRolled = table.Column<long>(type: "bigint", nullable: false),
                    GotZeroScore = table.Column<long>(type: "bigint", nullable: false),
                    GotMaxScore = table.Column<long>(type: "bigint", nullable: false),
                    TotalScores = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "Statistics");
        }
    }
}
