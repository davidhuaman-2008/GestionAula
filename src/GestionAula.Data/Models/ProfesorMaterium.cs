using System;
using System.Collections.Generic;

namespace GestionAula.Data.Models;

public partial class ProfesorMaterium
{
    public int ProfesorMateriaId { get; set; }

    public int ProfesorId { get; set; }

    public int MateriaId { get; set; }

    public virtual Materia Materia { get; set; } = null!;

    public virtual Profesore Profesor { get; set; } = null!;
}
