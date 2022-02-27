using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models;
using BeehiveTycoon.Models.Game;
using System.Diagnostics;
using System.Text.Json;

namespace BeehiveTycoon.Controllers
{
    public class UkolyController : UlController
    {
        /*
        public IActionResult Seznam()
        {
            Hra hra = NacistHru();

            if (hra.Ul.Vcelstvo <= 0)
                return Redirect(Url.Action("Prohra", "Uvod"));
            else if (hra.Vyhra == true)
                return Redirect(Url.Action("Vyhra", "Uvod"));

            return View(hra);
        }

        public IActionResult Pridat(int idUkolu)
        {
            Ukol ukol;
            Hra hra = NacistHru();

            if (hra.Ul.Vcelstvo <= 0)
                return Redirect(Url.Action("Prohra", "Uvod"));
            else if (hra.Vyhra == true)
                return Redirect(Url.Action("Vyhra", "Uvod"));

            if (TempData["ukol"] != null)
                ukol = JsonSerializer.Deserialize<Ukol>(Convert.ToString(TempData["ukol"]));
            else
            {
                ukol = hra.SeznamUkolu.Where(u => u.IdUkolu == idUkolu).FirstOrDefault();

                if (!hra.SeznamUkolu.Contains(ukol))
                {
                    string[] mesice = { "Leden", "Prosinec" };
                    string[] mesiceRoj = { "Duben", "Květen", "Červen" };
                    if (!mesice.Contains(hra.Ul.Mesic))
                    {
                        if (idUkolu == 0 && hra.Ul.Mesic != "Listopad" && hra.Ul.Mesic != "Únor")
                        {
                            ukol = new()
                            {
                                IdUkolu = idUkolu,
                                Nazev = "Sbírání pylu",
                                PocetVcel = 1
                            };
                        }
                        else if (idUkolu == 1)
                        {
                            ukol = new()
                            {
                                IdUkolu = idUkolu,
                                Nazev = "Nakladení vajíček",
                                PocetVajicek = 1,
                            };
                        }
                        else if (idUkolu == 2)
                        {
                            ukol = new()
                            {
                                IdUkolu = idUkolu,
                                Nazev = "Vytvoření plástve",
                                PocetPlastvi = 1
                            };
                        }
                        else if (idUkolu == 3)
                        {
                            ukol = new()
                            {
                                IdUkolu = idUkolu,
                                Nazev = "Obrana úlu",
                                PocetVcel = 1
                            };
                        }
                        else if(idUkolu == 4 && hra.Ul.Mesic == "Listopad")
                        {
                            ukol = new()
                            {
                                IdUkolu = idUkolu,
                                Nazev = "Zazimování úlu"
                            };
                        }
                        else if (idUkolu == 5 && hra.Ul.Med >= 50000 && hra.Ul.Vcelstvo >= 25000 && mesiceRoj.Contains(hra.Ul.Mesic))
                        {
                            ukol = new()
                            {
                                IdUkolu = idUkolu,
                                Nazev = "Vyrojení včelstva"
                            };
                        }
                        else
                            return Redirect(Url.Action("Plastev", "Ul"));
                    }
                    else
                        return Redirect(Url.Action("Plastev", "Ul"));

                }
            }

            int[] vcelyMed = SecistMedVcelyVUkolech(hra.SeznamUkolu, ukol.IdUkolu);

            if (idUkolu == 0 || idUkolu == 3)
            {
                TempData["DostupnyPocetVcel"] = hra.Ul.Vcelstvo - vcelyMed[0];
            }
            else if (idUkolu == 1)
            {
                int maxVajicekVcely = hra.Ul.Vcelstvo * 2 - vcelyMed[0] * 2;
                int maxVajicekMed = hra.Ul.Med / 2 - vcelyMed[1] / 2;

                if (maxVajicekVcely < maxVajicekMed)
                    TempData["MaxVajicek"] = maxVajicekVcely;
                else
                    TempData["MaxVajicek"] = maxVajicekMed;
            }
            else if (idUkolu == 2)
            {
                double maxPlastviVcely = hra.Ul.Vcelstvo / 100.0 - vcelyMed[0] / 100.0;
                double maxPlastviMed = hra.Ul.Med / 200.0 - vcelyMed[1] / 200.0;

                if (maxPlastviVcely < maxPlastviMed)
                    TempData["MaxPlastvi"] = maxPlastviVcely;
                else
                    TempData["MaxPlastvi"] = maxPlastviMed;
            }

            ViewBag.Ukol = ukol;
            return View(hra);
        }

        public IActionResult CoSUkolem(string tlacitko, int idUkolu, string nazevUkolu, int pocetVcel, int pocetVajicek, int pocetPlastvi)
        {
            Hra hra = NacistHru();
            if (hra.Ul.Vcelstvo <= 0)
                return Redirect(Url.Action("Prohra", "Uvod"));
            else if (hra.Vyhra == true)
                return Redirect(Url.Action("Vyhra", "Uvod"));

            if (tlacitko == "Zrušit úkol")
                return Redirect(Url.Action("ZrusitUkol", "Ukoly", new { idUkolu }));

            if ((pocetVcel <= 0 && idUkolu == 0) || (pocetVajicek <= 0 && idUkolu == 1) || (pocetPlastvi <= 0 && idUkolu == 2))
            {
                TempData["Chyba"] = "Prosím zadejde kladné číslo";
                return Redirect(Url.Action("Pridat", "Ukoly", new { idUkolu }));
            }

            int pocetMedu = 0;
            if (idUkolu == 1)
            {
                double i = pocetVajicek / 2.0;
                pocetVcel = Convert.ToInt32(Math.Round(i, MidpointRounding.AwayFromZero));
                pocetMedu = 2 * pocetVajicek;
            }
            else if (idUkolu == 2)
            {
                double i = pocetPlastvi * 100.0;
                pocetVcel = Convert.ToInt32(Math.Round(i, MidpointRounding.AwayFromZero));
                pocetMedu = 200 * pocetPlastvi;
            }

            int[] vcelyMed = { 0, 0 };
            if (hra.SeznamUkolu.Any())
            {
                vcelyMed = SecistMedVcelyVUkolech(hra.SeznamUkolu, idUkolu);
            }
            if (vcelyMed[0] + pocetVcel > hra.Ul.Vcelstvo || vcelyMed[1] + pocetMedu > hra.Ul.Med)
            {
                TempData["Chyba"] = "Úkol nelze přidat, protože máte nedostatek medu nebo včel.";
                return Redirect(Url.Action("Pridat", "Ukoly", new { idUkolu }));
            }

            if (tlacitko == "Zobrazit požadavky")
            {
                Ukol ukol = new() { IdUkolu = idUkolu, Nazev = nazevUkolu, PocetVcel = pocetVcel, PocetVajicek = pocetVajicek, PocetMedu = pocetMedu, PocetPlastvi = pocetPlastvi };
                TempData["ukol"] = JsonSerializer.Serialize(ukol);

                return Redirect(Url.Action("ZobrazitPozadavkyUkolu", "Ukoly", new { idUkolu, pocetVcel, pocetMedu }));
            }
            else if (tlacitko == "Přidat úkol")
                return Redirect(Url.Action("PridatUkol", "Ukoly", new { idUkolu, nazevUkolu, pocetVcel, pocetVajicek, pocetMedu, pocetPlastvi }));
            else
                return Redirect(Url.Action("ZrusitUkol", "Ukoly", new { idUkolu }));
        }

        public IActionResult ZobrazitPozadavkyUkolu(int idUkolu, int pocetVcel, int pocetMedu)
        {
            List<Pozadavek> pozadavky = new();
            Pozadavek pozadavek = new();
            Pozadavek pozadavek2 = new();

            if (idUkolu == 1 || idUkolu == 2)
            {
                pozadavek = new() { Jmeno = "Včely", Pocet = pocetVcel };
                pozadavek2 = new() { Jmeno = "Med", Pocet = pocetMedu };
            }

            pozadavky.Add(pozadavek);
            pozadavky.Add(pozadavek2);
            TempData["Pozadavky"] = JsonSerializer.Serialize(pozadavky);

            return Redirect(Url.Action("Pridat", "Ukoly", new { idUkolu }));
        }

        public IActionResult PridatUkol(int idUkolu, string nazevUkolu, int pocetVcel, int pocetVajicek, int pocetMedu, int pocetPlastvi)
        {
            Hra hra = NacistHru();

            if (hra.SeznamUkolu.Any())
            {
                Ukol nactenyUkol = hra.SeznamUkolu.Where(u => u.IdUkolu == idUkolu).FirstOrDefault();
                hra.SeznamUkolu.Remove(nactenyUkol);
            }

            Ukol ukol = new()
            {
                IdUkolu = idUkolu,
                Nazev = nazevUkolu,
                PocetVcel = pocetVcel,
                PocetVajicek = pocetVajicek,
                PocetMedu = pocetMedu,
                PocetPlastvi = pocetPlastvi
            };

            hra.SeznamUkolu.Add(ukol);
            UlozitHru(hra);

            return Redirect(Url.Action("Plastev", "Ul"));
        }

        public IActionResult ZrusitUkol(int idUkolu)
        {
            Hra hra = NacistHru();
            Ukol nactenyUkol = hra.SeznamUkolu.Where(u => u.IdUkolu == idUkolu).FirstOrDefault();
            hra.SeznamUkolu.Remove(nactenyUkol);
            UlozitHru(hra);

            return Redirect(Url.Action("Plastev", "Ul"));
        }

        private static int[] SecistMedVcelyVUkolech(List<Ukol> ukoly, int idUkolu)
        {
            int pocetVcel = 0;
            int pocetMedu = 0;

            foreach (Ukol ukol in ukoly)
            {
                if (ukol.IdUkolu != idUkolu)
                {
                    pocetVcel += ukol.PocetVcel;
                    pocetMedu += ukol.PocetMedu;
                }
            }

            int[] vysledky = { pocetVcel, pocetMedu };
            return vysledky;
        }
        */
        public IActionResult Seznam()
        {
            string pokus = "pokus33";

            //Ukol ukol = new Ukol(3, 10);
            //Debug.WriteLine(ukol.Podrobnosti[0].Jmeno + " " + ukol.Podrobnosti[0].Hodnota);

            return Json(pokus);
        }

        [HttpPost]
        public IActionResult Pridat([FromBody] DataUkolu dataUkolu)
        {
            if (dataUkolu == null)
                return Json("Zadejte smysluplné hodnoty");

            if(dataUkolu.Id <= 0 || dataUkolu.Id > 6)
                return Json("Něco se pokazilo... :(");

            if (dataUkolu.Hodnota <= 0 && (dataUkolu.Id == 1 || dataUkolu.Id == 2 || dataUkolu.Id == 3 || dataUkolu.Id == 4))
                return Json("Prosím zadejde kladné číslo");
            
            Hra0 hra = NacistHru0();
            string blaboly = hra.Ul0.PridatUkol(dataUkolu, hra.Datum.CisloMesice);

            if (blaboly != "přisně tajný string")
                return Json(blaboly);

            Debug.WriteLine(JsonSerializer.Serialize(hra.Ul0.SeznamUkolu));
            UlozitHru0(hra);
            
            return Json(hra.Ul0.SeznamUkolu);
        }
    }
}
