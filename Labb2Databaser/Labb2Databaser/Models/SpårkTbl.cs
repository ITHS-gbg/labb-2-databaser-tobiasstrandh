using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class SpårkTbl
{
    public int Id { get; set; }

    public string Spårk { get; set; } = null!;

    public virtual ICollection<BöckerTbl> BöckerTbls { get; } = new List<BöckerTbl>();
}
