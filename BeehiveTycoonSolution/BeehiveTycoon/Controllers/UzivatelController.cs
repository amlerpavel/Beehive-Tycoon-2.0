using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models;
using System.Diagnostics;
using BeehiveTycoon.Db;

namespace BeehiveTycoon.Controllers
{
    public class UzivatelController : Controller
    {
        private readonly DbUzivatele _dbUzivatele;

        public UzivatelController(Data.BeehiveTycoonContex contex)
        {
            _dbUzivatele = new(contex);
        }

        [HttpGet]
        public IActionResult Registrace()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Prihlaseni()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Zaregistrovat([FromBody] DataRegistrace registrace)
        {
            if (registrace == null)
                return Json("Něco se pokazilo.");

            registrace.Jmeno = registrace.Jmeno.Trim();
            registrace.Heslo = registrace.Heslo.Trim();
            registrace.HesloZnovu = registrace.HesloZnovu.Trim();

            if (registrace.Jmeno.Length == 0)
                return Json("Nevyplněné jméno.");
            if (registrace.Heslo.Length == 0 || registrace.HesloZnovu.Length == 0)
                return Json("Nevyplněné heslo.");

            if (registrace.Heslo != registrace.HesloZnovu)
                return Json("Hesla se neshodují.");

            string status = _dbUzivatele.PridatNovehoUzivatele(registrace.Jmeno, registrace.Heslo);

            if (status != "pridan")
                return Json(status);

            return Json("aaa");
        }

        [HttpPost]
        public IActionResult Prihlasit([FromBody] DataPrihlaseni prihlaseni)
        {
            if (prihlaseni == null)
                return Json("Něco se pokazilo.");

            prihlaseni.Jmeno = prihlaseni.Jmeno.Trim();
            prihlaseni.Heslo = prihlaseni.Heslo.Trim();

            if (prihlaseni.Jmeno.Length == 0 || prihlaseni.Heslo.Length == 0)
                return Json("Nevyplněné údaje.");

            return Json("aaa");
        }
    }
}
