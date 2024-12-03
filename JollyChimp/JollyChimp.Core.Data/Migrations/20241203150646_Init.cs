using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JollyChimp.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "JollyChimp");

            migrationBuilder.CreateTable(
                name: "Configurations",
                schema: "JollyChimp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DiscoveryService = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeleteQueue",
                schema: "JollyChimp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XabarilTableName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    XabarilId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteQueue", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EndPoints",
                schema: "JollyChimp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiPath = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    HealthChecksPredicate = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndPoints", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Executions",
                schema: "JollyChimp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OnStateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastExecuted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DiscoveryService = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Failures",
                schema: "JollyChimp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthCheckName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LastNotified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUpAndRunning = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Failures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealthChecks",
                schema: "JollyChimp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthChecks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ServerSettings",
                schema: "JollyChimp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WebHooks",
                schema: "JollyChimp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHooks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckExecutionEntries",
                schema: "JollyChimp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthCheckExecutionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckExecutionEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckExecutionEntries_Executions_HealthCheckExecutionId",
                        column: x => x.HealthCheckExecutionId,
                        principalSchema: "JollyChimp",
                        principalTable: "Executions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckExecutionHistories",
                schema: "JollyChimp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    On = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HealthCheckExecutionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckExecutionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckExecutionHistories_Executions_HealthCheckExecutionId",
                        column: x => x.HealthCheckExecutionId,
                        principalSchema: "JollyChimp",
                        principalTable: "Executions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckParameters",
                schema: "JollyChimp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    HealthCheckId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckParameters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HealthCheckParameters_HealthChecks_HealthCheckId",
                        column: x => x.HealthCheckId,
                        principalSchema: "JollyChimp",
                        principalTable: "HealthChecks",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "WebHookParameters",
                schema: "JollyChimp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    WebHookId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHookParameters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebHookParameters_WebHooks_WebHookId",
                        column: x => x.WebHookId,
                        principalSchema: "JollyChimp",
                        principalTable: "WebHooks",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                schema: "JollyChimp",
                table: "ServerSettings",
                columns: new[] { "ID", "IsDeleted", "IsEnabled", "Name", "Value" },
                values: new object[,]
                {
                    { 1, false, true, "EvaluationTimeInSeconds", "10" },
                    { 2, false, true, "ApiMaxActiveRequests", "50" },
                    { 3, false, true, "MaximumHistoryEntriesPerEndpoint", "50" },
                    { 4, false, true, "HeaderText", "Health Checks Dashboard" },
                    { 5, false, true, "MinimumSecondsBetweenFailureNotifications", "30" },
                    { 6, false, true, "NotifyUnHealthyOneTimeUntilChange", "false" },
                    { 7, false, true, "UiRoutePath", "/hc-ui" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckExecutionEntries_HealthCheckExecutionId",
                schema: "JollyChimp",
                table: "HealthCheckExecutionEntries",
                column: "HealthCheckExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckExecutionHistories_HealthCheckExecutionId",
                schema: "JollyChimp",
                table: "HealthCheckExecutionHistories",
                column: "HealthCheckExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckParameters_HealthCheckId",
                schema: "JollyChimp",
                table: "HealthCheckParameters",
                column: "HealthCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_WebHookParameters_WebHookId",
                schema: "JollyChimp",
                table: "WebHookParameters",
                column: "WebHookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "DeleteQueue",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "EndPoints",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "Failures",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "HealthCheckExecutionEntries",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "HealthCheckExecutionHistories",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "HealthCheckParameters",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "ServerSettings",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "WebHookParameters",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "Executions",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "HealthChecks",
                schema: "JollyChimp");

            migrationBuilder.DropTable(
                name: "WebHooks",
                schema: "JollyChimp");
        }
    }
}
