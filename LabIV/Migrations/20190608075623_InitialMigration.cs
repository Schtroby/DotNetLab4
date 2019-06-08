using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabIV.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.CreateTable(
        //        name: "Users",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
        //            FirstName = table.Column<string>(nullable: true),
        //            LastName = table.Column<string>(nullable: true),
        //            Username = table.Column<string>(nullable: true),
        //            Email = table.Column<string>(nullable: true),
        //            Password = table.Column<string>(nullable: true),
        //            UserRole = table.Column<int>(nullable: false),
        //            RegistrationDate = table.Column<DateTime>(nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Users", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Tasks",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
        //            Title = table.Column<string>(nullable: true),
        //            Description = table.Column<string>(nullable: true),
        //            DateAdded = table.Column<DateTime>(nullable: false),
        //            Deadline = table.Column<DateTime>(nullable: false),
        //            TaskImportance = table.Column<int>(nullable: false),
        //            TaskState = table.Column<int>(nullable: false),
        //            DateClosed = table.Column<DateTime>(nullable: true),
        //            OwnerId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Tasks", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Tasks_Users_OwnerId",
        //                column: x => x.OwnerId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Comments",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
        //            Text = table.Column<string>(nullable: true),
        //            Important = table.Column<bool>(nullable: false),
        //            TaskId = table.Column<int>(nullable: true),
        //            OwnerId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Comments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Comments_Users_OwnerId",
        //                column: x => x.OwnerId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Comments_Tasks_TaskId",
        //                column: x => x.TaskId,
        //                principalTable: "Tasks",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Comments_OwnerId",
        //        table: "Comments",
        //        column: "OwnerId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Comments_TaskId",
        //        table: "Comments",
        //        column: "TaskId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Tasks_OwnerId",
        //        table: "Tasks",
        //        column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
