namespace GestionAula.Shared.DTOs;

public class AlumnoDesaprobadoDto
{
    public int AlumnoId { get; set; }
    public string Alumno { get; set; } = string.Empty;
    public string Grado { get; set; } = string.Empty;
    public string Seccion { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public string Profesor { get; set; } = string.Empty;
    public decimal Promedio { get; set; }
}