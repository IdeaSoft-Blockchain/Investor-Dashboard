using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoinDriveICO.DataLayer.Migrations
{
    public partial class TransactionFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InnerTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssociatedTransactionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromValue = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ToValue = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TokenMultiplier = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TypeToTokenConversationRate = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InnerTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InnerTransactions_Transactions_AssociatedTransactionId",
                        column: x => x.AssociatedTransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TxKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InnerTransactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Id",
                table: "AspNetUsers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InnerTransactions_AssociatedTransactionId",
                table: "InnerTransactions",
                column: "AssociatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_InnerTransactions_UserId",
                table: "InnerTransactions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InnerTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "Transactions");
        }
    }
}
