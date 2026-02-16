using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_Bakalarka.Migrations
{
    /// <inheritdoc />
    public partial class PLS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pouzivatelia",
                columns: table => new
                {
                    PouzivatelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Meno = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priezvisko = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatumNarodenia = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Heslo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rola = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pouzivatelia", x => x.PouzivatelId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PredmetViewModel",
                columns: table => new
                {
                    PredmetViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nazov = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Popis = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ucitel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PocetZiakov = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredmetViewModel", x => x.PredmetViewId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Predmety",
                columns: table => new
                {
                    PredmetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nazov = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Popis = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UcitelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmety", x => x.PredmetId);
                    table.ForeignKey(
                        name: "FK_Predmety_Pouzivatelia_UcitelId",
                        column: x => x.UcitelId,
                        principalTable: "Pouzivatelia",
                        principalColumn: "PouzivatelId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Dochadzky",
                columns: table => new
                {
                    DochazkaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    PouzivatelId = table.Column<int>(type: "int", nullable: false),
                    JePritomny = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dochadzky", x => x.DochazkaId);
                    table.ForeignKey(
                        name: "FK_Dochadzky_Pouzivatelia_PouzivatelId",
                        column: x => x.PouzivatelId,
                        principalTable: "Pouzivatelia",
                        principalColumn: "PouzivatelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dochadzky_Predmety_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmety",
                        principalColumn: "PredmetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriradeniaPredmetovUcitelom",
                columns: table => new
                {
                    PriradeniePredUcitelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PouzivatelId = table.Column<int>(type: "int", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriradeniaPredmetovUcitelom", x => x.PriradeniePredUcitelId);
                    table.ForeignKey(
                        name: "FK_PriradeniaPredmetovUcitelom_Pouzivatelia_PouzivatelId",
                        column: x => x.PouzivatelId,
                        principalTable: "Pouzivatelia",
                        principalColumn: "PouzivatelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriradeniaPredmetovUcitelom_Predmety_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmety",
                        principalColumn: "PredmetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriradeniePredmetovZiakom",
                columns: table => new
                {
                    PriradeniePredZiakId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PouzivatelId = table.Column<int>(type: "int", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriradeniePredmetovZiakom", x => x.PriradeniePredZiakId);
                    table.ForeignKey(
                        name: "FK_PriradeniePredmetovZiakom_Pouzivatelia_PouzivatelId",
                        column: x => x.PouzivatelId,
                        principalTable: "Pouzivatelia",
                        principalColumn: "PouzivatelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriradeniePredmetovZiakom_Predmety_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmety",
                        principalColumn: "PredmetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Znamky",
                columns: table => new
                {
                    ZnamkaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    PouzivatelId = table.Column<int>(type: "int", nullable: false),
                    Hodnota = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Poznamka = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Znamky", x => x.ZnamkaId);
                    table.ForeignKey(
                        name: "FK_Znamky_Pouzivatelia_PouzivatelId",
                        column: x => x.PouzivatelId,
                        principalTable: "Pouzivatelia",
                        principalColumn: "PouzivatelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Znamky_Predmety_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmety",
                        principalColumn: "PredmetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Dochadzky_PouzivatelId",
                table: "Dochadzky",
                column: "PouzivatelId");

            migrationBuilder.CreateIndex(
                name: "IX_Dochadzky_PredmetId",
                table: "Dochadzky",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Predmety_UcitelId",
                table: "Predmety",
                column: "UcitelId");

            migrationBuilder.CreateIndex(
                name: "IX_PriradeniaPredmetovUcitelom_PouzivatelId",
                table: "PriradeniaPredmetovUcitelom",
                column: "PouzivatelId");

            migrationBuilder.CreateIndex(
                name: "IX_PriradeniaPredmetovUcitelom_PredmetId",
                table: "PriradeniaPredmetovUcitelom",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_PriradeniePredmetovZiakom_PouzivatelId",
                table: "PriradeniePredmetovZiakom",
                column: "PouzivatelId");

            migrationBuilder.CreateIndex(
                name: "IX_PriradeniePredmetovZiakom_PredmetId",
                table: "PriradeniePredmetovZiakom",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Znamky_PouzivatelId",
                table: "Znamky",
                column: "PouzivatelId");

            migrationBuilder.CreateIndex(
                name: "IX_Znamky_PredmetId",
                table: "Znamky",
                column: "PredmetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dochadzky");

            migrationBuilder.DropTable(
                name: "PredmetViewModel");

            migrationBuilder.DropTable(
                name: "PriradeniaPredmetovUcitelom");

            migrationBuilder.DropTable(
                name: "PriradeniePredmetovZiakom");

            migrationBuilder.DropTable(
                name: "Znamky");

            migrationBuilder.DropTable(
                name: "Predmety");

            migrationBuilder.DropTable(
                name: "Pouzivatelia");
        }
    }
}

