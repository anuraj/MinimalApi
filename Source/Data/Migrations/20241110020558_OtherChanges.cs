using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MinimalApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class OtherChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<int>(
                name: "PermitLimit",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<int>(
                name: "RateLimitWindowInMinutes",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5192));

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "CreatedOn", "IsCompleted", "Title", "UserId" },
                values: new object[,]
                {
                    { 2, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5230), false, "Todo Item 2", 1 },
                    { 3, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5248), false, "Todo Item 3", 1 },
                    { 4, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5272), false, "Todo Item 4", 1 },
                    { 5, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5394), false, "Todo Item 5", 1 },
                    { 6, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5429), false, "Todo Item 6", 1 },
                    { 7, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5449), false, "Todo Item 7", 1 },
                    { 8, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5488), false, "Todo Item 8", 1 },
                    { 9, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5515), false, "Todo Item 9", 1 },
                    { 10, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5534), false, "Todo Item 10", 1 },
                    { 11, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5572), false, "Todo Item 11", 1 },
                    { 12, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5605), false, "Todo Item 12", 1 },
                    { 13, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5695), false, "Todo Item 13", 1 },
                    { 14, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5739), false, "Todo Item 14", 1 },
                    { 15, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5762), false, "Todo Item 15", 1 },
                    { 16, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5784), false, "Todo Item 16", 1 },
                    { 17, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5801), false, "Todo Item 17", 1 },
                    { 18, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5821), false, "Todo Item 18", 1 },
                    { 19, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5844), false, "Todo Item 19", 1 },
                    { 20, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5863), false, "Todo Item 20", 1 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Email", "Password", "PermitLimit", "RateLimitWindowInMinutes", "Salt", "Username" },
                values: new object[] { new DateTime(2024, 11, 10, 2, 5, 58, 142, DateTimeKind.Utc).AddTicks(8431), "user1@example.com", "R1OP2CHvCht05DPQdNqEebuxg80pbIXpox5S7jieo/U=", 60, 5, "YLi1LuoI+awbyuUy9EqJlA==", "user1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "Email", "Password", "PermitLimit", "RateLimitWindowInMinutes", "Salt", "Username" },
                values: new object[,]
                {
                    { 2, new DateTime(2024, 11, 10, 2, 5, 58, 155, DateTimeKind.Utc).AddTicks(7363), "user2@example.com", "46dOi++b8pp+wnb52kVCUOX+Zz6ZipzvLRaqExHZdyM=", 60, 5, "LbF0O6EJhA+1rWVr7K7iPw==", "user2" },
                    { 3, new DateTime(2024, 11, 10, 2, 5, 58, 168, DateTimeKind.Utc).AddTicks(951), "user3@example.com", "F458HWoWuESQUhDNXH2B8lpoJ5TkZFofHX029/QLvjU=", 60, 5, "Sp3FBJKDR+a3ntfGouOd7Q==", "user3" },
                    { 4, new DateTime(2024, 11, 10, 2, 5, 58, 180, DateTimeKind.Utc).AddTicks(6534), "user4@example.com", "DOYYlKYN/MQ0+0czVKZPMPgySyGWCcRCdkt3VRCTSyU=", 60, 5, "fXn/NJfymsY3fLAmlKjIHA==", "user4" },
                    { 5, new DateTime(2024, 11, 10, 2, 5, 58, 193, DateTimeKind.Utc).AddTicks(123), "user5@example.com", "rCrsXQ7h0hKdR1FvEZgsaC6GpGG7p7cFmNdNNpQOdF4=", 60, 5, "7RueG+tgj5t889UqVnoTGg==", "user5" },
                    { 6, new DateTime(2024, 11, 10, 2, 5, 58, 206, DateTimeKind.Utc).AddTicks(1842), "user6@example.com", "eOSB5NSy440/6kEE8DTsaYrH2g6gPcNlypFeSdciw28=", 60, 5, "rhnWS13/zQmwAsqa2lFaqA==", "user6" },
                    { 7, new DateTime(2024, 11, 10, 2, 5, 58, 218, DateTimeKind.Utc).AddTicks(7315), "user7@example.com", "DlnHhC+Mwz2128rT7lJ5Iwvgca7Ncg44LmcLa5RRu4s=", 60, 5, "ctJtLshAT+Kcgb3aeM4ZMg==", "user7" },
                    { 8, new DateTime(2024, 11, 10, 2, 5, 58, 231, DateTimeKind.Utc).AddTicks(2523), "user8@example.com", "Z1Wa/TAgcIIWdnh30G8u6oex9BrjcQmOzWt31bFQNOc=", 60, 5, "dnzxC3rjGWzBIDk3Gzba8w==", "user8" },
                    { 9, new DateTime(2024, 11, 10, 2, 5, 58, 243, DateTimeKind.Utc).AddTicks(8903), "user9@example.com", "xpp6ajsKviHIAJeyiDLrwsM3cqDBhh0toXdZ3cJlG0M=", 60, 5, "gp66v6DiItk1kpCY0htGIw==", "user9" },
                    { 10, new DateTime(2024, 11, 10, 2, 5, 58, 256, DateTimeKind.Utc).AddTicks(4583), "user10@example.com", "p8ZBK+5Uk64f6M+3lHerqPYuSnBKOzQC9HuYVvn2BZo=", 60, 5, "YaEUy9eBdlNYjA8kk7HUqQ==", "user10" },
                    { 11, new DateTime(2024, 11, 10, 2, 5, 58, 268, DateTimeKind.Utc).AddTicks(9454), "user11@example.com", "+4UVXk0avggCJUPJ+zQkd7OZ1jkCqPNXhS+nzXmAZJ8=", 60, 5, "wpJ6S3NzNnE+6aRlIWhOnA==", "user11" },
                    { 12, new DateTime(2024, 11, 10, 2, 5, 58, 282, DateTimeKind.Utc).AddTicks(1688), "user12@example.com", "B3zrzPvyk1IQVBk/YXaN2cV0IbvBF9XA7RNP6VlmhYs=", 60, 5, "rxcM/rYUj/sgLPjaHcEUgA==", "user12" },
                    { 13, new DateTime(2024, 11, 10, 2, 5, 58, 296, DateTimeKind.Utc).AddTicks(3987), "user13@example.com", "vNYV5ezTys5hwk0vpuLaaDSB9lUmRV3eBuBIlOIsM64=", 60, 5, "FPpKOs/y1Qzv5BWF5IbhTQ==", "user13" },
                    { 14, new DateTime(2024, 11, 10, 2, 5, 58, 312, DateTimeKind.Utc).AddTicks(1762), "user14@example.com", "CjqiVxdsK0cDZ+WECz1boGtMyzk4dyL4by7q36P8GLo=", 60, 5, "nyRTVJ+Fe2AaHxgHD6TRXQ==", "user14" },
                    { 15, new DateTime(2024, 11, 10, 2, 5, 58, 327, DateTimeKind.Utc).AddTicks(4533), "user15@example.com", "tC1JEGTtS8qNHq6Q7tkCzyr0WijB+HBr5xyojIkGsFI=", 60, 5, "Kw8+zJyTYQ2DV900dOz0lQ==", "user15" },
                    { 16, new DateTime(2024, 11, 10, 2, 5, 58, 339, DateTimeKind.Utc).AddTicks(7105), "user16@example.com", "K+w6DavBl3vlgUd5FuzJzytMgiWhfTx4BN9NcZMzkSE=", 60, 5, "a6RXVeCC+bBJ/HXtRA46OQ==", "user16" },
                    { 17, new DateTime(2024, 11, 10, 2, 5, 58, 351, DateTimeKind.Utc).AddTicks(9898), "user17@example.com", "MotSsq6FI+Lj3yGFZ1B8R4Dbe8AvZJIn6wgWuM1IX68=", 60, 5, "47WKubHvPIulISqbb9Hw+w==", "user17" },
                    { 18, new DateTime(2024, 11, 10, 2, 5, 58, 367, DateTimeKind.Utc).AddTicks(4377), "user18@example.com", "qCGn9itDdbQmT8O0DW3s0Fdm87svllcUPyogOb166P8=", 60, 5, "oSCB2cvFYpk2YaUgn0bNUQ==", "user18" },
                    { 19, new DateTime(2024, 11, 10, 2, 5, 58, 382, DateTimeKind.Utc).AddTicks(2281), "user19@example.com", "AMJCNcRtbvfrQB5Gp73oEafrEeyq4ZbhmTneMDzE8Do=", 60, 5, "oO14BlJIJK3ieAU8c78zHw==", "user19" },
                    { 20, new DateTime(2024, 11, 10, 2, 5, 58, 394, DateTimeKind.Utc).AddTicks(5115), "user20@example.com", "3MPWg8I6mRwDpqUsKPfSFJZli50H/WSMHhOC/9W+/u8=", 60, 5, "IpdtDwe5x+csCHuyJcHsMA==", "user20" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "PermitLimit",
                table: "Users")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "RateLimitWindowInMinutes",
                table: "Users")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "UsersHistory")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2021, 12, 1, 5, 47, 6, 969, DateTimeKind.Utc).AddTicks(6540));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Email", "Password", "Username" },
                values: new object[] { new DateTime(2021, 12, 1, 5, 47, 6, 969, DateTimeKind.Utc).AddTicks(6423), "admin@example.com", "admin", "admin" });
        }
    }
}
