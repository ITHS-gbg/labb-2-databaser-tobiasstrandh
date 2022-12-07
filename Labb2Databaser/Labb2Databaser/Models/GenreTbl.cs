using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class GenreTbl
{
    public int Id { get; set; }

    public string GenreNamn { get; set; } = null!;

    public virtual ICollection<BöckerTbl> BöckerTbls { get; } = new List<BöckerTbl>();
}
