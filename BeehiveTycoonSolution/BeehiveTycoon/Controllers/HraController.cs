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
    public class HraController : Controller
    {
        [HttpGet]
        public IActionResult Ul()
        {
            return View();
        }

        [HttpGet]
        public IActionResult JSON()
        {
            Hra hra = NacistHru();

            return Json(hra);
        }

        [HttpGet]
        public IActionResult DalsiKolo()
        {
            Hra hra = NacistHru();
            hra.Dalsikolo();
            UlozitHru(hra);

            return Json(hra);
        }
        
        public void UlozitHru(Hra hra)
        {
            HttpContext.Session.SetString("Hra", JsonSerializer.Serialize(hra));

            //HttpContext.Response.Cookies.Append("Hra", JsonSerializer.Serialize(hra), new CookieOptions() { SameSite = SameSiteMode.Lax, HttpOnly = true });
        }
        public Hra NacistHru()
        {
            Hra hra;
            
            if (HttpContext.Session.GetString("Hra") == null)
            {
                if (HttpContext.Request.Cookies["Hra"] == null)
                {
                    hra = new Hra(
                        new Datum(5, 0),
                        new List<Ul> {
                            new Ul(
                                new Lokace("tady",1),
                                new List<GeneraceVcel> {
                                    new GeneraceVcel(300, 3),
                                    new GeneraceVcel(400, 0)
                                },
                                new List<Plastev> {
                                    new Plastev(1000)
                                },
                                new List<Ukol>(),
                                new Nepritel(0, "", 0, 0, 0, 0, false, true),
                                0,
                                false,
                                false,
                                true
                            ),
                            new Ul(
                                new Lokace("zde",2),
                                new List<GeneraceVcel> {
                                    new GeneraceVcel(700, 3),
                                    new GeneraceVcel(300, 0)
                                },
                                new List<Plastev> {
                                    new Plastev(1000),
                                    new Plastev(1000)
                                },
                                new List<Ukol>(),
                                new Nepritel(0, "", 0, 0, 0, 0, false, true),
                                0,
                                false,
                                false,
                                false
                            )
                        }
                    );
                    UlozitHru(hra);
                }
                else
                    hra = JsonSerializer.Deserialize<Hra>(HttpContext.Request.Cookies["Hra"]);
            }
            else
                hra = JsonSerializer.Deserialize<Hra>(HttpContext.Session.GetString("Hra"));

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
