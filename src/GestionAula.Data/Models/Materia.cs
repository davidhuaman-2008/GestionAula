using System;
using System.Collections.Generic;

namespace GestionAula.Data.Models;

public partial class Materia
{
    public int MateriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();

    public virtual ICollection<ProfesorMaterium> ProfesorMateria { get; set; } = new List<ProfesorMaterium>();
}
