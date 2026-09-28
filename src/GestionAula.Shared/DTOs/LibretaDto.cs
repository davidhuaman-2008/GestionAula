namespace GestionAula.Shared.DTOs;

public class LibretaDto
{
    public int AlumnoId { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string Grado { get; set; } = string.Empty;
    public string Seccion { get; set; } = string.Empty;
    public int AnioEscolar { get; set; }
    public decimal PromedioGeneral { get; set; }
    public List<NotaDto> Notas { get; set; } = new();
}