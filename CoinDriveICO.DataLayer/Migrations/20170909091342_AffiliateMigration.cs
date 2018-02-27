using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoinDriveICO.DataLayer.Migrations
{
    public partial class AffiliateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AffiliateUserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AffiliateUserId",
                table: "AspNetUsers",
                column: "AffiliateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AffiliateUserId",
                table: "AspNetUsers",
                column: "AffiliateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AffiliateUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AffiliateUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AffiliateUserId",
                table: "AspNetUsers");
        }
    }
}
