using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class VAntalSåldaPerGenre
{
    public string GenreNamn { get; set; } = null!;

    public string AntalSålda { get; set; } = null!;

    public string TotalVärde { get; set; } = null!;
}
