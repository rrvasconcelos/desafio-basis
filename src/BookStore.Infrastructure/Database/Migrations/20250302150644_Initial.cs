using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookStore.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "authors",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "books",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    publisher = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    edition = table.Column<int>(type: "integer", nullable: false),
                    publication_year = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "book_authors",
                schema: "public",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    author_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_book_authors", x => new { x.book_id, x.author_id });
                    table.ForeignKey(
                        name: "fk_book_authors_authors_author_id",
                        column: x => x.author_id,
                        principalSchema: "public",
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_book_authors_books_book_id",
                        column: x => x.book_id,
                        principalSchema: "public",
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "book_prices",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    purchase_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_book_prices", x => x.id);
                    table.ForeignKey(
                        name: "fk_book_prices_books_book_id",
                        column: x => x.book_id,
                        principalSchema: "public",
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "book_subjects",
                schema: "public",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    subject_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_book_subjects", x => new { x.book_id, x.subject_id });
                    table.ForeignKey(
                        name: "fk_book_subjects_books_book_id",
                        column: x => x.book_id,
                        principalSchema: "public",
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_book_subjects_subjects_subject_id",
                        column: x => x.subject_id,
                        principalSchema: "public",
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_authors_name",
                schema: "public",
                table: "authors",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_book_authors_author_id",
                schema: "public",
                table: "book_authors",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_book_prices_book_id",
                schema: "public",
                table: "book_prices",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "ix_book_subjects_subject_id",
                schema: "public",
                table: "book_subjects",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_books_title",
                schema: "public",
                table: "books",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subjects_description",
                schema: "public",
                table: "subjects",
                column: "description",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book_authors",
                schema: "public");

            migrationBuilder.DropTable(
                name: "book_prices",
                schema: "public");

            migrationBuilder.DropTable(
                name: "book_subjects",
                schema: "public");

            migrationBuilder.DropTable(
                name: "authors",
                schema: "public");

            migrationBuilder.DropTable(
                name: "books",
                schema: "public");

            migrationBuilder.DropTable(
                name: "subjects",
                schema: "public");
        }
    }
}
