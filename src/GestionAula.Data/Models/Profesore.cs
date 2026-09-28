using System;
using System.Collections.Generic;

namespace GestionAula.Data.Models;

public partial class Profesore
{
    public int ProfesorId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<ProfesorMaterium> ProfesorMateria { get; set; } = new List<ProfesorMaterium>();
}
