using System;
using System.Collections.Generic;

namespace Labb2Databaser.Models;

public partial class Ordrar
{
    public int OrderNr { get; set; }

    public string Isbn { get; set; } = null!;

    public int AntalBöcker { get; set; }

    public int StyckPris { get; set; }

    public virtual BöckerTbl IsbnNavigation { get; set; } = null!;

    public virtual OrdrarDetaljer OrderNrNavigation { get; set; } = null!;
}
