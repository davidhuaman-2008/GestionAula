using System;
using System.Collections.Generic;

namespace GestionAula.Data.Models;

public partial class Grado
{
    public int GradoId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
