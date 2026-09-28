using System;
using System.Collections.Generic;

namespace GestionAula.Data.Models;

public partial class Alumno
{
    public int AlumnoId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? DNI { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();
}
