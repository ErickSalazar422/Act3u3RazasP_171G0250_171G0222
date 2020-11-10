using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace PerrosDelMundo.Models.ViewModels
{
    public class EstadisticasViewModel
    {
        public uint id { get; set; }
        public Estadisticasraza Estadisticasraza { get; set; }
    }
}
