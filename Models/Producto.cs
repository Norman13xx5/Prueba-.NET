using System;
using System.Collections.Generic;

namespace Jiji.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Idorden { get; set; }

    public int Idcategoria { get; set; }

    public int Idempresa { get; set; }

    public decimal Valor { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Categoria IdcategoriaNavigation { get; set; } = null!;

    public virtual Empresa IdempresaNavigation { get; set; } = null!;

    public virtual Ordene IdordenNavigation { get; set; } = null!;
}
