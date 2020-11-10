using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerrosDelMundo.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using System.IO;
using System.Security.Cryptography.X509Certificates;




namespace PerrosDelMundo.Repositories
{
    public class RazasRepository : Repository<Razas>
    {
		public RazasRepository(sistem14_razasContext context):base(context)
		{ }

		
		public IEnumerable<RazaViewModel> GetRazas()
		{
			return Context.Razas.OrderBy(x => x.Nombre)
				.Select(x => new RazaViewModel
				{
					Id = x.Id,
					Nombre = x.Nombre
				});
		}

		public IEnumerable<RazaViewModel> GetRazasByLetraInicial(string letra)
		{
			return GetRazas().Where(x => x.Nombre.StartsWith(letra));
		}


		public IEnumerable<char> GetLetrasIniciales()
		{
			return Context.Razas.OrderBy(x => x.Nombre)
				.Select(x => x.Nombre.First());
		}

		public Razas GetRazaByNombre(string nombre)
		{
			nombre = nombre.Replace("-", " ");
			return Context.Razas
				.Include(x => x.Estadisticasraza)
				.Include(x => x.Caracteristicasfisicas)
				.Include(x => x.IdPaisNavigation)
				.FirstOrDefault(x => x.Nombre == nombre);
		}

		public IEnumerable<RazaViewModel> Get4RandomRazasExcept(string nombre)
		{
			nombre = nombre.Replace("-", " ");
			Random r = new Random();
			return GetRazas().Where(x => x.Nombre != nombre)
				.OrderBy(x => r.Next()).Take(4).Select(x => new RazaViewModel { Id = x.Id, Nombre = x.Nombre });
		}
		public Razas GetRazaById(uint id)
		{
			return Context.Razas.Include(x => x.Estadisticasraza)
				.Include(x => x.Caracteristicasfisicas)
				.Include(x => x.IdPaisNavigation)
				.FirstOrDefault(x => x.Id == id);
		}
        public override bool Validate(Razas raza)
        {
            if (raza.Id <= 0)
                throw new Exception("Ingrese una ID válida para la raza.");
            if (string.IsNullOrWhiteSpace(raza.Nombre))
                throw new Exception("Ingrese un nombre para la raza.");
            if (string.IsNullOrWhiteSpace(raza.Descripcion))
                throw new Exception("Ingrese una descripción para la raza.");
            if (string.IsNullOrWhiteSpace(raza.OtrosNombres))
                throw new Exception("Ingrese otro nombre para la raza.");
            if (!Context.Paises.Any(x => x.Id == raza.IdPais))
                throw new Exception("No existe el país específicado.");
            if (raza.PesoMin <= 0)
                throw new Exception("Ingrese un peso mínimo válido para la raza.");
            if (raza.PesoMax <= 0)
                throw new Exception("Ingrese un peso máximo válido para la raza.");
            if (raza.AlturaMin <= 0)
                throw new Exception("Ingrese una altura mínima válida para la raza.");
            if (raza.AlturaMax <= 0)
                throw new Exception("Ingrese una altura máxima válida para la raza.");
            if (raza.EsperanzaVida <= 0)
                throw new Exception("Ingrese una esperanza de vida válida para la raza.");
            if (Context.Razas.Any(x => x.Nombre == raza.Nombre && x.Id != raza.Id))
                throw new Exception("Ya existe una raza con ese mismo nombre.");
            if (string.IsNullOrWhiteSpace(raza.Caracteristicasfisicas.Patas))
                throw new Exception("Ingrese una característica física sobre las patas.");
            if (string.IsNullOrWhiteSpace(raza.Caracteristicasfisicas.Cola))
                throw new Exception("Ingrese una característica física sobre la cola.");
            if (string.IsNullOrWhiteSpace(raza.Caracteristicasfisicas.Hocico))
                throw new Exception("Ingrese una característica física sobre el hocico.");
            if (string.IsNullOrWhiteSpace(raza.Caracteristicasfisicas.Pelo))
                throw new Exception("Ingrese una característica física sobre el pelo.");
            if (string.IsNullOrWhiteSpace(raza.Caracteristicasfisicas.Color))
                throw new Exception("Ingrese una característica física sobre el color.");
            if (raza.Estadisticasraza.NivelEnergia < 0 || raza.Estadisticasraza.NivelEnergia > 10)
                throw new Exception("La estadistica debe de ser de 0 a 10.");
            if (raza.Estadisticasraza.FacilidadEntrenamiento < 0 || raza.Estadisticasraza.FacilidadEntrenamiento > 10)
                throw new Exception("La estadistica debe de ser de 0 a 10.");
            if (raza.Estadisticasraza.EjercicioObligatorio < 0 || raza.Estadisticasraza.EjercicioObligatorio > 10)
                throw new Exception("La estadistica debe de ser de 0 a 10.");
            if (raza.Estadisticasraza.AmistadDesconocidos < 0 || raza.Estadisticasraza.AmistadDesconocidos > 10)
                throw new Exception("La estadistica debe de ser de 0 a 10.");
            if (raza.Estadisticasraza.AmistadPerros < 0 || raza.Estadisticasraza.AmistadPerros > 10)
                throw new Exception("La estadistica debe de ser de 0 a 10.");
            if (raza.Estadisticasraza.NecesidadCepillado < 0 || raza.Estadisticasraza.NecesidadCepillado > 10)
                throw new Exception("La estadistica debe de ser de 0 a 10.");
            return true;
        }
    }
}
