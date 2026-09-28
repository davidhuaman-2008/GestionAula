namespace GestionAula.Shared.DTOs;

public class NotaDto
{
    public int NotaId { get; set; }
    public int AlumnoId { get; set; }
    public string Alumno { get; set; } = string.Empty;
    public int MateriaId { get; set; }
    public string Materia { get; set; } = string.Empty;
    public string Profesor { get; set; } = string.Empty;
    public decimal? Parcial1 { get; set; }
    public decimal? Parcial2 { get; set; }
    public decimal? Parcial3 { get; set; }
    public decimal? Parcial4 { get; set; }
    public decimal Promedio { get; set; }
    public string Estado { get; set; } = string.Empty;
}