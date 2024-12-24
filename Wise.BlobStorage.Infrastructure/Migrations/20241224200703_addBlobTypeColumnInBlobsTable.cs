using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wise.BlobStorage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addBlobTypeColumnInBlobsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlobType",
                table: "Blobs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobType",
                table: "Blobs");
        }
    }
}
