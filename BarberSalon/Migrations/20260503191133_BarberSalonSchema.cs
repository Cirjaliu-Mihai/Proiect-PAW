using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BarberSalon.Migrations
{
    /// <inheritdoc />
    public partial class BarberSalonSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberSince = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointment_Worker_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Worker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortofolioItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortofolioItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortofolioItem_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortofolioItem_Worker_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Worker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Worker_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Worker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Brand", "Description", "Name", "PhotoPath", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "American Crew", "Pomadă cu textură mată și fixare medie, ideală pentru stiluri moderne", "American Crew Fiber", "/assets/images/products/default-product.png", 65.00m, 15 },
                    { 2, "Layrite", "Pomadă cu fixare puternică și efect lucios pentru o coafură impecabilă", "Layrite Pomade", "/assets/images/products/default-product.png", 55.00m, 12 },
                    { 3, "Suavecito", "Pomadă clasică cu parfum discret, ușor de spălat", "Suavecito Original Hold", "/assets/images/products/default-product.png", 50.00m, 20 },
                    { 4, "Proraso", "Cremă de ras cu eucalipt și mentol pentru un ras confortabil", "Proraso Shaving Cream", "/assets/images/products/default-product.png", 45.00m, 18 },
                    { 5, "Proraso", "Balsam după ras, calmant și hidratant pentru pielea sensibilă", "Proraso After Shave Balm", "/assets/images/products/default-product.png", 40.00m, 10 },
                    { 6, "Wahl", "Ulei de întreținere pentru mașini de tuns, prelungește durata de viață a lamelor", "Wahl Clipper Oil", "/assets/images/products/default-product.png", 25.00m, 30 },
                    { 7, "King Research", "Dezinfectant profesional pentru unelte, standard de igienă în saloane", "Barbicide Disinfectant", "/assets/images/products/default-product.png", 35.00m, 8 }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Tuns modern, adaptat preferințelor tale", "Tuns Barbat", 50 },
                    { 2, "Tuns complet cu ras și îngrijire a bărbii", "Tuns Barbat + Barba", 70 },
                    { 3, "Spălare și massage pentru relaxare", "Spalat", 30 },
                    { 4, "Vopsire păr cu produse profesionale", "Vopsit", 60 },
                    { 5, "Ras și îngrijire profesională a bărbii", "Barba", 40 },
                    { 6, "Tratament hidratant și nutritiv pentru păr", "Tratament Păr", 45 },
                    { 7, "Aplicare produse de finisare și styling", "Pomadă", 15 },
                    { 8, "Tunsoare specială pentru copii cu îngrijire", "Tuns Copil", 35 },
                    { 9, "Masaj relaxant cu produse profesionale", "Masaj Scalp", 50 }
                });

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[] { "Id", "Email", "MemberSince", "Name", "Phone", "PhotoPath", "UserId" },
                values: new object[] { 1, "andrei@gmail.ro", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andrei Popescu", "0712 345 678", "/assets/images/poza-profil.png", 1 });

            migrationBuilder.InsertData(
                table: "Worker",
                columns: new[] { "Id", "Address", "HireDate", "Name", "PhoneNumber", "PhotoPath", "Position" },
                values: new object[,]
                {
                    { 1, "Strada Primaverii, Craiova", new DateTime(2014, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andrei Popescu", 740123456, "/assets/images/team/frizer1.png", "Frizer Senior" },
                    { 2, "Strada Primaverii, Craiova", new DateTime(2016, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mihai Ionescu", 740123457, "/assets/images/team/frizer2.png", "Frizer Specialist" },
                    { 3, "Strada Primaverii, Craiova", new DateTime(2018, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cristian Georgescu", 740123458, "/assets/images/team/frizer3.png", "Frizer" },
                    { 4, "Strada Primaverii, Craiova", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daniel Vasilescu", 740123459, "/assets/images/team/frizer4.png", "Frizer Apprentice" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ServiceId",
                table: "Appointment",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_WorkerId",
                table: "Appointment",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_PortofolioItem_ServiceId",
                table: "PortofolioItem",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PortofolioItem_WorkerId",
                table: "PortofolioItem",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ServiceId",
                table: "Review",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_WorkerId",
                table: "Review",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "PortofolioItem");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Worker");
        }
    }
}
