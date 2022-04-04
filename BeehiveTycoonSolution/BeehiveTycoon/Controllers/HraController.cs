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
        public IActionResult Nova()
        {
            Hra hra = VytvoritHru();

            return Json(hra);
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

        public Hra VytvoritHru()
        {
            Hra hra = new(
                new Datum(5, 0),
                new List<Ul> {
                    new Ul(
                        new Lokace(
                            "Zahrada",
                            1,
                            new Pyl(
                                new Vystkyt(new int[] { 5 }, new int[] { 4, 6, 7, 8 }, new int[] { 3, 9 }, new int[] { 2, 10 }),
                                new MnostviNaVcelu(8, 6, 3, 0.5)
                            )
                        ),
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
                        false
                    )
                },
                false,
                false
            );

            UlozitHru(hra);

            return hra;
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
                    hra = VytvoritHru();
                else
                    hra = JsonSerializer.Deserialize<Hra>(HttpContext.Request.Cookies["Hra"]);
            }
            else
                hra = JsonSerializer.Deserialize<Hra>(HttpContext.Session.GetString("Hra"));

            return hra;
        }
    }
}
