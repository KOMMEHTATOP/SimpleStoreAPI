using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class CheckDifferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Удаляем FK constraints
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");
                
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId", 
                table: "OrderItems");

            // 2. Удаляем Primary Keys
            migrationBuilder.DropPrimaryKey(name: "PK_Products", table: "Products");
            migrationBuilder.DropPrimaryKey(name: "PK_Orders", table: "Orders");
            migrationBuilder.DropPrimaryKey(name: "PK_OrderItems", table: "OrderItems");

            // 3. Удаляем индексы
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems");
                
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductId", 
                table: "OrderItems");

            // 4. Добавляем новые string колонки
            migrationBuilder.AddColumn<string>(
                name: "NewId",
                table: "Products", 
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
                
            migrationBuilder.AddColumn<string>(
                name: "NewId",
                table: "Orders",
                type: "nvarchar(450)", 
                nullable: false,
                defaultValue: "");
                
            migrationBuilder.AddColumn<string>(
                name: "NewId",
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: false, 
                defaultValue: "");
                
            migrationBuilder.AddColumn<string>(
                name: "NewOrderId",
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
                
            migrationBuilder.AddColumn<string>(
                name: "NewProductId", 
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            // 5. Генерируем GUID для существующих записей
            migrationBuilder.Sql(@"
                UPDATE Products SET NewId = NEWID();
                UPDATE Orders SET NewId = NEWID(); 
                UPDATE OrderItems SET NewId = NEWID();
                UPDATE OrderItems SET NewOrderId = (SELECT o.NewId FROM Orders o WHERE o.Id = OrderItems.OrderId);
                UPDATE OrderItems SET NewProductId = (SELECT p.NewId FROM Products p WHERE p.Id = OrderItems.ProductId);
            ");

            // 6. Удаляем старые колонки
            migrationBuilder.DropColumn(name: "Id", table: "Products");
            migrationBuilder.DropColumn(name: "Id", table: "Orders");
            migrationBuilder.DropColumn(name: "Id", table: "OrderItems");
            migrationBuilder.DropColumn(name: "OrderId", table: "OrderItems");
            migrationBuilder.DropColumn(name: "ProductId", table: "OrderItems");

            // 7. Переименовываем новые колонки
            migrationBuilder.RenameColumn(table: "Products", name: "NewId", newName: "Id");
            migrationBuilder.RenameColumn(table: "Orders", name: "NewId", newName: "Id");
            migrationBuilder.RenameColumn(table: "OrderItems", name: "NewId", newName: "Id");
            migrationBuilder.RenameColumn(table: "OrderItems", name: "NewOrderId", newName: "OrderId");
            migrationBuilder.RenameColumn(table: "OrderItems", name: "NewProductId", newName: "ProductId");

            // 8. Создаем новые Primary Keys
            migrationBuilder.AddPrimaryKey(name: "PK_Products", table: "Products", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_Orders", table: "Orders", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_OrderItems", table: "OrderItems", column: "Id");

            // 9. Восстанавливаем Foreign Keys с правильными настройками CASCADE
            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId", 
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
                
            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems", 
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);  // ← ИСПРАВЛЕНО: NoAction вместо Cascade

            // 10. Восстанавливаем индексы
            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
                
            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems", 
                column: "ProductId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
