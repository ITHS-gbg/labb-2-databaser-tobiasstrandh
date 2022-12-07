using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class ButiksSaldoTbl
{
    public int ButiksId { get; set; }

    public string Isbn { get; set; } = null!;

    public int AntalBöcker { get; set; }

    public virtual ButikTbl Butiks { get; set; } = null!;

    public virtual BöckerTbl IsbnNavigation { get; set; } = null!;
}
