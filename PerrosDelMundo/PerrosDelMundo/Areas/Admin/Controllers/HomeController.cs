using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PerrosDelMundo.Models;
using PerrosDelMundo.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using PerrosDelMundo.Models.ViewModels;


namespace PerrosDelMundo.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class HomeController : Controller

    {
        public IWebHostEnvironment Environment { get; set; }
        public HomeController(IWebHostEnvironment env)
        {
            Environment = env;
        }
        [Route("{area:exists}/{controller=Home}/{action=Index}/{id?}")]
        public IActionResult Index()
        {
            sistem14_razasContext context = new sistem14_razasContext();
            RazasRepository rp = new RazasRepository(context);
            return View(rp.GetAll().Where(x => x.Eliminado == 0));
        }
        public IActionResult AgregarRaza()
        {
            RazaViewModel vm = new RazaViewModel();
           sistem14_razasContext context = new sistem14_razasContext();
            PaisesRepository paisesRepository = new PaisesRepository(context);
            vm.Paises = paisesRepository.GetAll();
            return View(vm);
        }
        [HttpPost]
        public IActionResult AgregarRaza(RazaViewModel vm)
        {
            sistem14_razasContext context = new sistem14_razasContext();
            if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
            {
                ModelState.AddModelError("", "Debe selecionar un archivo jpg de menos de 2mb");
                PaisesRepository paisesRepository = new PaisesRepository(context);
                vm.Paises = paisesRepository.GetAll();

                return View(vm);
            }
            try
            {

                RazasRepository rp = new RazasRepository(context);
                rp.Insert(vm.Razas);
                //Guardar archivo de inserción
                FileStream fs = new FileStream(Environment.WebRootPath + "/imgs_perros/" + vm.Razas.Id + "_0.jpg", FileMode.Create);
                vm.Archivo.CopyTo(fs);
                fs.Close();

                return RedirectToAction("Index", "Home", new { area= "Admin"});

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                PaisesRepository paisesRepository = new PaisesRepository(context);
                vm.Paises = paisesRepository.GetAll();
                return View(vm);
            }

        }
        public IActionResult EditarRaza(uint id)
        {
            RazaViewModel vm = new RazaViewModel();
           sistem14_razasContext context = new sistem14_razasContext();
            RazasRepository rr = new RazasRepository(context);

            vm.Razas = rr.GetRazaById(id);
            if (vm.Razas == null)
            {
                return RedirectToAction("Index", "Home", new { area= "Admin"});
            }
            PaisesRepository paisesRepository = new PaisesRepository(context);
            vm.Paises = paisesRepository.GetAll();
            if (System.IO.File.Exists(Environment.WebRootPath + $"/imgs_perros/{vm.Razas.Id}_0.jpg"))
            {
                vm.Imagen = vm.Razas.Id + "_0.jpg";
            }
            else
            {
                vm.Imagen = "no-photo.jpg";
            }

            return View(vm);
        }
        [HttpPost]
        public IActionResult EditarRaza( RazaViewModel vm)
        {
            sistem14_razasContext context = new sistem14_razasContext();
            if (vm.Archivo != null)
            {
                if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("", "Debe selecionar un archivo jpg de menos de 2mb");
                    PaisesRepository paisesRepository = new PaisesRepository(context);
                    vm.Paises= paisesRepository.GetAll();

                    return View(vm);
                }
            }

            try
            {
                
                RazasRepository repos = new RazasRepository(context);
                var r = repos.GetRazaById(vm.Razas.Id);
                if (r != null)
                {
                    r.Nombre = vm.Razas.Nombre;
                    r.Descripcion = vm.Razas.Descripcion;
                    r.OtrosNombres = vm.Razas.OtrosNombres;
                    r.PesoMin = vm.Razas.PesoMin;
                    r.PesoMax = vm.Razas.PesoMax;
                    r.AlturaMin = vm.Razas.AlturaMin;
                    r.AlturaMax = vm.Razas.AlturaMax;
                    r.EsperanzaVida = vm.Razas.EsperanzaVida;
                    r.IdPais = vm.Razas.IdPais;
                    r.Caracteristicasfisicas.Patas = vm.Razas.Caracteristicasfisicas.Patas;
                    r.Caracteristicasfisicas.Cola = vm.Razas.Caracteristicasfisicas.Cola;
                    r.Caracteristicasfisicas.Hocico = vm.Razas.Caracteristicasfisicas.Hocico;
                    r.Caracteristicasfisicas.Pelo = vm.Razas.Caracteristicasfisicas.Pelo;
                    r.Caracteristicasfisicas.Color = vm.Razas.Caracteristicasfisicas.Color;
                    r.Estadisticasraza.NivelEnergia = vm.Razas.Estadisticasraza.NivelEnergia;
                    r.Estadisticasraza.FacilidadEntrenamiento = vm.Razas.Estadisticasraza.FacilidadEntrenamiento;
                    r.Estadisticasraza.EjercicioObligatorio = vm.Razas.Estadisticasraza.EjercicioObligatorio;
                    r.Estadisticasraza.AmistadDesconocidos = vm.Razas.Estadisticasraza.AmistadDesconocidos;
                    r.Estadisticasraza.AmistadPerros = vm.Razas.Estadisticasraza.AmistadPerros;
                    r.Estadisticasraza.NecesidadCepillado = vm.Razas.Estadisticasraza.NecesidadCepillado;
                    repos.Update(r);
                    //Guardar archivo de inserción
                    if (vm.Archivo != null)
                    {
                        FileStream fs = new FileStream(Environment.WebRootPath + "/imgs_perros/" + vm.Razas.Id + "_0.jpg", FileMode.Create);
                        vm.Archivo.CopyTo(fs);
                        fs.Close();
                    }

                }


                return RedirectToAction("Index", "Home", new { area = "Admin" });

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                PaisesRepository paisesRepository = new PaisesRepository(context);
                vm.Paises = paisesRepository.GetAll();
                return View(vm);
            }
        }


        public IActionResult EliminarRaza(uint id)
        {
            using (sistem14_razasContext context = new sistem14_razasContext())
            {
                RazasRepository repos = new RazasRepository(context);
                var razas = repos.GetById(id);
                if (razas != null)
                {
                    return View(razas);
                }
                else
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

        }
       
        [HttpPost]
        public IActionResult EliminarRaza(Razas r)
        {
            try
            {
                using (sistem14_razasContext context = new sistem14_razasContext())
                {
                    RazasRepository repos = new RazasRepository(context);
                    var raza = repos.GetRazaById(r.Id);
                    raza.Eliminado = 1;
                   
                    repos.Update(raza);

                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(r);
            }






        }






    }
}
