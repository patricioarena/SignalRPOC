using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aspnet_core_api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Domicilio",
                schema: "dbo",
                columns: table => new
                {
                    DomicilioID = table.Column<Guid>(nullable: false),
                    Pais = table.Column<string>(maxLength: 20, nullable: false),
                    Provincia = table.Column<string>(maxLength: 50, nullable: false),
                    Localidad = table.Column<string>(maxLength: 50, nullable: false),
                    Partido = table.Column<string>(maxLength: 50, nullable: false),
                    CodPostal = table.Column<int>(nullable: false),
                    Calle = table.Column<string>(maxLength: 50, nullable: false),
                    Numero = table.Column<int>(maxLength: 50, nullable: false),
                    Piso = table.Column<int>(maxLength: 50, nullable: false),
                    Depto = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domicilio", x => x.DomicilioID);
                });

            migrationBuilder.CreateTable(
                name: "DatosPersonales",
                schema: "dbo",
                columns: table => new
                {
                    PersonaID = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(maxLength: 50, nullable: false),
                    FechaDeNac = table.Column<DateTime>(nullable: false),
                    TEL = table.Column<int>(nullable: false),
                    CEL = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    DomicilioID = table.Column<Guid>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosPersonales", x => x.PersonaID);
                    table.ForeignKey(
                        name: "FK_DatosPersonales_Domicilio_DomicilioID",
                        column: x => x.DomicilioID,
                        principalSchema: "dbo",
                        principalTable: "Domicilio",
                        principalColumn: "DomicilioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConocimientosAdicionales",
                schema: "dbo",
                columns: table => new
                {
                    ConAdicionalesID = table.Column<Guid>(nullable: false),
                    Titulo = table.Column<string>(maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 254, nullable: false),
                    PersonaID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConocimientosAdicionales", x => x.ConAdicionalesID);
                    table.ForeignKey(
                        name: "FK_ConocimientosAdicionales_DatosPersonales_PersonaID",
                        column: x => x.PersonaID,
                        principalSchema: "dbo",
                        principalTable: "DatosPersonales",
                        principalColumn: "PersonaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConocimientosTecnicos",
                schema: "dbo",
                columns: table => new
                {
                    ConTecnicosID = table.Column<Guid>(nullable: false),
                    Titulo = table.Column<int>(nullable: false),
                    Conocimiento = table.Column<string>(maxLength: 50, nullable: false),
                    Nivel = table.Column<int>(nullable: false),
                    PersonaID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConocimientosTecnicos", x => x.ConTecnicosID);
                    table.ForeignKey(
                        name: "FK_ConocimientosTecnicos_DatosPersonales_PersonaID",
                        column: x => x.PersonaID,
                        principalSchema: "dbo",
                        principalTable: "DatosPersonales",
                        principalColumn: "PersonaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estudio",
                schema: "dbo",
                columns: table => new
                {
                    EstudioID = table.Column<Guid>(nullable: false),
                    Establecimiento = table.Column<string>(maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(maxLength: 50, nullable: false),
                    Disciplina = table.Column<string>(maxLength: 50, nullable: false),
                    FechaDeInicio = table.Column<DateTime>(nullable: false),
                    FechaDeFin = table.Column<DateTime>(nullable: false),
                    ActExtra = table.Column<string>(maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 254, nullable: false),
                    PersonaID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudio", x => x.EstudioID);
                    table.ForeignKey(
                        name: "FK_Estudio_DatosPersonales_PersonaID",
                        column: x => x.PersonaID,
                        principalSchema: "dbo",
                        principalTable: "DatosPersonales",
                        principalColumn: "PersonaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiencia",
                schema: "dbo",
                columns: table => new
                {
                    ExperienciaID = table.Column<Guid>(nullable: false),
                    FechaDeInicio = table.Column<DateTime>(nullable: false),
                    FechaDeFin = table.Column<DateTime>(nullable: false),
                    Cargo = table.Column<string>(maxLength: 50, nullable: false),
                    TipoDeEmpleo = table.Column<string>(maxLength: 50, nullable: false),
                    Empresa = table.Column<string>(maxLength: 50, nullable: false),
                    Ubicacion = table.Column<string>(maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(maxLength: 254, nullable: true),
                    PersonaID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiencia", x => x.ExperienciaID);
                    table.ForeignKey(
                        name: "FK_Experiencia_DatosPersonales_PersonaID",
                        column: x => x.PersonaID,
                        principalSchema: "dbo",
                        principalTable: "DatosPersonales",
                        principalColumn: "PersonaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Idiomas",
                schema: "dbo",
                columns: table => new
                {
                    IdiomasID = table.Column<Guid>(nullable: false),
                    Idioma = table.Column<string>(maxLength: 20, nullable: false),
                    NivelEscrito = table.Column<int>(nullable: false),
                    NivelLectura = table.Column<int>(nullable: false),
                    NivelOral = table.Column<int>(nullable: false),
                    PersonaID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.IdiomasID);
                    table.ForeignKey(
                        name: "FK_Idiomas_DatosPersonales_PersonaID",
                        column: x => x.PersonaID,
                        principalSchema: "dbo",
                        principalTable: "DatosPersonales",
                        principalColumn: "PersonaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConocimientosAdicionales_PersonaID",
                schema: "dbo",
                table: "ConocimientosAdicionales",
                column: "PersonaID");

            migrationBuilder.CreateIndex(
                name: "IX_ConocimientosTecnicos_PersonaID",
                schema: "dbo",
                table: "ConocimientosTecnicos",
                column: "PersonaID");

            migrationBuilder.CreateIndex(
                name: "IX_DatosPersonales_DomicilioID",
                schema: "dbo",
                table: "DatosPersonales",
                column: "DomicilioID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estudio_PersonaID",
                schema: "dbo",
                table: "Estudio",
                column: "PersonaID");

            migrationBuilder.CreateIndex(
                name: "IX_Experiencia_PersonaID",
                schema: "dbo",
                table: "Experiencia",
                column: "PersonaID");

            migrationBuilder.CreateIndex(
                name: "IX_Idiomas_PersonaID",
                schema: "dbo",
                table: "Idiomas",
                column: "PersonaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConocimientosAdicionales",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ConocimientosTecnicos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Estudio",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Experiencia",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Idiomas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DatosPersonales",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Domicilio",
                schema: "dbo");
        }
    }
}
