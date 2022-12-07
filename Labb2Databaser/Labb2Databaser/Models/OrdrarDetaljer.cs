using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class OrdrarDetaljer
{
    public int OrderNr { get; set; }

    public DateTime? OrderDatum { get; set; }

    public int? ButiksId { get; set; }

    public virtual ButikTbl? Butiks { get; set; }

    public virtual ICollection<Ordrar> Ordrars { get; } = new List<Ordrar>();
}
