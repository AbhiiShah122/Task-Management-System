using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FirstCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAttachments",
                columns: table => new
                {
                    TaskAttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttachments", x => x.TaskAttachmentId);
                    table.ForeignKey(
                        name: "FK_TaskAttachments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskNotes",
                columns: table => new
                {
                    TaskNoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskNotes", x => x.TaskNoteId);
                    table.ForeignKey(
                        name: "FK_TaskNotes_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Employee" },
                    { 2, "Manager" },
                    { 3, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "TeamId", "TeamName" },
                values: new object[,]
                {
                    { 1, "Development" },
                    { 2, "Marketing" },
                    { 3, "Sales" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Discriminator", "Email", "FirstName", "LastName", "Password", "RoleId", "TeamId" },
                values: new object[,]
                {
                    { 1, "Admin", "admin@gmail.com", "Admin", "Admin", ";M8ig{QV", 3, null },
                    { 6, "Manager", "chiman.patel@gmail.com", "Chiman", "Patel", "Y,WkQRp]", 2, 1 },
                    { 7, "Manager", "prakash.shah@gmail.com", "Prakash", "Shah", "]P3$SlJF", 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Discriminator", "Email", "FirstName", "LastName", "ManagerId", "Password", "RoleId", "TeamId" },
                values: new object[,]
                {
                    { 2, "Employee", "abhi.shah@gmail.com", "Abhi", "Shah", 6, "S{7ZA[k%", 1, 1 },
                    { 3, "Employee", "parth.shah@gmail.com", "Parth", "Shah", 6, "t;5KJKr1", 1, 1 },
                    { 4, "Employee", "jay.shah@gmail.com", "Jay", "Shah", 7, "SVRB.e_]", 1, 2 },
                    { 5, "Employee", "smit.shah@gmail.com", "Smit", "Shah", 7, "r[-A!L+!", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskId", "CompletedOn", "Description", "DueDate", "EmployeeId", "IsCompleted", "Title" },
                values: new object[,]
                {
                    { 1, null, "Develop backend", new DateTime(2024, 7, 21, 11, 7, 13, 413, DateTimeKind.Utc).AddTicks(5504), 2, false, "Task 1" },
                    { 2, null, "Develop frontend (optional)", new DateTime(2024, 7, 22, 11, 7, 13, 413, DateTimeKind.Utc).AddTicks(5517), 3, false, "Task 2" },
                    { 3, null, "Add Authentication", new DateTime(2024, 7, 21, 11, 7, 13, 413, DateTimeKind.Utc).AddTicks(5519), 2, false, "Task 1" },
                    { 4, null, "Add Unit Test", new DateTime(2024, 7, 22, 11, 7, 13, 413, DateTimeKind.Utc).AddTicks(5520), 3, false, "Task 2" }
                });

            migrationBuilder.InsertData(
                table: "TaskAttachments",
                columns: new[] { "TaskAttachmentId", "FileName", "FilePath", "TaskId" },
                values: new object[,]
                {
                    { 1, "Attachment1.txt", "Desktop", 1 },
                    { 2, "Attachment2.jpg", "Desktop", 1 },
                    { 3, "Attachment3.jpg", "Desktop", 2 },
                    { 4, "Attachment4.jpg", "Desktop", 2 },
                    { 5, "Attachment5.jpg", "Desktop", 3 },
                    { 6, "Attachment6.jpg", "Desktop", 4 }
                });

            migrationBuilder.InsertData(
                table: "TaskNotes",
                columns: new[] { "TaskNoteId", "Note", "TaskId" },
                values: new object[,]
                {
                    { 1, "Note 1 for Task 1", 1 },
                    { 2, "Note 2 for Task 1", 1 },
                    { 3, "Note 2 for Task 1", 2 },
                    { 4, "Note 2 for Task 1", 2 },
                    { 5, "Note 2 for Task 1", 3 },
                    { 6, "Note 2 for Task 1", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachments_TaskId",
                table: "TaskAttachments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskNotes_TaskId",
                table: "TaskNotes",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EmployeeId",
                table: "Tasks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerId",
                table: "Users",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAttachments");

            migrationBuilder.DropTable(
                name: "TaskNotes");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
