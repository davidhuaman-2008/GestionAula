namespace GestionAula.Shared.DTOs;

public class AlumnoDto
{
    public int AlumnoId { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Grado { get; set; } = string.Empty;
    public string Seccion { get; set; } = string.Empty;
    public int AnioEscolar { get; set; }
    public decimal PromedioGeneral { get; set; }
    public int MateriasAprobadas { get; set; }
    public int MateriasDesaprobadas { get; set; }
    public string EstadoGeneral { get; set; } = string.Empty;
}