using GestionAula.Data.Data;
using GestionAula.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionAula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MateriasController : ControllerBase
{
    private readonly GestionAulaContext _context;

    public MateriasController(GestionAulaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Resumen de cada materia: total alumnos, aprobados, desaprobados y promedio.
    /// </summary>
    [HttpGet("resumen")]
    public async Task<ActionResult<IEnumerable<MateriaResumenDto>>> GetResumen()
    {
        var materias = await _context.Materias.AsNoTracking().ToListAsync();
        var notas = await _context.Notas.AsNoTracking().ToListAsync();

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

        var resultado = materias.Select(m =>
        {
            var notasMateria = notas.Where(n => n.MateriaId == m.MateriaId).ToList();
            var promedios = notasMateria
                .Select(n => CalcularPromedio(n.Parcial1, n.Parcial2, n.Parcial3, n.Parcial4))
                .ToList();

            var total = promedios.Count;
            var aprobados = promedios.Count(p => p >= 11);
            var desaprobados = total - aprobados;

            return new MateriaResumenDto
            {
                MateriaId = m.MateriaId,
                Nombre = m.Nombre,
                Profesor = profesoresPorMateria.GetValueOrDefault(m.MateriaId, "—"),
                TotalAlumnos = total,
                Aprobados = aprobados,
                Desaprobados = desaprobados,
                PromedioMateria = total > 0 ? Math.Round(promedios.Average(), 2) : 0,
                PorcentajeAprobados = total > 0 ? Math.Round((decimal)aprobados * 100 / total, 1) : 0
            };
        })
        .OrderByDescending(m => m.PorcentajeAprobados)
        .ToList();

        return Ok(resultado);
    }

    /// <summary>
    /// Alumnos aprobados en una materia específica.
    /// </summary>
    [HttpGet("{id}/aprobados")]
    public async Task<ActionResult<IEnumerable<NotaDto>>> GetAprobados(int id)
    {
        return await ObtenerAlumnosPorEstado(id, aprobado: true);
    }

    /// <summary>
    /// Alumnos desaprobados en una materia específica.
    /// </summary>
    [HttpGet("{id}/desaprobados")]
    public async Task<ActionResult<IEnumerable<NotaDto>>> GetDesaprobados(int id)
    {
        return await ObtenerAlumnosPorEstado(id, aprobado: false);
    }

    private async Task<ActionResult<IEnumerable<NotaDto>>> ObtenerAlumnosPorEstado(int materiaId, bool aprobado)
    {
        var materiaExiste = await _context.Materias.AnyAsync(m => m.MateriaId == materiaId);
        if (!materiaExiste) return NotFound($"Materia {materiaId} no encontrada");

        var notas = await _context.Notas
            .Include(n => n.Alumno)
            .Include(n => n.Materia)
            .Where(n => n.MateriaId == materiaId)
            .AsNoTracking()
            .ToListAsync();

        // ✅ Profesores (puede haber más de uno por materia)
        var profesores = await _context.ProfesorMateria
            .Include(pm => pm.Profesor)
            .Where(pm => pm.MateriaId == materiaId)
            .Select(pm => pm.Profesor.Nombres + " " + pm.Profesor.Apellidos)
            .ToListAsync();

        var profesor = profesores.Any() ? string.Join(", ", profesores) : "—";

        var resultado = notas
            .Select(n =>
            {
                var promedio = CalcularPromedio(n.Parcial1, n.Parcial2, n.Parcial3, n.Parcial4);
                return new { Nota = n, Promedio = promedio };
            })
            .Where(x => aprobado ? x.Promedio >= 11 : x.Promedio < 11)
            .Select(x => new NotaDto
            {
                NotaId = x.Nota.NotaId,
                AlumnoId = x.Nota.AlumnoId,
                Alumno = $"{x.Nota.Alumno.Nombres} {x.Nota.Alumno.Apellidos}",
                MateriaId = x.Nota.MateriaId,
                Materia = x.Nota.Materia.Nombre,
                Profesor = profesor,
                Parcial1 = x.Nota.Parcial1,
                Parcial2 = x.Nota.Parcial2,
                Parcial3 = x.Nota.Parcial3,
                Parcial4 = x.Nota.Parcial4,
                Promedio = x.Promedio,
                Estado = aprobado ? "APROBADO" : "DESAPROBADO"
            })
            .OrderByDescending(n => n.Promedio)
            .ToList();

        return Ok(resultado);
    }

    private static decimal CalcularPromedio(decimal? p1, decimal? p2, decimal? p3, decimal? p4)
    {
        var valores = new[] { p1, p2, p3, p4 }
            .Where(v => v.HasValue)
            .Select(v => v!.Value)
            .ToList();

        return valores.Any() ? Math.Round(valores.Average(), 2) : 0;
    }
}