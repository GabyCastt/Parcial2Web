using System;
using System.Collections.Generic;

namespace GestionProyectoFINAL.Models;

public partial class Tarea
{
    public int TareaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; } = null!;

    public int? ProyectoId { get; set; }

    public virtual Proyecto? Proyecto { get; set; }
}
