using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.WebApi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "todos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users_todos",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    todo_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_todos", x => new { x.user_id, x.todo_id });
                    table.ForeignKey(
                        name: "FK_users_todos_todos_todo_id",
                        column: x => x.todo_id,
                        principalTable: "todos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_todos_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_todos_todo_id",
                table: "users_todos",
                column: "todo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_todos");

            migrationBuilder.DropTable(
                name: "todos");
        }
    }
}
