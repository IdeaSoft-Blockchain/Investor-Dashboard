using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoinDriveICO.DataLayer.Migrations
{
    public partial class AffiliatePayoffsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AffiliatePayoffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AffiliatePayoffMultiplier = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    AffiliateUserId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnerTransactionId = table.Column<int>(type: "int", nullable: false),
                    PayingUserId = table.Column<int>(type: "int", nullable: false),
                    PayoffValue = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TransactionValue = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliatePayoffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffiliatePayoffs_AspNetUsers_AffiliateUserId",
                        column: x => x.AffiliateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AffiliatePayoffs_InnerTransactions_InnerTransactionId",
                        column: x => x.InnerTransactionId,
                        principalTable: "InnerTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AffiliatePayoffs_AspNetUsers_PayingUserId",
                        column: x => x.PayingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AffiliatePayoffs_AffiliateUserId",
                table: "AffiliatePayoffs",
                column: "AffiliateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliatePayoffs_InnerTransactionId",
                table: "AffiliatePayoffs",
                column: "InnerTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliatePayoffs_PayingUserId",
                table: "AffiliatePayoffs",
                column: "PayingUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AffiliatePayoffs");
        }
    }
}
