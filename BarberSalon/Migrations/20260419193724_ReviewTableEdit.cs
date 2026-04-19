using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BarberSalon.Migrations
{
    /// <inheritdoc />
    public partial class ReviewTableEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Review",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "Id", "Author", "Comment", "Rating", "ServiceId", "UserId", "WorkerId" },
                values: new object[,]
                {
                    { 1, "Ion Popescu", "Cel mai bun frizer din oraș! Întotdeauna lasă cu un zâmbet și serviciul este impecabil. Recomand cu tărie!", 5, 1, 1, 1 },
                    { 2, "Maria Ionescu", "Servicii de top și atmosferă prietenoasă. Mă simt relaxată și bine tratată de fiecare dată. Recomand cu căldură!", 5, 2, 1, 1 },
                    { 3, "Vasile Georgescu", "Întotdeauna mă simt proaspăt și încrezător după o vizită. Profesioniștii sunt atenți la detalii și foarte courtezi.", 5, 1, 1, 2 },
                    { 4, "Alexandru Mihai", "Calitate impecabilă! Am găsit în sfârșit un salon unde mă simt acasă. Echipa este foarte profesionistă și amabilă.", 5, 5, 1, 1 },
                    { 5, "Cristina Popescu", "Sunt client fidel de mai bine de un an. Mereu mă surprinde cu atenția la detalii și creativitate. Superb!", 5, 1, 1, 2 },
                    { 6, "Daniel Stancu", "Cel mai profesionist salon din Craiova! Recomand cu încredere oricui caută un serviciu de calitate.", 5, 2, 1, 3 },
                    { 7, "Andrei Popescu", "Masajul scalp este absolut relaxant. Echipa este super profesionistă și amabilă. Voi reveni cu siguranță!", 5, 3, 1, 1 },
                    { 8, "Elena Vlăduț", "Tratamentul păr este foarte eficient. Părul meu se simte mult mai moale și ușor de coafat după tratament.", 4, 6, 1, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Review");
        }
    }
}
