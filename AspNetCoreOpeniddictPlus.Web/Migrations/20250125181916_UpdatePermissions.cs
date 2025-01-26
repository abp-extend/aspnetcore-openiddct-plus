using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreOpeniddictPlus.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OpeniddictPlusPermissions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "OpeniddictPlusPermissions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "CreatedByAdmin",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordChangeRequired",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OpeniddictPlusRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeniddictPlusRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeniddictPlusRoles_AspNetRoles_Id",
                        column: x => x.Id,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpeniddictPlusPermissions_RoleId",
                table: "OpeniddictPlusPermissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeniddictPlusPermissions_OpeniddictPlusRoles_RoleId",
                table: "OpeniddictPlusPermissions",
                column: "RoleId",
                principalTable: "OpeniddictPlusRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeniddictPlusPermissions_OpeniddictPlusRoles_RoleId",
                table: "OpeniddictPlusPermissions");

            migrationBuilder.DropTable(
                name: "OpeniddictPlusRoles");

            migrationBuilder.DropIndex(
                name: "IX_OpeniddictPlusPermissions_RoleId",
                table: "OpeniddictPlusPermissions");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "OpeniddictPlusPermissions");

            migrationBuilder.DropColumn(
                name: "CreatedByAdmin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordChangeRequired",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OpeniddictPlusPermissions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");
        }
    }
}
