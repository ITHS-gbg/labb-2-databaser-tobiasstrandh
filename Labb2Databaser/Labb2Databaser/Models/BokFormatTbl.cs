using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class BokFormatTbl
{
    public int Id { get; set; }

    public string BokFormat { get; set; } = null!;

    public virtual ICollection<BöckerTbl> BöckerTbls { get; } = new List<BöckerTbl>();
}
