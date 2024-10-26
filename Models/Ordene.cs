using System;
using System.Collections.Generic;

namespace Jiji.Models;

public partial class Ordene
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Idusuario { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
