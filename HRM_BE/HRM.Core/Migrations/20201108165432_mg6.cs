using Microsoft.EntityFrameworkCore.Migrations;

namespace HRM.Core.Migrations
{
    public partial class mg6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskHour_TimesheetTask_TimeSheetTaskId",
                table: "TaskHour");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetTask_AccountDomains_AccountDomainId",
                table: "TimesheetTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetTask_Activity_ActivityId",
                table: "TimesheetTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetTask_Timesheets_TimesheetId",
                table: "TimesheetTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimesheetTask",
                table: "TimesheetTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "TimesheetTask",
                newName: "TimesheetTasks");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_TimesheetTask_TimesheetId",
                table: "TimesheetTasks",
                newName: "IX_TimesheetTasks_TimesheetId");

            migrationBuilder.RenameIndex(
                name: "IX_TimesheetTask_ActivityId",
                table: "TimesheetTasks",
                newName: "IX_TimesheetTasks_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_TimesheetTask_AccountDomainId",
                table: "TimesheetTasks",
                newName: "IX_TimesheetTasks_AccountDomainId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimesheetTasks",
                table: "TimesheetTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHour_TimesheetTasks_TimeSheetTaskId",
                table: "TaskHour",
                column: "TimeSheetTaskId",
                principalTable: "TimesheetTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetTasks_AccountDomains_AccountDomainId",
                table: "TimesheetTasks",
                column: "AccountDomainId",
                principalTable: "AccountDomains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetTasks_Activities_ActivityId",
                table: "TimesheetTasks",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetTasks_Timesheets_TimesheetId",
                table: "TimesheetTasks",
                column: "TimesheetId",
                principalTable: "Timesheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskHour_TimesheetTasks_TimeSheetTaskId",
                table: "TaskHour");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetTasks_AccountDomains_AccountDomainId",
                table: "TimesheetTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetTasks_Activities_ActivityId",
                table: "TimesheetTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetTasks_Timesheets_TimesheetId",
                table: "TimesheetTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimesheetTasks",
                table: "TimesheetTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "TimesheetTasks",
                newName: "TimesheetTask");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_TimesheetTasks_TimesheetId",
                table: "TimesheetTask",
                newName: "IX_TimesheetTask_TimesheetId");

            migrationBuilder.RenameIndex(
                name: "IX_TimesheetTasks_ActivityId",
                table: "TimesheetTask",
                newName: "IX_TimesheetTask_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_TimesheetTasks_AccountDomainId",
                table: "TimesheetTask",
                newName: "IX_TimesheetTask_AccountDomainId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimesheetTask",
                table: "TimesheetTask",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHour_TimesheetTask_TimeSheetTaskId",
                table: "TaskHour",
                column: "TimeSheetTaskId",
                principalTable: "TimesheetTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetTask_AccountDomains_AccountDomainId",
                table: "TimesheetTask",
                column: "AccountDomainId",
                principalTable: "AccountDomains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetTask_Activity_ActivityId",
                table: "TimesheetTask",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetTask_Timesheets_TimesheetId",
                table: "TimesheetTask",
                column: "TimesheetId",
                principalTable: "Timesheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
