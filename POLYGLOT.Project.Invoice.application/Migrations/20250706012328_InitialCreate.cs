using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POLYGLOT.Project.Invoice.application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "invoices_idinvoice_seq");

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    idInvoice = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('invoices_idinvoice_seq'::regclass)"),
                    amount = table.Column<float>(type: "real", nullable: true),
                    state = table.Column<bool>(type: "boolean", nullable: true),
                    secuencial = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    paid = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("invoices_pkey", x => x.idInvoice);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropSequence(
                name: "invoices_idinvoice_seq");
        }
    }
}
