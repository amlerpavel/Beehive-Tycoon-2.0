using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using BeehiveTycoon.Game;
using BeehiveTycoon.Db;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BeehiveTycoon.Controllers
{
    public class HraController : Controller
    {
        private readonly DbUzivatele _dbUzivatele;
        private readonly DbUlozeneHry _dbUlozeneHry;

        public HraController(Data.BeehiveTycoonContex contex)
        {
            _dbUzivatele = new(contex);
            _dbUlozeneHry = new(contex);
        }

        [HttpGet]
        public IActionResult Ul()
        {
            if (JmenoUzivatele() != null && Pozice() == 0)
                return Redirect("/Uzivatel/Profil");

            return View();
        }

        [HttpPost]
        public IActionResult Pozice([FromBody] int pozice)
        {
            if (pozice <= 0 || pozice > 3)
                return Json("Něco  se pokazilo. :(");
            if (JmenoUzivatele() == null)
                return Redirect("Prihlaseni");

            HttpContext.Session.SetString("Pozice", Convert.ToString(pozice));

            return Redirect("Ul");
        }

        [HttpPost]
        public IActionResult Nova([FromBody] int idObtiznosti)
        {
            if (idObtiznosti <= 0 || idObtiznosti > 3)
                return Json("Něco  se pokazilo. :(");

            string prihlasenyUzivatel = JmenoUzivatele();
            int pozice = Pozice();

            if (prihlasenyUzivatel != null && pozice == 0)
                return Json("neni vybrana pozice");

            Hra hra = VytvoritHru(VytvoritObtiznost(idObtiznosti));
            string postup = JsonSerializer.Serialize(hra);

            if (prihlasenyUzivatel != null)
                _dbUlozeneHry.PridatHru(postup, pozice, prihlasenyUzivatel);
            else
                HttpContext.Response.Cookies.Append("Hra", postup, new CookieOptions() { SameSite = SameSiteMode.Lax, HttpOnly = true });

            return Json(hra);
        }

        [HttpPost]
        public IActionResult Smazat([FromBody] int pozice)
        {
            if (pozice <= 0 || pozice > 3)
                return Json("Něco  se pokazilo. :(");
            if (JmenoUzivatele() == null)
                return Redirect("Prihlaseni");

            _dbUlozeneHry.SmazatHru(pozice, JmenoUzivatele());
            HttpContext.Session.SetString("Pozice", "0");

            return Json("OK");
        }

        [HttpGet]
        public IActionResult Nacist()
        {
            Hra hra = NacistHru();

            return Json(hra);
        }

        [HttpGet]
        public IActionResult DalsiKolo()
        {
            Hra hra = NacistHru();

            if (hra == null)
                return Json(null);

            hra.Dalsikolo();
            UlozitHru(hra);

            return Json(hra);
        }

        [HttpGet]
        public IActionResult Rozehrane()
        {
            List<MUlozenaHra> rozehraneHry = new();
            string jmenouzivatele = JmenoUzivatele();

            if (jmenouzivatele != null)
                rozehraneHry = _dbUzivatele.ZiskatUzivatele(jmenouzivatele).UlozeneHry;

            return Json(rozehraneHry);
        }

        private static Hra VytvoritHru(Obtiznost obtiznost)
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
                            ),
                            0,
                            90,
                            0
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
                        false,
                        25
                    )
                },
                false,
                false,
                obtiznost
            );

            return hra;
        }
        private static Obtiznost VytvoritObtiznost(int idObtiznosti)
        {
            string nazev;
            int pMedu;
            int pNepratel;
            int pMaxOchrana;
            int pZtraceneVcely;

            if (idObtiznosti == 1)
            {
                nazev = "Lehká";
                pMedu = 20;
                pNepratel = -20;
                pMaxOchrana = 15;
                pZtraceneVcely = -30;
            }
            else if (idObtiznosti == 2)
            {
                nazev = "Normální";
                pMedu = 0;
                pNepratel = 0;
                pMaxOchrana = 0;
                pZtraceneVcely = 0;
            }
            else
            {
                nazev = "Těžká";
                pMedu = -20;
                pNepratel = 20;
                pMaxOchrana = -15;
                pZtraceneVcely = 30;
            }

            return new Obtiznost(idObtiznosti, nazev, pMedu, pNepratel, pMaxOchrana, pZtraceneVcely);
        }

        public void UlozitHru(Hra hra)
        {
            string postup = JsonSerializer.Serialize(hra);
            string jmenoUzivatele = JmenoUzivatele();

            if (jmenoUzivatele != null)
                _dbUlozeneHry.AktualizovatHru(postup, Pozice(), jmenoUzivatele);
            else
                HttpContext.Response.Cookies.Append("Hra", postup, new CookieOptions() { SameSite = SameSiteMode.Lax, HttpOnly = true });
        }
        public Hra NacistHru()
        {
            string postup;
            string jmenoUzivatele = JmenoUzivatele();

            if (jmenoUzivatele == null)
            {
                string hraCookie = HttpContext.Request.Cookies["Hra"];

                if (hraCookie == null)
                    postup = null;
                else
                    postup = hraCookie;
            }
            else
                postup = _dbUlozeneHry.ZiskatPostup(Pozice(), jmenoUzivatele);

            Hra hra;

            if (postup != null)
                hra = JsonSerializer.Deserialize<Hra>(postup);
            else
                hra = null;

            return hra;
        }

        private string JmenoUzivatele()
        {
            return HttpContext.Session.GetString("JmenoUzivatele");
        }
        private int Pozice()
        {
            return Convert.ToInt32(HttpContext.Session.GetString("Pozice"));
        }
    }
}
