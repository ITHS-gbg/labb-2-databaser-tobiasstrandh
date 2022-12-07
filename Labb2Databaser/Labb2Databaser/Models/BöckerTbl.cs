using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class BöckerTbl
{
    public string Isbn { get; set; } = null!;

    public string Titel { get; set; } = null!;

    public int Språk { get; set; }

    public int? Pris { get; set; }

    public DateTime? Utgivningsdatum { get; set; }

    public int? FörlagId { get; set; }

    public int? BokFormatId { get; set; }

    public int? GenreId { get; set; }

    public virtual BokFormatTbl? BokFormat { get; set; }

    public virtual ICollection<ButiksSaldoTbl> ButiksSaldoTbls { get; } = new List<ButiksSaldoTbl>();

    public virtual FörlagTbl? Förlag { get; set; }

    public virtual GenreTbl? Genre { get; set; }

    public virtual ICollection<Ordrar> Ordrars { get; } = new List<Ordrar>();

    public virtual SpårkTbl SpråkNavigation { get; set; } = null!;

    public virtual ICollection<FörfattareTbl> Författares { get; } = new List<FörfattareTbl>();
}
