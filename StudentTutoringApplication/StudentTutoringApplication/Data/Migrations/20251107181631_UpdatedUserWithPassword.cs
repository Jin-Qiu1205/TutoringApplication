using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentTutoringApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserWithPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* Removing for testing purposes - issues with migration due to existing column
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Tutors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
            

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Students");
        }
    }
}
