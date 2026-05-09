using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerPulseRestAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKnowledgeArticleReadCfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_knowledge_article_reads_article_id_user_id",
                table: "knowledge_article_reads");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_article_id",
                table: "knowledge_article_reads",
                column: "article_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_knowledge_article_reads_article_id",
                table: "knowledge_article_reads");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_article_id_user_id",
                table: "knowledge_article_reads",
                columns: new[] { "article_id", "user_id" },
                unique: true);
        }
    }
}
