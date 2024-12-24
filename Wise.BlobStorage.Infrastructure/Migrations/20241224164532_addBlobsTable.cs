using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wise.BlobStorage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addBlobsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContainerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BlobName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ActionUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blobs");
        }
    }
}
