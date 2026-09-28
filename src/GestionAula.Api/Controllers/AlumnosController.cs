using GestionAula.Data.Data;
using GestionAula.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionAula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlumnosController : ControllerBase
{
    private readonly GestionAulaContext _context;

    public AlumnosController(GestionAulaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todos los alumnos con su promedio general y estado.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlumnoDto>>> GetAlumnos()
    {
        var alumnos = await _context.Alumnos
            .Include(a => a.Matriculas).ThenInclude(m => m.Grado)
            .Include(a => a.Matriculas).ThenInclude(m => m.Seccion)
            .Include(a => a.Nota).ThenInclude(n => n.Materia)
            .AsNoTracking()
            .ToListAsync();

        var resultado = alumnos.Select(a =>
        {
            var matricula = a.Matriculas.FirstOrDefault();
            var promedios = a.Nota
                .Select(n => CalcularPromedio(n.Parcial1, n.Parcial2, n.Parcial3, n.Parcial4))
                .ToList();

            var promedioGeneral = promedios.Any() ? Math.Round(promedios.Average(), 2) : 0;
            var aprobadas = promedios.Count(p => p >= 11);
            var desaprobadas = promedios.Count(p => p < 11);

            string estado = desaprobadas == 0 ? "APROBADO"
                          : desaprobadas <= 2 ? "EN RIESGO"
                          : "DESAPROBADO";

            return new AlumnoDto
            {
                AlumnoId = a.AlumnoId,
                Nombres = a.Nombres,
                Apellidos = a.Apellidos,
                DNI = a.DNI ?? "",
                Email = a.Email,
                Grado = matricula?.Grado?.Nombre ?? "Sin asignar",
                Seccion = matricula?.Seccion?.Nombre ?? "-",
                AnioEscolar = matricula?.AnioEscolar ?? 0,
                PromedioGeneral = promedioGeneral,
                MateriasAprobadas = aprobadas,
                MateriasDesaprobadas = desaprobadas,
                EstadoGeneral = estado
            };
        }).ToList();

        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene la libreta completa de un alumno.
    /// </summary>
    [HttpGet("{id}/libreta")]
    public async Task<ActionResult<LibretaDto>> GetLibreta(int id)
    {
        var alumno = await _context.Alumnos
            .Include(a => a.Matriculas).ThenInclude(m => m.Grado)
            .Include(a => a.Matriculas).ThenInclude(m => m.Seccion)
            .Include(a => a.Nota).ThenInclude(n => n.Materia)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AlumnoId == id);

        if (alumno == null) return NotFound($"Alumno {id} no encontrado");

        var matricula = alumno.Matriculas.FirstOrDefault();

        // ✅ Profesores por materia (agrupado porque una materia puede tener varios profes)
        var profesoresPorMateria = await _context.ProfesorMateria
            .Include(pm => pm.Profesor)
            .GroupBy(pm => pm.MateriaId)
            .Select(g => new
            {
                MateriaId = g.Key,
                Profesores = string.Join(", ",
                    g.Select(pm => pm.Profesor.Nombres + " " + pm.Profesor.Apellidos))
            })
            .ToDictionaryAsync(x => x.MateriaId, x => x.Profesores);

        var notasDto = alumno.Nota.Select(n =>
        {
            var promedio = CalcularPromedio(n.Parcial1, n.Parcial2, n.Parcial3, n.Parcial4);
            return new NotaDto
            {
                NotaId = n.NotaId,
                AlumnoId = n.AlumnoId,
                Alumno = $"{alumno.Nombres} {alumno.Apellidos}",
                MateriaId = n.MateriaId,
                Materia = n.Materia.Nombre,
                Profesor = profesoresPorMateria.GetValueOrDefault(n.MateriaId, "—"),
                Parcial1 = n.Parcial1,
                Parcial2 = n.Parcial2,
                Parcial3 = n.Parcial3,
                Parcial4 = n.Parcial4,
                Promedio = promedio,
                Estado = promedio >= 11 ? "APROBADO" : "DESAPROBADO"
            };
        }).ToList();

        var promedioGeneral = notasDto.Any()
            ? Math.Round(notasDto.Average(n => n.Promedio), 2)
            : 0;

        var libreta = new LibretaDto
        {
            AlumnoId = alumno.AlumnoId,
            Nombres = alumno.Nombres,
            Apellidos = alumno.Apellidos,
            DNI = alumno.DNI ?? "",
            Grado = matricula?.Grado?.Nombre ?? "-",
            Seccion = matricula?.Seccion?.Nombre ?? "-",
            AnioEscolar = matricula?.AnioEscolar ?? 0,
            PromedioGeneral = promedioGeneral,
            Notas = notasDto
        };

        return Ok(libreta);
    }

    /// <summary>
    /// Lista de alumnos con materias desaprobadas.
    /// </summary>
    [HttpGet("desaprobados")]
    public async Task<ActionResult<IEnumerable<AlumnoDesaprobadoDto>>> GetDesaprobados()
    {
        // ✅ Profesores por materia (agrupado)
        var profesoresPorMateria = await _context.ProfesorMateria
            .Include(pm => pm.Profesor)
            .GroupBy(pm => pm.MateriaId)
            .Select(g => new
            {
                MateriaId = g.Key,
                Profesores = string.Join(", ",
                    g.Select(pm => pm.Profesor.Nombres + " " + pm.Profesor.Apellidos))
            })
            .ToDictionaryAsync(x => x.MateriaId, x => x.Profesores);

        var notas = await _context.Notas
            .Include(n => n.Alumno).ThenInclude(a => a.Matriculas).ThenInclude(m => m.Grado)
            .Include(n => n.Alumno).ThenInclude(a => a.Matriculas).ThenInclude(m => m.Seccion)
            .Include(n => n.Materia)
            .AsNoTracking()
            .ToListAsync();

        var resultado = notas
            .Select(n => new
            {
                Nota = n,
                Promedio = CalcularPromedio(n.Parcial1, n.Parcial2, n.Parcial3, n.Parcial4)
            })
            .Where(x => x.Promedio < 11)
            .Select(x =>
            {
                var matricula = x.Nota.Alumno.Matriculas.FirstOrDefault();
                return new AlumnoDesaprobadoDto
                {
                    AlumnoId = x.Nota.AlumnoId,
                    Alumno = $"{x.Nota.Alumno.Nombres} {x.Nota.Alumno.Apellidos}",
                    Grado = matricula?.Grado?.Nombre ?? "-",
                    Seccion = matricula?.Seccion?.Nombre ?? "-",
                    Materia = x.Nota.Materia.Nombre,
                    Profesor = profesoresPorMateria.GetValueOrDefault(x.Nota.MateriaId, "—"),
                    Promedio = x.Promedio
                };
            })
            .OrderBy(x => x.Alumno)
            .ThenBy(x => x.Materia)
            .ToList();

        return Ok(resultado);
    }

    /// <summary>
    /// Calcula el promedio de los 4 parciales (ignora nulos).
    /// </summary>
    private static decimal CalcularPromedio(decimal? p1, decimal? p2, decimal? p3, decimal? p4)
    {
        var valores = new[] { p1, p2, p3, p4 }
            .Where(v => v.HasValue)
            .Select(v => v!.Value)
            .ToList();

        return valores.Any() ? Math.Round(valores.Average(), 2) : 0;
    }
}