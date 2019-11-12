using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Travel.Shop.Back.Migrations
{
    public partial class AddOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Tourists",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DateOf = table.Column<DateTime>(nullable: false),
                    TourId = table.Column<int>(nullable: false),
                    ManagerId = table.Column<int>(nullable: false),
                    ManagerId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ManagerId1",
                        column: x => x.ManagerId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tourists_OrderId",
                table: "Tourists",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ManagerId1",
                table: "Orders",
                column: "ManagerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TourId",
                table: "Orders",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tourists_Orders_OrderId",
                table: "Tourists",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tourists_Orders_OrderId",
                table: "Tourists");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Tourists_OrderId",
                table: "Tourists");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Tourists");
        }
    }
}
