using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRM.Core.Migrations
{
    public partial class mg5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DoB",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHasAvatar",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountDomainId = table.Column<string>(nullable: true),
                    ActivityDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Resource = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    WorkingOffice = table.Column<string>(nullable: true),
                    SeatingPlan = table.Column<string>(nullable: true),
                    WorkLocation = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Timesheets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TotalHour = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsChange = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timesheets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimesheetId = table.Column<int>(nullable: false),
                    AccountDomainId = table.Column<string>(nullable: true),
                    ActivityId = table.Column<string>(nullable: true),
                    Task = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    IsEdit = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    IsHoliday = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimesheetTask_AccountDomains_AccountDomainId",
                        column: x => x.AccountDomainId,
                        principalTable: "AccountDomains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimesheetTask_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimesheetTask_Timesheets_TimesheetId",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskHour",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeSheetTaskId = table.Column<int>(nullable: false),
                    WorkingDate = table.Column<DateTime>(nullable: false),
                    WorkingHour = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskHour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskHour_TimesheetTask_TimeSheetTaskId",
                        column: x => x.TimeSheetTaskId,
                        principalTable: "TimesheetTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHour_TimeSheetTaskId",
                table: "TaskHour",
                column: "TimeSheetTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_UserId",
                table: "Timesheets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetTask_AccountDomainId",
                table: "TimesheetTask",
                column: "AccountDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetTask_ActivityId",
                table: "TimesheetTask",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetTask_TimesheetId",
                table: "TimesheetTask",
                column: "TimesheetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "TaskHour");

            migrationBuilder.DropTable(
                name: "TimesheetTask");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Timesheets");

            migrationBuilder.DropColumn(
                name: "DoB",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsHasAvatar",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
