using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class FörfattareTbl
{
    public int Id { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public DateTime? Födelsedatum { get; set; }

    public virtual ICollection<BöckerTbl> Isbns { get; } = new List<BöckerTbl>();
}
