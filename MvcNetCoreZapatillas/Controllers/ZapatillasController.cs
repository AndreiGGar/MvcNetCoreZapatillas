using Microsoft.AspNetCore.Mvc;
using MvcNetCoreZapatillas.Models;
using MvcNetCoreZapatillas.Repositories;

namespace MvcNetCoreZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;


        public ZapatillasController (RepositoryZapatillas repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Zapatilla> zapatillas = this.repo.GetZapatillas();
            return View(zapatillas);
        }

        public IActionResult Details(int idproducto)
        {
            Zapatilla zapatilla = this.repo.FindZapatilla(idproducto);
            return View(zapatilla);
        }

        public async Task<IActionResult> ImagenesZapatilla(int idproducto, int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosZapatillas(idproducto);
            int siguiente = posicion.Value + 1;
            if (siguiente > numregistros)
            {
                siguiente = numregistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            List<ImagenZapatilla> imagenes = await this.repo.GetImagenesAsync(idproducto, posicion.Value);
            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["REGISTROS"] = numregistros;
            ViewData["IDPRODUCTO"] = idproducto;
            ViewData["POSICION"] = posicion.Value;
            return View(imagenes);
        }

        public IActionResult _DetailsPartial(int idproducto)
        {
            Zapatilla zapatilla = this.repo.FindZapatilla(idproducto);
            return View(zapatilla);
        }

        public async Task<IActionResult> _Pagination(int idproducto)
        {
            int posicion = 1;
            int numregistros = this.repo.GetNumeroRegistrosZapatillas(idproducto);
            int siguiente = posicion + 1;
            if (siguiente > numregistros)
            {
                siguiente = numregistros;
            }
            int anterior = posicion - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            List<ImagenZapatilla> imagenes = await this.repo.GetImagenesAsync(idproducto, posicion);
            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["REGISTROS"] = numregistros;
            ViewData["IDPRODUCTO"] = idproducto;
            ViewData["POSICION"] = posicion;
            return View(imagenes);
        }
    }
}
