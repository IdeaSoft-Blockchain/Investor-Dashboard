using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoinDriveICO.DataLayer.Migrations
{
    public partial class DecimalTypesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TypeToTokenConversationRate",
                table: "InnerTransactions",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TokenMultiplier",
                table: "InnerTransactions",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ToValue",
                table: "InnerTransactions",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "FromValue",
                table: "InnerTransactions",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "AspNetUsers",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionValue",
                table: "AffiliatePayoffs",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "PayoffValue",
                table: "AffiliatePayoffs",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "AffiliatePayoffMultiplier",
                table: "AffiliatePayoffs",
                type: "decimal(38,20)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TypeToTokenConversationRate",
                table: "InnerTransactions",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TokenMultiplier",
                table: "InnerTransactions",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ToValue",
                table: "InnerTransactions",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FromValue",
                table: "InnerTransactions",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionValue",
                table: "AffiliatePayoffs",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PayoffValue",
                table: "AffiliatePayoffs",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AffiliatePayoffMultiplier",
                table: "AffiliatePayoffs",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,20)");
        }
    }
}
