using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace PerrosDelMundo.Models
{
    public class RazaViewModel
    {
        public uint Id { get; set; }
        public string Nombre { get; set; }
        public Razas Razas { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
        public IFormFile Archivo { get; set; }
        public string Imagen { get; set; }
        public Estadisticasraza Estadisticasraza { get; set; }
    }
}
