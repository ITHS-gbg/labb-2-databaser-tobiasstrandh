using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class FörlagTbl
{
    public int Id { get; set; }

    public string FörlagNamn { get; set; } = null!;

    public virtual ICollection<BöckerTbl> BöckerTbls { get; } = new List<BöckerTbl>();
}
