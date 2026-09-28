using System;
using System.Collections.Generic;

namespace GestionAula.Data.Models;

public partial class Matricula
{
    public int MatriculaId { get; set; }

    public int AlumnoId { get; set; }

    public int GradoId { get; set; }

    public int SeccionId { get; set; }

    public int AnioEscolar { get; set; }

    public virtual Alumno Alumno { get; set; } = null!;

    public virtual Grado Grado { get; set; } = null!;

    public virtual Seccione Seccion { get; set; } = null!;
}
