using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class VTitlarPerFörfattare
{
    public string Namn { get; set; } = null!;

    public string Ålder { get; set; } = null!;

    public string AntalTitlar { get; set; } = null!;

    public string TotalVärdeLager { get; set; } = null!;
}
