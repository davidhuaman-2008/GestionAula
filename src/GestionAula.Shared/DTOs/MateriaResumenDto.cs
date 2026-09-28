namespace GestionAula.Shared.DTOs;

public class MateriaResumenDto
{
    public int MateriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Profesor { get; set; } = string.Empty;
    public int TotalAlumnos { get; set; }
    public int Aprobados { get; set; }
    public int Desaprobados { get; set; }
    public decimal PromedioMateria { get; set; }
    public decimal PorcentajeAprobados { get; set; }
}