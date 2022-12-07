using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class ButikTbl
{
    public int Id { get; set; }

    public string Butiksnamn { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string Postnummer { get; set; } = null!;

    public string Stad { get; set; } = null!;

    public virtual ICollection<ButiksSaldoTbl> ButiksSaldoTbls { get; } = new List<ButiksSaldoTbl>();

    public virtual ICollection<OrdrarDetaljer> OrdrarDetaljers { get; } = new List<OrdrarDetaljer>();
}
