using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressBook.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonalDetailId",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Company_PersonalDetailId",
                table: "Company",
                column: "PersonalDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_PersonalDetails_PersonalDetailId",
                table: "Company",
                column: "PersonalDetailId",
                principalTable: "PersonalDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_PersonalDetails_PersonalDetailId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_PersonalDetailId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PersonalDetailId",
                table: "Company");
        }
    }
}
