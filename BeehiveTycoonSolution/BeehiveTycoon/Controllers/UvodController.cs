using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models;

namespace BeehiveTycoon.Controllers
{
    public class UvodController : UlController
    {
        public IActionResult Hrat()
        {
            return View();
        }

        public IActionResult Chyba()
        {
            return View();
        }

        public IActionResult Prohra(string tlacitko)
        {
            Hra hra = NacistHru();

            if (hra.Ul.Vcelstvo > 0)
                return Redirect(Url.Action("Plastev", "Ul"));

            if (tlacitko == "znova")
            {
                hra = null;
                UlozitHru(hra);
                return Redirect(Url.Action("Plastev", "Ul"));
            }

            return View();
        }

        public IActionResult Vyhra(string tlacitko)
        {
            Hra hra = NacistHru();
            if (hra.Vyhra == false)
                return Redirect(Url.Action("Plastev", "Ul"));
            if(tlacitko == "znova")
            {
                hra = null;
                UlozitHru(hra);
                return Redirect(Url.Action("Plastev", "Ul"));
            }
            else if(tlacitko == "pokracovat")
            {
                hra.Vyhra = false;
                UlozitHru(hra);
                return Redirect(Url.Action("Plastev", "Ul"));
            }
            
            return View();
        }
    }
}
