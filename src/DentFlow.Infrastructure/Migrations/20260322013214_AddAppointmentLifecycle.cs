using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "appointments",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "appointment_status_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatus = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ToStatus = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ChangedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsOverride = table.Column<bool>(type: "boolean", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment_status_history", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointment_status_history_TenantId_AppointmentId",
                table: "appointment_status_history",
                columns: new[] { "TenantId", "AppointmentId" });

            // Data migration: normalise legacy statuses to the new 6-status lifecycle.
            // Confirmed / Rescheduled → Scheduled (no action needed, keep as is)
            // InChair → InProgress (set StartedAt = CheckedInAt as best estimate)
            migrationBuilder.Sql(@"
                UPDATE appointments
                SET ""Status"" = 'Scheduled'
                WHERE ""Status"" IN ('Confirmed', 'Rescheduled');

                UPDATE appointments
                SET ""Status"" = 'InProgress',
                    ""StartedAt"" = ""CheckedInAt""
                WHERE ""Status"" = 'InChair';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointment_status_history");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "appointments");
        }
    }
}
