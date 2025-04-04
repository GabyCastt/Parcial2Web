using System;
using System.Collections.Generic;

namespace GestionProyectoFINAL.Models;

public partial class Proyecto
{
    public int ProyectoId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
