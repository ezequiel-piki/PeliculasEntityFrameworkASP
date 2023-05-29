﻿using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Drawing;

namespace EFCorePeliculas.Entidades
{
    public class Cine
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

       // [Precision(precision:9,scale:2)]
        public NetTopologySuite.Geometries.Point Ubicacion { get; set; }
        public CineOferta CineOferta { get; set; }
        public HashSet<SalaDeCine> SalasDeCine { get; set; }    

    }
}
