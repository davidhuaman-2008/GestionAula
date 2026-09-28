using System;
using System.Collections.Generic;

namespace GestionAula.Data.Models;

public partial class Nota
{
    public int NotaId { get; set; }

    public int AlumnoId { get; set; }

    public int MateriaId { get; set; }

    public decimal? Parcial1 { get; set; }

    public decimal? Parcial2 { get; set; }

    public decimal? Parcial3 { get; set; }

    public decimal? Parcial4 { get; set; }

    public virtual Alumno Alumno { get; set; } = null!;

    public virtual Materia Materia { get; set; } = null!;
}
