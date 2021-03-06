using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using BeehiveTycoon.Db;
using Microsoft.AspNetCore.Http;

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
            if (HttpContext.Session.GetString("JmenoUzivatele") != null)
                return Redirect("Profil");

            return View();
        }

        [HttpGet]
        public IActionResult Profil()
        {
            string jmenoUzivatele = HttpContext.Session.GetString("JmenoUzivatele");

            if (jmenoUzivatele == null)
                return Redirect("Prihlaseni");

            ViewData["uzivatel"] = jmenoUzivatele;

            return View();
        }

        [HttpPost]
        public IActionResult Zaregistrovat([FromBody] DataRegistrace dataRegistrace)
        {
            if (dataRegistrace == null)
                return Json("Něco se pokazilo.");

            dataRegistrace.Jmeno = dataRegistrace.Jmeno.Trim();
            dataRegistrace.Heslo = dataRegistrace.Heslo.Trim();
            dataRegistrace.HesloZnovu = dataRegistrace.HesloZnovu.Trim();

            if (dataRegistrace.Jmeno.Length == 0)
                return Json("Nevyplněné jméno.");
            if (dataRegistrace.Heslo.Length == 0 || dataRegistrace.HesloZnovu.Length == 0)
                return Json("Nevyplněné heslo.");
            if (dataRegistrace.Heslo != dataRegistrace.HesloZnovu)
                return Json("Hesla se neshodují.");

            string status = _dbUzivatele.PridatNovehoUzivatele(dataRegistrace.Jmeno, dataRegistrace.Heslo);

            if (status != "pridan")
                return Json(status);

            return Json("zaregistrovan");
        }

        [HttpPost]
        public IActionResult Prihlasit([FromBody] DataPrihlaseni dataPrihlaseni)
        {
            if (dataPrihlaseni == null)
                return Json("Něco se pokazilo.");

            dataPrihlaseni.Jmeno = dataPrihlaseni.Jmeno.Trim();
            dataPrihlaseni.Heslo = dataPrihlaseni.Heslo.Trim();

            if (dataPrihlaseni.Jmeno.Length == 0 || dataPrihlaseni.Heslo.Length == 0)
                return Json("Nevyplněné údaje.");

            if (_dbUzivatele.OveritUzivatele(dataPrihlaseni.Jmeno, dataPrihlaseni.Heslo) == false)
                return Json("Heslo nebo uživatelské jméno není správné.");

            HttpContext.Session.SetString("JmenoUzivatele", dataPrihlaseni.Jmeno);

            return Json("prihlasen");
        }

        [HttpGet]
        public IActionResult Odhlasit()
        {
            HttpContext.Session.Clear();

            return Redirect("/");
        }
    }

    public class DataRegistrace
    {
        public string Jmeno { get; set; }
        public string Heslo { get; set; }
        public string HesloZnovu { get; set; }
    }

    public class DataPrihlaseni
    {
        public string Jmeno { get; set; }
        public string Heslo { get; set; }
    }
}
