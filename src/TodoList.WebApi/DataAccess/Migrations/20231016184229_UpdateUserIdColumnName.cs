using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.WebApi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIdColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "Id");
        }
    }
}
