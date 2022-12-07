using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class VHurMångaSåldaIvarjeButikNov2022
{
    public string Butiksnamn { get; set; } = null!;

    public int? SåldaPerButik { get; set; }

    public int? Total { get; set; }
}
