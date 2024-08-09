using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CitiesManager.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("582d7597-e6d5-46ef-94b6-cf4a50418b0c"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("89f7b38a-d30b-4a10-8668-11ad8d4acfa9"));

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1735a681-c90b-4c50-88d8-a27df26e995f"), "London" },
                    { new Guid("5a91fd62-9c0c-404a-8881-2313f7da1e95"), "New York" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("1735a681-c90b-4c50-88d8-a27df26e995f"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("5a91fd62-9c0c-404a-8881-2313f7da1e95"));

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("582d7597-e6d5-46ef-94b6-cf4a50418b0c"), null },
                    { new Guid("89f7b38a-d30b-4a10-8668-11ad8d4acfa9"), null }
                });
        }
    }
}
