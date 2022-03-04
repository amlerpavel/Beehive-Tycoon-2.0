using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using BeehiveTycoon.Models;
using BeehiveTycoon.Models.Game;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BeehiveTycoon.Controllers
{
    public class UlController : Controller
    {
        [HttpGet]
        public IActionResult Plastev0()
        {
            Hra0 hra = NacistHru0();

            return View(hra);
        }

        [HttpGet]
        public IActionResult JSONHra()
        {
            Hra0 hra0 = NacistHru0();

            return Json(hra0);
        }

        [HttpGet]
        public IActionResult DalsiKolo()
        {
            Hra0 hra0 = NacistHru0();
            hra0.Dalsikolo();
            UlozitHru0(hra0);

            return Json(hra0);
        }
        
        public void UlozitHru0(Hra0 hra)
        {
            HttpContext.Session.SetString("Hra", JsonSerializer.Serialize(hra));

            //HttpContext.Response.Cookies.Append("Hra", JsonSerializer.Serialize(hra), new CookieOptions() { SameSite = SameSiteMode.Lax, HttpOnly = true });
        }
        public Hra0 NacistHru0()
        {
            Hra0 hra;
            
            if (HttpContext.Session.GetString("Hra") == null)
            {
                if (HttpContext.Request.Cookies["Hra"] == null)
                {
                    hra = new Hra0(
                        new Datum(3, 0),
                        new Ul0(
                            "netusim",
                            new List<GeneraceVcel> {
                                new GeneraceVcel(300, 3),
                                new GeneraceVcel(400, 0)
                            },
                            new List<Plastev> {
                                new Plastev(1000)
                            },
                            new List<Ukol>(),
                            new Nepritel(0, "", 0, 0, 0, 0, false, true),
                            0
                        )
                    );
                    UlozitHru0(hra);
                }
                else
                    hra = JsonSerializer.Deserialize<Hra0>(HttpContext.Request.Cookies["Hra"]);
            }
            else
                hra = JsonSerializer.Deserialize<Hra0>(HttpContext.Session.GetString("Hra"));

            return hra;
        }

        /*
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
         */
    }
}
